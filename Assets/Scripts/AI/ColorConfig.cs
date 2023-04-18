using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ColorConfig", menuName = "ColorConfig")]
public class ColorConfig : ScriptableObject
{
    private Color color;
    [Header("Resistance Color")]
    public Color fire = Color.red;
    public Color ice = Color.cyan;
    public Color wind = Color.green;
    public Color lightning = Color.yellow;
    public Color earth = new Color(164, 91, 58, 255);

    //[Header("Glow Color")]
    //public Color fireGlow;
    //public Color iceGlow;
    //public Color windGlow;
    //public Color lightningGlow;
    //public Color earthGlow;

    [Header("Glow Amount")]
    public float amount = 5f;
}
