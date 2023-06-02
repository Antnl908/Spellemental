using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

[RequireComponent(typeof(MaterialInstance))]
public class Enemy_Health : Pooling_Object, IDamageable, IMagicEffect, IGuaranteedDamage
{
    // Made by Daniel, edited by Andreas J

    private HealthBar healthBar;

    private int health;

    [SerializeField]
    private int maxHealth = 100;

    [SerializeField]
    private Spell.SpellType weakness;

    [SerializeField]
    private Spell.SpellType resistance;

    [SerializeField]
    private string objectPoolName = "Damage_Indicator";

    [SerializeField]
    private float damageIndicatorForce = 10f;

    private bool isDamageable = true;

    private bool canBeHitByEffect = true;

    [SerializeField]
    private float timeUntilNextHit = 0.5f;

    private float currentTimeUntilNextHit = 0;

    [SerializeField]
    private float timeUntilNextEffectDamage = 0.5f;

    private float currentTimeUntilNextEffectDamage = 0;

    private int effectDamage;

    private int effectBuildUp = 0;

    private float timeUntilEffectDisappears = 0;

    [SerializeField] 
    private float effectDuration = 5;

    [SerializeField]
    private float slowDownEffectDuration = 5;

    private float timeUntilSlowDownDisappears = 0;

    private bool effectIsApplied = false;

    private bool isSlow = false;

    private EffectType effectType = EffectType.None;

    [SerializeField]
    private float damageIndicatorDestructiontime = 1f;

    [SerializeField]
    private int weaknessMultiplier = 2;

    [SerializeField]
    private int strengthMultiplier = 2;

    [SerializeField]
    private Material material;
    
    [SerializeField]
    private Color fireRes = Color.red;
    [SerializeField]
    private Color iceRes = Color.cyan;
    [SerializeField]
    private Color windRes = Color.green;
    [SerializeField]
    private Color lightningRes = Color.yellow;
    [SerializeField]
    private Color earthRes = Color.magenta;
    [SerializeField]
    private MaterialInstance matInst;
    
    [SerializeField]
    private ColorConfig colorConfig;

    [SerializeField]
    private int score = 100;

    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private Transform attackPoint;

    [SerializeField]
    private float attackRadius = 1f;

    [SerializeField]
    private LayerMask playerMask;

    private readonly Collider[] playerColliders = new Collider[50];

    private int hitCount;

    [SerializeField] Ragdoll ragdoll;

    NavMeshAgent navMeshAgent;

    [SerializeField]
    private float slowedMoveSpeed = 1f;

    [SerializeField]
    private string hitEffectPool = "Blood_Splatter";

    [SerializeField]
    private string deathEffectPool = "Death_Smoke";

    [SerializeField]
    private Transform visualEffectSpawnPos;

    [SerializeField] 
    private AIConfig config;

    [SerializeField]
    private GameObject speedDownIcon;

    [SerializeField]
    private bool usesNavMeshAgent = true;

    private bool isDead = false;

    public bool IsDead { get => isDead; }

    private IObjectPool<Pooling_Object> pool;

    private Spawner_With_Increasing_Difficulty spawner;

    private bool killedOnceOrMore = false;

    /// <summary>
    /// Not implemented.
    /// </summary>
    /// <param name="knockBack"></param>
    public void KnockBack(float knockBack)
    {
        //throw new System.NotImplementedException();
    }

    float ragdollTimer;
    readonly float ragdollDelay = 7f;

    /// <summary>
    /// Deals damage to the enemy. Regular damage and effect damage are handled seperately.
    /// Returns the enemy to its pool if health reaches 0. Updates the health bar.
    /// </summary>
    /// <param name="damage">How much damage is dealt</param>
    /// <param name="spellType">Which type of damage is dealt. Is equal to null if it is effect damage.</param>
    /// <returns>Returns true if enemy was killed</returns>
    public bool TryToDestroyDamageable(int damage, Spell.SpellType? spellType)
    {
        if (isDamageable && spellType != null)
        {
            int appliedDamage = damage;

            ApplyWeaknessOrResistanceToDamage(ref appliedDamage, (Spell.SpellType)spellType);

            health -= appliedDamage;

            isDamageable = false;

            currentTimeUntilNextHit = timeUntilNextHit;

            SpawnDamageIndicator(appliedDamage);

            if(appliedDamage > 0)
            {
                PlayEffect(hitEffectPool);
            }            
        }
        
        if(canBeHitByEffect && spellType == null)
        {
            health -= damage;

            canBeHitByEffect = false;

            currentTimeUntilNextEffectDamage = timeUntilNextEffectDamage;

            SpawnDamageIndicator(damage);
        }

        if(healthBar != null) 
        { 
            healthBar.SetHealthAmount((float)health / maxHealth);
        }

        if(health <= 0)
        {
            Death();

            return true;
        }

        return false;
    }

