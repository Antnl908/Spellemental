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

    HealthBar healthBar;

    [SerializeField]
    private int health;
    
    private int maxHealth;

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
    private ParticleSystem particle;

    [SerializeField]
    private int score = 100;

    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private Transform attackPoint;

    [SerializeField]
    private float attackRadius = 1f;

    [SerializeField] Ragdoll ragdoll;

    NavMeshAgent navMeshAgent;

    [SerializeField]
    private float normalMoveSpeed = 5f;

    [SerializeField]
    private float slowedMoveSpeed = 1f;

    [SerializeField]
    private string hitEffectPool = "Blood_Splatter";

    [SerializeField]
    private string deathEffectPool = "Death_Smoke";

    [SerializeField]
    private Transform visualEffectSpawnPos;

    private bool isDead = false;

    private IObjectPool<Pooling_Object> pool;

    public void KnockBack(float knockBack)
    {
        //throw new System.NotImplementedException();
    }

    //Deals damage to object. Regular damage and effect damage is handled seperately. Destroys object if health reaches 0.
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
        if(healthBar != null) { healthBar.SetHealthAmount((float)health / maxHealth); Debug.Log($"Enemy health:{health} Enemy maxHealth:{maxHealth} current health:{(float)health / maxHealth}"); }
        if(health <= 0)
        {
            Death();

            return true;
        }

        return false;
    }

    void DestroySelf()
    {
        Spawner.CurrentEnemyCount--;

        pool.Release(this);
    }

    //Spawns a damage indicator that shows how much damage was dealt to this enemy.
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
        maxHealth = health;
        if (matInst == null) { matInst = GetComponent<MaterialInstance>(); }
        if(matInst != null) 
        { 
            matInst.albedo = ResColor; 
            matInst.color = ResColor * (colorConfig ? colorConfig.amount : 1f);
            matInst.MeshRenderer = transform.GetComponentsInChildren<MeshRenderer>();
            matInst.SkinMesh = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
            matInst.NewMBP();
        }
        ragdoll = GetComponent<Ragdoll>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        healthBar = GetComponentInChildren<HealthBar>();

        navMeshAgent.speed = normalMoveSpeed;
    }

    private void OnEnable()
    {
        isDead = false;

        if(maxHealth > health)
        {
            health = maxHealth;
        }

        healthBar.SetHealthAmount((float)health / maxHealth);

        if (ragdoll != null)
        {
            navMeshAgent.enabled = true;

            ragdoll.DeactiveteRagdoll();
        }

        effectBuildUp = 0;

        timeUntilEffectDisappears = 0;

        effectIsApplied = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDamageable)
        {
            currentTimeUntilNextHit -= Time.deltaTime;

            if(currentTimeUntilNextHit <= 0)
            {
                isDamageable = true;
            }
        }

        if (!canBeHitByEffect)
        {
            currentTimeUntilNextEffectDamage -= Time.deltaTime;

            if(currentTimeUntilNextEffectDamage <= 0)
            {
                canBeHitByEffect = true;
            }
        }

        if (effectIsApplied)
        {
            TryToDestroyDamageable(effectDamage, null);

            timeUntilEffectDisappears -= Time.deltaTime;

            if(timeUntilEffectDisappears <= 0)
            {
                effectIsApplied = false;
            }
        }

        if(isSlow)
        {
            timeUntilSlowDownDisappears -= Time.deltaTime;

            if(timeUntilSlowDownDisappears <= 0)
            {
                navMeshAgent.speed = normalMoveSpeed;

                isSlow = false;
            }
        }
    }

    //Deals fire damage.
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

    //Deals ice damage.
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

    //Slows down enemy movement.
    public void SlowDownEffect()
    {
        navMeshAgent.speed = slowedMoveSpeed;

        timeUntilSlowDownDisappears = slowDownEffectDuration;

        isSlow = true;

        //Debug.LogWarning("Slow down effect applied");
    }

    //Damage is increased or decreased if enemy is weak or resistant to its type.
    private void ApplyWeaknessOrResistanceToDamage(ref int damage, Spell.SpellType type)
    {
        if(type == weakness)
        {
            damage *= weaknessMultiplier;
        }
        else if(type == resistance)
        {
            damage /= strengthMultiplier;
        }
    }

    //Effect is applied and enemy will take effect damage.
    private void ApplyEffect(int effectDamage)
    {
        this.effectDamage = effectDamage;

        effectBuildUp = 0;

        timeUntilEffectDisappears = effectDuration;

        effectIsApplied = true;
    }

    //Tries to apply an effect of a certain type.
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

    public void Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(attackPoint.position, attackRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                IDamageable damagable = colliders[i].transform.GetComponent<IDamageable>();

                damagable?.TryToDestroyDamageable(damage, null);
            }
        }
    }

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

    void SetMaterial()
    {
        if (material == null) { return; }

        MeshRenderer[] renderers = transform.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer r in renderers)
        {
            r.material = material;
        }

        SkinnedMeshRenderer[] sRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer r in sRenderers)
        {
            r.material = material;
        }
    }

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

    private void Death()
    {
        if (!isDead)
        {
            isDead = true;

            Score_Keeper.AddScore(score);

            PlayEffect(deathEffectPool);

            if (ragdoll != null)
            {
                navMeshAgent.enabled = false;
                ragdoll.ActiveteRagdoll();
                Invoke(nameof(DestroySelf), 3f);
            }
            else
            {
                Spawner.CurrentEnemyCount--;

                pool.Release(this);
            }
        }       
    }

    private void PlayEffect(string poolName)
    {
        Pooled_VFX vfx = (Pooled_VFX)Object_Pooler.Pools[poolName].Get();

        vfx.Initialize(visualEffectSpawnPos.position, visualEffectSpawnPos.rotation, Vector3.zero, Object_Pooler.Pools[poolName]);
    }

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

    public override void Initialize(Vector3 position, Quaternion rotation, Vector3 direction, IObjectPool<Pooling_Object> pool)
    {
        transform.SetPositionAndRotation(position, rotation);

        this.pool = pool;
    }

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
}
