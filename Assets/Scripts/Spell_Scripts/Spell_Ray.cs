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
    readonly Collider[] colliders = new Collider[100]; //Used for overlap sphere
    readonly List<GameObject> targets = new(); //Objects that are within the bounds set by the spell settings
    [SerializeField] LayerMask layerMask;
    [SerializeField] bool cone;
    [Range(0f, 1f)]
    [SerializeField] float radius;
    Ray ray = new();
    RaycastHit hit = new();

    [SerializeField]
    private string effectObjectPoolName = "Error";

    [SerializeField]
    private int effectInstanceAmount = 3;

    public override void CastSpell(Player_Look player_Look, Vector3 position, Quaternion rotation, Vector3 direction)
    {
        count = Physics.OverlapSphereNonAlloc(player_Look.VirtualCamera.transform.position, range, colliders, layerMask, QueryTriggerInteraction.Collide);
        targets.Clear();
        if(count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject go = colliders[i].gameObject;

                if (CheckTarget(go, colliders[i].bounds.center, player_Look.VirtualCamera.transform.position, player_Look.VirtualCamera.transform.forward))
                {
                    targets.Add(go);
                }
            }
        }

        //10-04-2023 Daniel changed this for loop so it deals both regular and effect damage.
        //It also now adds to the players kill count and activates a visual effect.
        for (int i = 0; i < targets.Count; i++)
        {
            IMagicEffect magicEffect = targets[i].transform.GetComponent<IMagicEffect>();

            magicEffect?.ApplyMagicEffect(effectDamage, effectBuildUp, Type);

            IDamageable damagable = targets[i].transform.GetComponent<IDamageable>();

            bool gotAKill = false;

            if (damagable != null)
            {
                gotAKill = (bool)(damagable?.TryToDestroyDamageable(damage, Type));
            }

            if (gotAKill)
            {
                Player_Health.killCount++;
            }

            if (effectObjectPoolName != "Error")
            {
                for(int x = 0; x < effectInstanceAmount; x++)
                {
                    Pooling_Object pooling_Object = Object_Pooler.Pools[effectObjectPoolName].Get();

                    pooling_Object.Initialize(position, rotation, targets[i].transform.position, 
                                                                                       Object_Pooler.Pools[effectObjectPoolName]);
                }
            }
        }

    }

    private bool CheckTarget(GameObject go, Vector3 offset, Vector3 pos, Vector3 dir)
    {
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

    private bool IsInSight(GameObject obj, Vector3 offset, Vector3 pos, Vector3 dir)
    {
        Vector3 sightDir = offset - pos;
        
        if (Vector3.Dot(sightDir.normalized, dir.normalized) >= radius)
        {
            return true;
        }

        return false;
    }

    private bool IsVisible(GameObject obj, Vector3 offset, Vector3 pos)
    {
        ray.origin = pos;
        ray.direction = (offset - pos).normalized;
       
        if(Physics.Raycast(ray, out hit, range, layerMask))
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
