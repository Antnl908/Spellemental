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
                PlaceIndicator(hitInfo);
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

    /// <summary>
    /// Places the indicator on the surface the player is looking at.
    /// </summary>
    /// <param name="hitInfo"></param>
    private void PlaceIndicator(RaycastHit hitInfo)
    {
        directionIndicator.SetActive(true);

        Vector3 position = hitInfo.point + hitInfo.normal.normalized / 10;

        Quaternion rotation = Quaternion.LookRotation(hitInfo.normal);

        directionIndicator.transform.SetPositionAndRotation(position, rotation);
    }

    /// <summary>
    /// Deactivates the indicator
    /// </summary>
    private void Deactivate()
    {
        if (directionIndicator.activeInHierarchy)
        {
            directionIndicator.SetActive(false);
        }
    }

    /// <summary>
    /// Draws a line which shows the reach if the indicator.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, Vector3.forward * range);
    }
}
