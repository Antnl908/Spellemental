using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice_Blast : MonoBehaviour
{
    [SerializeField]
    private Spell_Projectile projectile;

    [SerializeField]
    private List<Spell_Projectile> iceShards;

    [SerializeField]
    private List<Vector3> localPositions;

    [SerializeField]
    private float force = 10f;

    void Awake()
    {
        projectile.OnInitialisation += SetShardStats;
    }

    /// <summary>
    /// Puts all ice shards in their respective positions.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SetShardStats(object sender, EventArgs e)
    {
        for(int i = 0; i < iceShards.Count; i++)
        {
            iceShards[i].transform.localPosition = localPositions[i];

            iceShards[i].Initialize(projectile.Damage, projectile.EffectDamage, projectile.EffectBuildUp, projectile.SpellType,
                transform.forward * force, iceShards[i].transform.position, transform.rotation, projectile.Pool, 100);
        }
    }
}
