using UnityEngine;
using UnityEngine.UI;

public class Fill_FadeIn_FadeOut : MonoBehaviour
{
    private Image fillImage;

    private bool fadeIn = false;

    [SerializeField]
    private float fadeInRate = 10;

    [SerializeField]
    private float fadOutRate = 10;

    private void Awake()
    {
        fillImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        fadeIn = true;

        fillImage.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //The object fades in and then it fades out.
        if (fadeIn)
        {
            fillImage.fillAmount += fadeInRate * Time.deltaTime;

            if(fillImage.fillAmount >= 1)
            {
                fadeIn = false;
            }
        }
        else
        {
            fillImage.fillAmount -= fadOutRate * Time.deltaTime;

            if(fillImage.fillAmount <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
