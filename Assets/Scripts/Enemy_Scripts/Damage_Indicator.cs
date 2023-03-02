using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Damage_Indicator : MonoBehaviour
{
    private Rigidbody rb;

    private TextMeshProUGUI textMesh;

    public void SetValues(int damage, float force)
    {
        rb = GetComponent<Rigidbody>();
        textMesh = GetComponent<TextMeshProUGUI>();

        textMesh.text = damage.ToString();

        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
    }
}
