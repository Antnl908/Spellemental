using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_Heart : MonoBehaviour
{
    //Made by Daniel.

    [SerializeField]
    private Image heartFill;

    [SerializeField]
    private Color fullHealthColor;

    [SerializeField]
    private Color defenseBuffColor;

    private float currentScale = 1f;

    [SerializeField]
    private float scaleChangeRate;

    [SerializeField]
    private GameObject deathUI;

    [SerializeField]
    private TextMeshProUGUI deathText;

    [SerializeField]
    private Image deathIndicator;

    [SerializeField]
    private GameObject healthBuffEffect;

    [SerializeField]
    private Shield_FadeIn_FadeOut shield;

    [SerializeField]
    private int scale;

    // Start is called before the first frame update
    void Start()
    {
        heartFill.fillAmount = 1;

        deathUI.SetActive(false);

        SetColor(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Changes the scale to make it look like the heart beats.
        currentScale += scaleChangeRate * Time.deltaTime;

        transform.localScale = Mathf.Clamp(Mathf.Sin(currentScale), 0.8f, 1.2f) * scale * Vector3.one;
    }

    /// <summary>
    /// Sets the fillAmount in heartFill based on how much health the player has.
    /// </summary>
    /// <param name="currentHealth">The current health of the player</param>
    /// <param name="maxHealth">The maximum health of the player</param>
    public void SetHealth(float currentHealth, float maxHealth)
    {
        if (heartFill != null)
        {
            heartFill.fillAmount = currentHealth / maxHealth;
        }
    }

    /// <summary>
    /// Activates or deactivates the UI which displays that the player is dying.
    /// </summary>
    /// <param name="isDying">Decides whetee the deathUI will be activated or deactivated</param>
    public void SetIfIsDying(bool isDying)
    {
        deathUI.SetActive(isDying);
    }

    /// <summary>
    /// Displays how much time the player has left before they die.
    /// </summary>
    /// <param name="timeUntilDeath">How much time the player has left in seconds</param>
    /// <param name="percentageOfTimeUntilDeath">How much time the player has left as a percentage</param>
    public void SetDeathTimer(float timeUntilDeath, float percentageOfTimeUntilDeath)
    {
        deathText.text = $"Time Until Death: {(int)timeUntilDeath}";

        deathIndicator.fillAmount = percentageOfTimeUntilDeath;
    }

    /// <summary>
    /// Sets the color of the heart. 
    /// Activates the shield if the player has a defense buff. Deactivates the shield if the player has no defense buff.
    /// </summary>
    /// <param name="hasDefenseBuff">Determines wheter or not the player has a defense buff</param>
    public void SetColor(bool hasDefenseBuff)
    {
        if(hasDefenseBuff)
        {
            heartFill.color = defenseBuffColor;

            shield.gameObject.SetActive(true);
        }
        else
        {
            heartFill.color = fullHealthColor;

            shield.Deactivate();
        }
    }

    /// <summary>
    /// Activates the effect which displays that the player has been healed.
    /// </summary>
    public void ActivateHealthBuffEffect()
    {
        healthBuffEffect.SetActive(true);
    }
}
