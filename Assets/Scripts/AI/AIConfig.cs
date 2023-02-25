using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AIConfig", menuName = "Configs")]
public class AIConfig : ScriptableObject
{
    public float speed = 7f;
    public float startHealth = 100f;
    //enum ElementType, om en fiende ska vara av n�gon slags element, kanske kan knytas samman med enum i Spell s� det �r enkelt att j�mf�ra dem n�r man attackerar?
}
