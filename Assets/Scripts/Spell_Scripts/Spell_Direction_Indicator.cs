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
        if(Time.timeScale > 0)
        {
            if (Physics.Raycast(look.VirtualCamera.transform.position, look.VirtualCamera.transform.forward.normalized,
                                                                                   out RaycastHit hitInfo, range, detectableSurfaces))
            {
                directionIndicator.SetActive(true);

                Vector3 position = hitInfo.point + hitInfo.normal.normalized / 10;

                Quaternion rotation = Quaternion.LookRotation(hitInfo.normal);

                directionIndicator.transform.SetPositionAndRotation(position, rotation);
            }
            else
            {
                Deactivate();
            }
        }
        else
        {
            Deactivate();
        }
    }

    private void Deactivate()
    {
        if (directionIndicator.activeInHierarchy)
        {
            directionIndicator.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, Vector3.forward * range);
    }
}
