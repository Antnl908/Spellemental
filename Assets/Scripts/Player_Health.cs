using System;
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

    private const float timeBetweenHits = 0.5f;

    private float currentTimeBetweenHits;

    private bool isDamageable = true;

    public void KnockBack(float knockBack)
    {
        //throw new System.NotImplementedException();
    }

    /// <summary>
    /// Makes the player take damage. Can only damage the player once between each hit delay.
    /// If the player has a shield equipped, then the player takes no damage and loses the shield.
    /// If the player has no shield, then they lose 1 health and enter the dying state.
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="spellType"></param>
    /// <returns></returns>
    public bool TryToDestroyDamageable(int damage, Spell.SpellType? spellType)
    {
        if(isDamageable)
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

            currentTimeBetweenHits = timeBetweenHits;

            isDamageable = false;
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
        //Revives the player if they have been healed.
        if(hasHealthBuff)
        {
            SecondWind();
        }

        //Activates the visual effect that is shown when the player is healed.
        if(applyHealthBuffEffect)
        {
            heart.ActivateHealthBuffEffect();

            applyHealthBuffEffect = false;
        }

        //If the player is in the dying state and they get a kill, then they are brought back to life.
        //If they are in the dying state for more than the timeUntilDeath, then they lose and the death scene is loaded.
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

        //Activates the effect that is shown when the player is given a shield.
        if (giveHeartDefenseBuffColor)
        {
            heart.SetColor(true);

            giveHeartDefenseBuffColor = false;
        }

        //Counts down until the next time the player can be hit.
        if(!isDamageable)
        {
            currentTimeBetweenHits -= Time.deltaTime;

            if(currentTimeBetweenHits <= 0)
            {
                isDamageable = true;
            }
        }
        
        heart.SetHealth(currentHealth, maxHealth);
    }

    /// <summary>
    /// Brings the player back to life when they are in the dying state.
    /// </summary>
    private void SecondWind()
    {
        isDying = false;

        killCount = 0;

        currentHealth = maxHealth;

        hasHealthBuff = false;

        heart.SetIfIsDying(false);
    }

    /// <summary>
    /// Puts the player into the dying state.
    /// </summary>
    private void Dying()
    {
        if(!isDying)
        {
            killCount = 0;

            isDying = true;

            currentTimeUntilDeath = timeUntilDeath;

            heart.SetIfIsDying(true);
        }       
    }

    /// <summary>
    /// Gives the player a shield.
    /// </summary>
    public static void GiveDefenseBuff()
    {
        if (!isDying)
        {
            hasDefenseBuff = true;
            giveHeartDefenseBuffColor = true;
        }
    }

    /// <summary>
    /// Brings the player back to life.
    /// </summary>
    public static void GiveHealthBuff()
    {
        if(isDying)
        {
            hasHealthBuff = true;
        }

        applyHealthBuffEffect = true;
    }
}
