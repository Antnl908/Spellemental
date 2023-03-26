using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Spell Ray", menuName = "Spell Ray")]
public class Spell_Ray : Spell
{
    //Made by Anton Lindeborg.

    [SerializeField] float range;
    int count;
    readonly Collider[] colliders = new Collider[20]; //Used for overlap sphere
    readonly List<GameObject> targets = new(); //Objects that are within the bounds set by the spell settings
    [SerializeField] LayerMask layerMask;
    [SerializeField] bool cone;
    [Range(0f, 1f)]
    [SerializeField] float radius;
    Ray ray = new();
    RaycastHit hit = new();

    public override void CastSpell(Player_Look player_Look, Vector3 position, Quaternion rotation, Vector3 direction)
    {
        //GameObject spawnedProjectile = Instantiate(projectile, position, rotation);

        //spawnedProjectile.GetComponent<Spell_Projectile>().Initialize(damage, effectDamage, effectBuildUp, Type, direction * travelDistance);

        //Destroy(spawnedProjectile, destructionTime);

        count = Physics.OverlapSphereNonAlloc(position, range, colliders, layerMask, QueryTriggerInteraction.Collide);
        targets.Clear();
        if(count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject go = colliders[i].gameObject;
                //if(IsVisible(go))
                //{
                //    targets.Add(go);
                //}
                //if (CheckTarget(go, position, direction))
                //if (CheckTarget(go, ((CapsuleCollider)colliders[i]).height * 0.5f ,player_Look.VirtualCamera.transform.position, player_Look.VirtualCamera.transform.forward))
                if (CheckTarget(go, colliders[i].bounds.size.y * 0.5f ,player_Look.VirtualCamera.transform.position, player_Look.VirtualCamera.transform.forward))
                {
                    targets.Add(go);
                }
            }
        }

        //for (int i = 0; i < colliders.Length; i++)
        //{
        //    if (colliders[i].gameObject != gameObject)
        //    {
        //        IDamageable damagable = colliders[i].transform.GetComponent<IDamageable>();

        //        damagable?.TryToDestroyDamageable(damage, null);
        //    }
        //}

        for (int i = 0; i < targets.Count; i++)
        {
            //if (targets[i] != player)
            //{
            //    IDamageable damagable = targets[i].transform.GetComponent<IDamageable>();
            //    damagable?.TryToDestroyDamageable(damage, null);

            //}
            IDamageable damagable = targets[i].transform.GetComponent<IDamageable>();
            damagable?.TryToDestroyDamageable(damage, Type);

        }

        Debug.Log("Count: " + count + " Targets: "+targets.Count);

    }

    bool CheckTarget(GameObject go, float offset, Vector3 pos, Vector3 dir)
    {
        //pos += Vector3.up * 1.5f;
        if(cone)
        {
            if(IsInSight(go, offset, pos, dir) && IsVisible(go, offset, pos))
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
            Debug.DrawRay(pos, sightDir, Color.blue, 10f);
            Debug.Log("Dot: " + Vector3.Dot(sightDir, dir));
            return true;
        }
        Debug.DrawRay(pos, sightDir, Color.yellow, 10f);
        return false;
    }
    bool IsVisible(GameObject obj, float offset, Vector3 pos)
    {
        ray.origin = pos;
        ray.direction = (obj.transform.position + (Vector3.up * offset) - pos).normalized;
        //Debug.DrawRay(ray.origin, ray.direction, Color.green, 10f);
        //if(Physics.Raycast(ray, out hit, range, layerMask))
        if(Physics.Raycast(ray, out hit, range, layerMask))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 10f);
            Debug.Log("Object name: " + hit.collider.gameObject.name);
            if (hit.collider.gameObject == obj)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 10f);
        return false;
    }

    //Debug
    void OnDrawGizmos()
    {
        foreach(GameObject go in targets)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(go.transform.position, 5f);

        }
    }
}
