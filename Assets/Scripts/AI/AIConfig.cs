using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AIConfig", menuName = "Configs")]
public class AIConfig : ScriptableObject
{
    [Header("Speed Values")]
    public float speed = 7f;
    public float runSpeed = 7f;

    [Header("Health Values")]
    public float startHealth = 100f;
    //enum ElementType, om en fiende ska vara av någon slags element, kanske kan knytas samman med enum i Spell så det är enkelt att jämföra dem när man attackerar?

    [Header("Update Intervals Values")]
    public float chaseUpdateTime = 3f;
    public float idleUpdateTime = 3f;

    [Header("Range")]
    public float aggroMinRange = 1.5f;
    public float aggroMaxRange = 10f;

}
