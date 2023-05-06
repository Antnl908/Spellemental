using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Health : MonoBehaviour, IDamageable
{
    //Made by Daniel.

    private int currentHealth;

    [SerializeField]
    private int maxHealth;

    public static int killCount = 0;

    [SerializeField]
    private Player_Heart heart;

    private static bool isDying = false;

    [SerializeField]
    private float timeUntilDeath = 10f;

    private float currentTimeUntilDeath;

    private static bool hasDefenseBuff = false;

    private static bool giveHeartDefenseBuffColor = false;

    private static bool hasHealthBuff = false;

    private static bool applyHealthBuffEffect = false;

    [SerializeField]
    private Player_Look player_Look;

    public void KnockBack(float knockBack)
    {
        //throw new System.NotImplementedException();
    }

    public bool TryToDestroyDamageable(int damage, Spell.SpellType? spellType)
    {
        if (hasDefenseBuff)
        {
            hasDefenseBuff = false;

            heart.SetColor(false);
        }
        else
        {
            currentHealth = Math.Clamp(currentHealth - damage, 0, maxHealth);

            if (currentHealth <= 0)
            {
                Dying();
            }
        }       

        return isDying;
    }

    // Start is called before the first frame update
    void Start()
    {
        SecondWind();
    }

    // Update is called once per frame
    void Update()
    {
        if(hasHealthBuff)
        {
            SecondWind();
        }

        if(applyHealthBuffEffect)
        {
            heart.ActivateHealthBuffEffect();

            applyHealthBuffEffect = false;
        }

        if(isDying)
        {
            if(killCount > 0)
            {
                SecondWind();
            }

            currentTimeUntilDeath -= Time.deltaTime;

            heart.SetDeathTimer(currentTimeUntilDeath, currentTimeUntilDeath / timeUntilDeath);

            if(currentTimeUntilDeath <= 0)
            {
                player_Look.HideCursor = false;

                SceneManager.LoadScene("Death_Scene");
            }
        }

        if (giveHeartDefenseBuffColor)
        {
            heart.SetColor(true);

            giveHeartDefenseBuffColor = false;
        }
        
        heart.SetHealth(currentHealth, maxHealth);
    }

    private void SecondWind()
    {
        isDying = false;

        killCount = 0;

        currentHealth = maxHealth;

        heart.SetIfIsDying(false);
    }

    private void Dying()
    {
        killCount = 0;

        isDying = true;

        currentTimeUntilDeath = timeUntilDeath;

        heart.SetIfIsDying(true);
    }

    public static void GiveDefenseBuff()
    {
        if (!isDying)
        {
            hasDefenseBuff = true;
            giveHeartDefenseBuffColor = true;
        }
    }

    public static void GiveHealthBuff()
    {
        if(isDying)
        {
            hasHealthBuff = true;
        }

        applyHealthBuffEffect = true;
    }
}