    /// <summary>
    /// Returns the enemy to its pool.
    /// </summary>
    void DestroySelf()
    {
        pool.Release(this);
    }

    /// <summary>
    /// Spawns a damage indicator that shows how much damage was dealt to this enemy.
    /// </summary>
    /// <param name="appliedDamage">The amount of damage that was dealt to the enemy</param>
    private void SpawnDamageIndicator(int appliedDamage)
    {
        if(appliedDamage > 0)
        {
            Damage_Indicator indicator = (Damage_Indicator)Object_Pooler.Pools[objectPoolName].Get();

            indicator.SetValues(appliedDamage, damageIndicatorForce, Object_Pooler.Pools[objectPoolName], 
                                                               damageIndicatorDestructiontime, transform.position, transform.rotation);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        healthBar = GetComponentInChildren<HealthBar>();

        speedDownIcon.SetActive(false);
    }

    private void OnEnable()
    {
        isDead = false;

        ragdollTimer = ragdollDelay;

        effectBuildUp = 0;

        timeUntilEffectDisappears = 0;

        effectIsApplied = false;
    }

    // Update is called once per frame
    void Update()
    {
        DamageCountDown();

        EffectDamageCountDown();

        ApplyEffectDamage();

        SlowDownCountDown();

        RagdollCountDown();
    }

    /// <summary>
    /// Counts down until the next time the enemy can be hit by regular damage.
    /// </summary>
    private void DamageCountDown()
    {
        if (!isDamageable)
        {
            currentTimeUntilNextHit -= Time.deltaTime;

            if (currentTimeUntilNextHit <= 0)
            {
                isDamageable = true;
            }
        }
    }

    /// <summary>
    /// Counts down until the next time the enemy can be hit by effect damage.
    /// </summary>
    private void EffectDamageCountDown()
    {
        if (!canBeHitByEffect)
        {
            currentTimeUntilNextEffectDamage -= Time.deltaTime;

            if (currentTimeUntilNextEffectDamage <= 0)
            {
                canBeHitByEffect = true;
            }
        }
    }

    /// <summary>
    /// Applies effect damage.
    /// </summary>
    private void ApplyEffectDamage()
    {
        if (effectIsApplied)
        {
            TryToDestroyDamageable(effectDamage, null);

            timeUntilEffectDisappears -= Time.deltaTime;

            if (timeUntilEffectDisappears <= 0)
            {
                effectIsApplied = false;
            }
        }
    }

    /// <summary>
    /// Counts down until slow down effect is over and removes it when it is over.
    /// </summary>
    private void SlowDownCountDown()
    {
        if (isSlow)
        {
            timeUntilSlowDownDisappears -= Time.deltaTime;

            if (timeUntilSlowDownDisappears <= 0)
            {
                if (usesNavMeshAgent)
                    navMeshAgent.speed = config.speed;

                speedDownIcon.SetActive(false);

                isSlow = false;
            }
        }
    }

    /// <summary>
    /// Counts down until ragdoll is over and the exits the ragdoll state.
    /// </summary>
    private void RagdollCountDown()
    {
        if (ragdoll != null)
        {
            if (ragdoll.IsActivated && !isDead)
            {
                ragdollTimer -= Time.deltaTime;
                navMeshAgent.enabled = false;
                if (ragdollTimer < 0)
                {
                    if (!ragdoll.AdjustPosition())
                    {
                        Death();
                        return;
                    }

                    ragdoll.DeactivateRagdoll();
                    ragdollTimer = ragdollDelay;
                    navMeshAgent.enabled = true;
                }

            }
        }
    }

    /// <summary>
    /// Deals fire damage if effect build up reaches 100%.
    /// </summary>
    /// <param name="fireDamage">How much fire damage is dealt</param>
    /// <param name="effectBuildUp">How large the effect buildup is</param>
    public void FireEffect(int fireDamage, int effectBuildUp)
    {
        this.effectBuildUp += effectBuildUp;

        if(this.effectBuildUp >= 100 && !effectIsApplied)
        {
            ApplyWeaknessOrResistanceToDamage(ref fireDamage, Spell.SpellType.Fire);

            ApplyEffect(fireDamage);

            effectType = EffectType.Fire;

            Debug.Log("Fire effect");
        }
    }

    /// <summary>
    /// Deals ice damage if effect build up reaches 100%.
    /// </summary>
    /// <param name="iceDamage">How much ice damage is dealt</param>
    /// <param name="effectBuildUp">How large the effect buildup is</param>
    public void FrostEffect(int iceDamage, int effectBuildUp)
    {
        this.effectBuildUp += effectBuildUp;

        if (this.effectBuildUp >= 100 && !effectIsApplied)
        {
            ApplyWeaknessOrResistanceToDamage(ref iceDamage, Spell.SpellType.Ice);

            ApplyEffect(iceDamage);

            effectType = EffectType.Ice;

            SlowDownEffect();

            Debug.Log("Ice effect");
        }
    }

    /// <summary>
    /// Slows down enemy movement.
    /// </summary>
    public void SlowDownEffect()
    {
        if (usesNavMeshAgent)
            navMeshAgent.speed = slowedMoveSpeed;

        timeUntilSlowDownDisappears = slowDownEffectDuration;

        speedDownIcon.SetActive(true);

        isSlow = true;
    }

    /// <summary>
    /// Damage is increased if the enemy is weak to its type.
    /// Damage is decreased if the enemy is resistant to its type.
    /// </summary>
    /// <param name="damage">The damage being dealt</param>
    /// <param name="type">The type of damage</param>
    private void ApplyWeaknessOrResistanceToDamage(ref int damage, Spell.SpellType type)
    {
        if(type == weakness)
        {
            damage *= weaknessMultiplier;

            Score_Keeper.AddScore(damage);
        }
        else if(type == resistance)
        {
            damage /= strengthMultiplier;
        }
    }

    /// <summary>
    /// Effect is applied and enemy will take the applied effect damage.
    /// </summary>
    /// <param name="effectDamage">How much damage the effect does</param>
    private void ApplyEffect(int effectDamage)
    {
        this.effectDamage = effectDamage;

        effectBuildUp = 0;

        timeUntilEffectDisappears = effectDuration;

        effectIsApplied = true;
    }

    /// <summary>
    /// Tries to apply an effect of a certain type.
    /// </summary>
    /// <param name="effectDamage">The effect damage</param>
    /// <param name="effectBuildUp">How many percantages of the effect bar is filled</param>
    /// <param name="spellType">Damage type</param>
    public void ApplyMagicEffect(int effectDamage, int effectBuildUp, Spell.SpellType spellType)
    {
        if(isDamageable)
        {
            if (spellType == Spell.SpellType.Fire)
            {
                FireEffect(effectDamage, effectBuildUp);
            }
            else if (spellType == Spell.SpellType.Ice)
            {
                FrostEffect(effectDamage, effectBuildUp);
            }
            else if(spellType == Spell.SpellType.Lightning)
            {
                LightningEffect(effectDamage, effectBuildUp);
            }
            else if(spellType == Spell.SpellType.Earth)
            {
                EarthEffect(effectDamage, effectBuildUp);
            }
            else if(spellType == Spell.SpellType.Wind)
            {
                WindEffect(effectDamage, effectBuildUp);
            }
        }
    }

    /// <summary>
    /// Deals lightning damage if effect build up reaches 100%.
    /// </summary>
    /// <param name="lightningDamage">How much lightning damage is dealt</param>
    /// <param name="effectBuildUp">How large the effect buildup is</param>
    public void LightningEffect(int lightningDamage, int effectBuildUp)
    {
        this.effectBuildUp += effectBuildUp;

        if (this.effectBuildUp >= 100 && !effectIsApplied)
        {
            ApplyWeaknessOrResistanceToDamage(ref lightningDamage, Spell.SpellType.Lightning);

            ApplyEffect(lightningDamage);

            effectType = EffectType.Lightning;

            Debug.Log("Lightning effect");
        }
    }

    /// <summary>
    /// Deals earth damage if effect build up reaches 100%.
    /// </summary>
    /// <param name="earthDamage">How much earth damage is dealt</param>
    /// <param name="effectBuildUp">How large the effect buildup is</param>
    public void EarthEffect(int earthDamage, int effectBuildUp)
    {
        this.effectBuildUp += effectBuildUp;

        if (this.effectBuildUp >= 100 && !effectIsApplied)
        {
            ApplyWeaknessOrResistanceToDamage(ref earthDamage, Spell.SpellType.Earth);

            ApplyEffect(earthDamage);

            effectType = EffectType.Earth;

            SlowDownEffect();

            Debug.Log("Earth effect");
        }
    }

    /// <summary>
    /// Tries to deal damage to the player.
    /// </summary>
    public void Attack()
    {
        hitCount = Physics.OverlapSphereNonAlloc(attackPoint.position, attackRadius, 
                                                             playerColliders, playerMask, QueryTriggerInteraction.Collide);

        for (int i = 0; i < hitCount; i++)
        {
            if (playerColliders[i].gameObject != gameObject)
            {
                IDamageable damagable = playerColliders[i].transform.GetComponent<IDamageable>();

                damagable?.TryToDestroyDamageable(damage, null);
            }
        }
    }

    /// <summary>
    /// Draws attack radius in the editor.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null) { return; }

        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    public enum EffectType
    {
        None,
        Fire,
        Ice,
        Lightning,
        Earth,
        Wind,
    }

    Color ResColor
    {
        get
        {
            Color c = Color.gray;
            switch(resistance)
            {
                case Spell.SpellType.Fire: if (colorConfig != null) { c = colorConfig.fire; } else { c = fireRes; } /*return colorConfig ? c = colorConfig.fire : c = fireRes;*/
                    break;
                case Spell.SpellType.Ice: if (colorConfig != null) { c = colorConfig.ice; } else { c = iceRes; } //return colorConfig ? c = colorConfig.ice : c = iceRes; //c = iceRes;
                    break;
                case Spell.SpellType.Wind: if (colorConfig != null) { c = colorConfig.wind; } else { c = windRes; } //return colorConfig ? c = colorConfig.wind : c = windRes; //c = windRes;
                    break;
                case Spell.SpellType.Lightning: if (colorConfig != null) { c = colorConfig.lightning; } else { c = lightningRes; } //return colorConfig ? c = colorConfig.lightning : c = lightningRes; //c = lightningRes;
                    break;
                case Spell.SpellType.Earth: if (colorConfig != null) { c = colorConfig.earth; } else { c = earthRes; } //return colorConfig ? c = colorConfig.earth : c = earthRes; //c = earthRes;
                    break;
            }

            return c;
        }
    }

    private void OnValidate()
    {
        SetMaterial();
    }

    /// <summary>
    /// Made by Andreas.
    /// </summary>
    void SetMaterial()
    {
        if (material == null) { return; }

        MeshRenderer[] renderers = transform.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer r in renderers)
        {
            if(!r.GameObject().CompareTag("Ignore"))
                r.material = material;
        }

        SkinnedMeshRenderer[] sRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer r in sRenderers)
        {
            r.material = material;
        }
    }

    /// <summary>
    /// Deals guraranteed damage. Is not affected by hit delays.
    /// </summary>
    /// <param name="damage">How much damage will be dealt</param>
    /// <param name="spellType">The type of damage</param>
    /// <returns></returns>
    public bool GuaranteedDamage(int damage, Spell.SpellType? spellType)
    {
        int appliedDamage = damage;

        ApplyWeaknessOrResistanceToDamage(ref appliedDamage, (Spell.SpellType)spellType);

        health -= appliedDamage;

        isDamageable = false;

        SpawnDamageIndicator(appliedDamage);

        PlayEffect(hitEffectPool);

        if (health <= 0)
        {
            Death();

            return true;
        }

        return false;
    }

    /// <summary>
    /// Kills the enemy and increases the players score.
    /// </summary>
    private void Death()
    {
        if (!isDead)
        {
            isDead = true;

            killedOnceOrMore = true;

            Score_Keeper.AddScore(score);

            PlayEffect(deathEffectPool);

            if (ragdoll != null)
            {
                if(spawner != null)
                {
                    spawner.MinusOneEnemy();
                }
                else
                {
                    Debug.LogWarning(gameObject.name + " had no spawner.");
                }

                if (usesNavMeshAgent)
                    navMeshAgent.enabled = false;
                speedDownIcon.SetActive(false);
                ragdoll.ActivateRagdoll();
                Invoke(nameof(DestroySelf), 3f);
            }
            else
            {
                if (spawner != null)
                {
                    spawner.MinusOneEnemy();
                }
                else
                {
                    Debug.LogWarning(gameObject.name + " had no spawner.");
                }

                pool.Release(this);
            }
        }       
    }

    private void PlayEffect(string poolName)
    {
        Pooled_VFX vfx = (Pooled_VFX)Object_Pooler.Pools[poolName].Get();

        vfx.Initialize(visualEffectSpawnPos.position, visualEffectSpawnPos.rotation, Vector3.zero, Object_Pooler.Pools[poolName]);
    }

    /// <summary>
    /// Deals wind damage if effect build up reaches 100%.
    /// </summary>
    /// <param name="windDamage">How much wind damage is dealt</param>
    /// <param name="effectBuildUp">How large the effect buildup is</param>
    public void WindEffect(int windDamage, int effectBuildUp)
    {
        this.effectBuildUp += effectBuildUp;

        if (this.effectBuildUp >= 100 && !effectIsApplied)
        {
            ApplyWeaknessOrResistanceToDamage(ref windDamage, Spell.SpellType.Wind);

            ApplyEffect(windDamage);

            effectType = EffectType.Wind;

            Debug.Log("Fire effect");
        }
    }

    /// <summary>
    /// Gives the enemy its starting values.
    /// </summary>
    /// <param name="position">The position the enemy is spawned at</param>
    /// <param name="rotation">The rotation the enmy is spawned with</param>
    /// <param name="direction">Unused</param>
    /// <param name="pool">The pool that the enemy came from</param>
    public override void Initialize(Vector3 position, Quaternion rotation, Vector3 direction, IObjectPool<Pooling_Object> pool)
    {
        ragdoll = GetComponent<Ragdoll>();

        if (!killedOnceOrMore)
        {
            if (matInst == null) { matInst = GetComponent<MaterialInstance>(); }
            if (matInst != null)
            {
                matInst.albedo = ResColor;
                matInst.color = ResColor * (colorConfig ? colorConfig.amount : 1f);
                matInst.MeshRenderer = transform.GetComponentsInChildren<MeshRenderer>();
                matInst.SkinMesh = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
                matInst.NewMBP();
            }
        }        

        if (ragdoll != null && killedOnceOrMore)
        {
            ragdoll.DeactivateRagdoll();
        }

        if (usesNavMeshAgent)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();


            navMeshAgent.speed = config.speed;
        }      

        transform.SetPositionAndRotation(position, rotation);

        this.pool = pool;
    }

    /// <summary>
    /// Sets the enemy's weakness and resistance. Updates material albedo and color.
    /// </summary>
    /// <param name="weakness">The weakness</param>
    /// <param name="resistance">The resistance</param>
    public void SetWeaknessAndResistance(Spell.SpellType weakness, Spell.SpellType resistance)
    {
        this.weakness = weakness;

        this.resistance = resistance;

        if(matInst != null)
        {
            matInst.albedo = ResColor;
            matInst.color = ResColor * (colorConfig ? colorConfig.amount : 1f);
        }
    }

    /// <summary>
    /// Sets the size and health of the enemy. Also sets which spawner that spawned it.
    /// </summary>
    /// <param name="isLarge">Wheter or not the enemy is extra large</param>
    /// <param name="sizeMultiplier">How much larger the enemy will be if the enemy is extra large</param>
    /// <param name="extraHealth">How much extra health the nemey will be given</param>
    /// <param name="spawner">The spawner that spawned the enemy</param>
    public void SetSizeAndSpawner(bool isLarge, float sizeMultiplier, int extraHealth, Spawner_With_Increasing_Difficulty spawner)
    {
        this.spawner = spawner;       

        if (usesNavMeshAgent)
            navMeshAgent.enabled = true;

        if (isLarge)
        {
            health = maxHealth * 2 + extraHealth;

            transform.localScale = new Vector3(sizeMultiplier, sizeMultiplier, sizeMultiplier);
        }
        else
        {
            health = maxHealth + extraHealth;

            transform.localScale = Vector3.one;
        }

        if (healthBar != null) { healthBar.SetHealthAmount((float)health / maxHealth); }
    }
}
