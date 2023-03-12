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

    public override void CastSpell(Vector3 position, Quaternion rotation, Vector3 direction)
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
                if (CheckTarget(go, position, direction))
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
            damagable?.TryToDestroyDamageable(damage, null);

        }

        Debug.Log("Count: " + count + " Targets: "+targets.Count);

    }

    bool CheckTarget(GameObject go, Vector3 pos, Vector3 dir)
    {
        if(cone)
        {
            if(IsInSight(go, pos, dir) && IsVisible(go, pos))
            {
                return true;
            }
        }
        else
        {
            if (IsVisible(go, pos))
            {
                return true;
            }
        }
        return false;
    }

    bool IsInSight(GameObject obj, Vector3 pos, Vector3 dir)
    {
        Vector3 sightDir = obj.transform.position - pos;
        if(Vector3.Dot(sightDir, dir) >= radius)
        {
            return true;
        }
        return false;
    }
    bool IsVisible(GameObject obj, Vector3 pos)
    {
        ray.origin = pos;
        ray.direction = obj.transform.position - pos;
        if(Physics.Raycast(ray, out hit, range, layerMask))
        {
            if(hit.collider.gameObject == obj)
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
