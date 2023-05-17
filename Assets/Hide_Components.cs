using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide_Components : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;

    public void Hide_Arrow()
    {
        mesh.enabled = false;
    }

    public void Show_Arrow()
    {
        mesh.enabled = true;
    }
}
