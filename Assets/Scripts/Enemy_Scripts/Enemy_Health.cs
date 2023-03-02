using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour, IDamageable
{
    // Made by Daniel.

    [SerializeField]
    private int health;

    [SerializeField]
    private Spell.SpellType weakness;

    [SerializeField]
    private Spell.SpellType resistance;

    [SerializeField]
    private GameObject damageIndicator;

    [SerializeField]
    private float damageIndicatorForce = 10f;

    private bool isDamageable = true;

    private const float timeUntilNextHit = 0.1f;

    private float currentTimeUntilNextHit = 0;

    public void KnockBack(float knockBack)
    {
        //throw new System.NotImplementedException();
    }

    public void TakeDamage(int damage, Spell.SpellType spellType)
    {
        if (isDamageable)
        {
            int appliedDamage = damage;

            if (spellType == weakness)
            {
                appliedDamage *= 2;
            }
            else if (spellType == resistance)
            {
                appliedDamage /= 2;
            }

            health -= appliedDamage;

            isDamageable = false;

            currentTimeUntilNextHit = timeUntilNextHit;

            GameObject indicator = Instantiate(damageIndicator, transform.position, Quaternion.Euler(transform.eulerAngles));

            indicator.GetComponentInChildren<Damage_Indicator>().SetValues(appliedDamage, damageIndicatorForce);

            Destroy(indicator, 1);
        }

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
    }
}
