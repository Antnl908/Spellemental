using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MaterialInstance))]
public class Enemy_Health : MonoBehaviour, IDamageable, IMagicEffect
{
    // Made by Daniel, edited by Andreas J

    [SerializeField]
    private int health;

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
    private float timeUntilNextHit = 0.1f;

    private float currentTimeUntilNextHit = 0;

    [SerializeField]
    private float timeUntilNextEffectDamage = 0.5f;

    private float currentTimeUntilNextEffectDamage = 0;

    private int effectDamage;

    private int effectBuildUp = 0;

    private float timeUntilEffectDisappears = 0;

    [SerializeField] 
    private float effectDuration = 5;

    private bool effectIsApplied = false;

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
    private ParticleSystem particle;

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
        }
        
        if(canBeHitByEffect && spellType == null)
        {
            health -= damage;

            canBeHitByEffect = false;

            currentTimeUntilNextEffectDamage = timeUntilNextEffectDamage;

            SpawnDamageIndicator(damage);
        }

        if(health <= 0)
        {
            Instantiate(particle, new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z), Quaternion.identity);

            Destroy(gameObject);

            return true;
        }

        return false;
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
        if(matInst == null) { matInst = GetComponent<MaterialInstance>(); }
        if(matInst != null) { matInst.albedo = ResColor; }
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

            Debug.Log("Ice effect");
        }
    }

    //SLows down enemy movement.
    public void SlowDownEffect(float slowDownAmount)
    {
        // Make it later.

        effectType = EffectType.Slowdown;
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

    public enum EffectType
    {
        None,
        Fire,
        Ice,
        Slowdown,
        Lightning,
    }

    Color ResColor
    {
        get
        {
            Color c = Color.gray;
            switch(resistance)
            {
                case Spell.SpellType.Fire: c = fireRes;
                    break;
                case Spell.SpellType.Ice: c = iceRes;
                    break;
                case Spell.SpellType.Wind: c = windRes;
                    break;
                case Spell.SpellType.Lightning: c = lightningRes;
                    break;
                case Spell.SpellType.Earth: c = earthRes;
                    break;
            }

            return c;
        }
    }

    private void OnValidate()
    {
        MeshRenderer[] renderers = transform.GetComponentsInChildren<MeshRenderer>();

        foreach(MeshRenderer r in renderers)
        {
            r.material = material;
        }

        SkinnedMeshRenderer[] sRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer r in sRenderers)
        {
            r.material = material;
        }
    }
}
