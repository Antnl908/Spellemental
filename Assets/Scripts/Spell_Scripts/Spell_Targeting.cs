using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Targeting : MonoBehaviour
{
    [SerializeField] float range;
    int count;
    readonly Collider[] colliders = new Collider[30]; //Used for overlap sphere
    readonly List<GameObject> targets = new(); //Objects that are within the bounds set by the spell settings
    [SerializeField] LayerMask layerMask;
    [SerializeField] bool cone;
    [Range(0f, 1f)]
    [SerializeField] float radius;
    Ray ray = new();
    RaycastHit hit = new();
    private Player_Look player_Look;

    [SerializeField] float targetCheckTimer;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        player_Look = GetComponent<Player_Look>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer > 0f) { return; }
        CheckTargets();
        timer = targetCheckTimer;
    }

    public void CheckTargets()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, range, colliders, layerMask, QueryTriggerInteraction.Collide);
        targets.Clear();
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject go = colliders[i].gameObject;

                if (CheckTarget(go, colliders[i].bounds.size.y * 0.5f, player_Look.VirtualCamera.transform.position, player_Look.VirtualCamera.transform.forward))
                {
                    targets.Add(go);
                }
            }
        }

        for (int i = 0; i < targets.Count; i++)
        {
            MaterialInstance instance = targets[i].transform.GetComponent<MaterialInstance>();
            
            instance?.SetGlow(1f);
        }

    }

    bool CheckTarget(GameObject go, float offset, Vector3 pos, Vector3 dir)
    {
        if (cone)
        {
            if (IsInSight(go, offset, pos, dir) && IsVisible(go, offset, pos))
            {
                return true;
            }
        }
        else
        {
            if (IsVisible(go, offset, pos))
            {
                return true;
            }
        }
        return false;
    }

    bool IsInSight(GameObject obj, float offset, Vector3 pos, Vector3 dir)
    {
        Vector3 sightDir = obj.transform.position + (Vector3.up * offset) - pos;

        if (Vector3.Dot(sightDir.normalized, dir.normalized) >= radius)
        {
            return true;
        }

        return false;
    }
    bool IsVisible(GameObject obj, float offset, Vector3 pos)
    {
        ray.origin = pos;
        ray.direction = (obj.transform.position + (Vector3.up * offset) - pos).normalized;
        
        if (Physics.Raycast(ray, out hit, range, layerMask))
        {
            if (hit.collider.gameObject == obj)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        return false;
    }
}
