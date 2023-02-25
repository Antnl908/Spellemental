using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AIConfig", menuName = "Configs")]
public class AIConfig : ScriptableObject
{
    public float speed = 7f;
    public float startHealth = 100f;
    //enum ElementType, om en fiende ska vara av någon slags element, kanske kan knytas samman med enum i Spell så det är enkelt att jämföra dem när man attackerar?
}
