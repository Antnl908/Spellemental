using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveConfig", menuName = "WaveConfig")]
public class WaveConfig : ScriptableObject
{
    [Header("Wave Enemies")]
    public int nbrOfBasicSkeletons = 5;
    public int nbrOfFireSkeletons = 1;
    public int nbrOfIceSkeletons = 1;
    public int nbrOfLightningSkeletons = 1;
    public int nbrOfEarthSkeletons = 1;
    public int nbrOfWindSkeletons = 1;

    [Header("Wave Configurations")]
    public float spawnDelay = 5f;
    public float initialSpawnDelay = 5f;
    public float spawnArea = 1f;
}
