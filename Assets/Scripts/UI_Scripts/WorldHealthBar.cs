using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldHealthBar : HealthBar
{
    [SerializeField] Canvas parentCanvas;
    [SerializeField] RectTransform healthBar;
    [SerializeField] bool lookAtCamera = true;
    [SerializeField] bool contstrainYAxis = true;
    [SerializeField] bool useTarget = false;
    Vector3 direction;

    protected override void Start()
    {
        if(parentCanvas != null) { parentCanvas = GetComponentInChildren<Canvas>(); }
        if(healthBar == null) 
        { 
            parentWidth = parentCanvas.GetComponent<RectTransform>().rect.width; 
        } 
        else
        {
            parentWidth = healthBar.GetComponent<RectTransform>().rect.width;
        }
        //parentWidth = healthBar.GetComponent<RectTransform>().rect.width;
    }
    protected override void LateUpdate()
    {
        Direction = Camera.main.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(Direction, Vector3.up);
        if (useTarget) { transform.position = target.position + offset; }
    }

    public Vector3 Direction
    {
        get { return lookAtCamera ? -direction : direction; }
        set { direction = value; if (contstrainYAxis) { direction.y = 0f; } }
    }
}
