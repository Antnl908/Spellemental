using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_Heart : MonoBehaviour
{
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
        currentScale += scaleChangeRate * Time.deltaTime;

        transform.localScale = Vector3.one * Mathf.Clamp(Mathf.Sin(currentScale), 0.8f, 1.2f);
    }

    public void SetHealth(float currentHealth, float maxHealth)
    {
        if (heartFill != null)
        {
            heartFill.fillAmount = currentHealth / maxHealth;
        }
    }

    public void SetIfIsDying(bool isDying)
    {
        deathUI.SetActive(isDying);
    }

    public void SetDeathTimer(float timeUntilDeath, float percentageOfTimeUntilDeath)
    {
        deathText.text = $"Time Until Death: {(int)timeUntilDeath}";

        deathIndicator.fillAmount = percentageOfTimeUntilDeath;
    }

    public void SetColor(bool hasDefenseBuff)
    {
        if(hasDefenseBuff)
        {
            heartFill.color = defenseBuffColor;
        }
        else
        {
            heartFill.color = fullHealthColor;
        }
    }
}
