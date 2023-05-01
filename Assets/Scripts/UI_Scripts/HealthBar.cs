using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Check out Kiwicoder: https://www.youtube.com/watch?v=oLT4k-lrnwg&t=1195s
//this script was made following this tutorial
public class HealthBar : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] Image foregroundImage;
    [SerializeField] Image backgroundImage;
    float parentWidth;
    // Start is called before the first frame update
    private void Start()
    {
        parentWidth = GetComponent<RectTransform>().rect.width;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        //convert from world space to screen space
        Vector3 direction = (target.position - Camera.main.transform.position).normalized;
        bool isBehind = Vector3.Dot(direction, Camera.main.transform.forward) <= 0.0f;
        foregroundImage.enabled = !isBehind;
        backgroundImage.enabled = !isBehind;
        transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
    }

    public void SetHealthAmount(float percentage)
    {
        float width = parentWidth * percentage;
        foregroundImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }
}
