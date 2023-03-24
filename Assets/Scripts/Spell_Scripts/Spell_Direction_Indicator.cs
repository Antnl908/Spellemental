using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spell_Direction_Indicator : MonoBehaviour
{
    //Made by Daniel.

    [SerializeField]
    private Player_Look look;

    [SerializeField]
    private GameObject directionIndicator;

    [Range(0f, 1000f)]
    [SerializeField]
    private float range = 100f;

    [SerializeField]
    private LayerMask detectableSurfaces;

    [SerializeField]
    private Color indicatorColor;

    private void Awake()
    {
        Image[] images = directionIndicator.GetComponentsInChildren<Image>();

        for(int i = 0; i < images.Length; i++)
        {
            images[i].color = indicatorColor;
        }
    }

    private void LateUpdate()
    {
        if (Physics.Raycast(look.VirtualCamera.transform.position, look.VirtualCamera.transform.forward.normalized, 
                                                                                   out RaycastHit hitInfo, range, detectableSurfaces))
        {
            directionIndicator.SetActive(true);

            directionIndicator.transform.SetPositionAndRotation(hitInfo.point + Vector3.up / 10 + Vector3.back / 10, 
                                                                                              Quaternion.LookRotation(hitInfo.normal));
        }
        else
        {
            if (directionIndicator.activeInHierarchy)
            {
                directionIndicator.SetActive(false);
            }            
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, Vector3.forward * range);
    }
}
