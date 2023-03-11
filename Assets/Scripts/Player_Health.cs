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

    private bool isDying = false;

    [SerializeField]
    private float timeUntilDeath = 10f;

    private float currentTimeUntilDeath;

    public void KnockBack(float knockBack)
    {
        //throw new System.NotImplementedException();
    }

    public bool TryToDestroyDamageable(int damage, Spell.SpellType? spellType)
    {
        currentHealth = Math.Clamp(currentHealth - damage, 0, maxHealth);

        if(currentHealth <= 0)
        {
            Dying();
        }

        return isDying;
    }



    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
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
                SceneManager.LoadScene("Death_Scene");
            }
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
}
