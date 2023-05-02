using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Check out Kiwicoder: https://www.youtube.com/watch?v=oLT4k-lrnwg&t=1195s
//this script was made following this tutorial
public class HealthBar : MonoBehaviour
{
    [SerializeField] protected Transform target;
    [SerializeField] protected Vector3 offset;
    [SerializeField] protected Image foregroundImage;
    [SerializeField] protected Image backgroundImage;
    protected float parentWidth;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        parentWidth = GetComponent<RectTransform>().rect.width;
    }
    // Update is called once per frame
    protected virtual void LateUpdate()
    {
        //convert from world space to screen space
        Vector3 direction = (target.position - Camera.main.transform.position).normalized;
        bool isBehind = Vector3.Dot(direction, Camera.main.transform.forward) <= 0.0f;
        foregroundImage.enabled = !isBehind;
        backgroundImage.enabled = !isBehind;
        transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
    }

    public virtual void SetHealthAmount(float percentage)
    {
        float width = parentWidth * percentage;
        foregroundImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        Debug.Log($"percentage:{percentage} parentWidth:{parentWidth} width:{width}");
    }
}
