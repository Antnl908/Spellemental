using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spell_Select : MonoBehaviour
{
    private Collider collider;

    [SerializeField]
    private Image image;

    [SerializeField]
    private Sprite selectedSprite;

    [SerializeField]
    private Sprite unselectedSprite;

    [SerializeField]
    private Spell spell;

    public Spell SelectedSpell { get => spell; }

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();

        UnSelect();
    }

    public bool SelectIfContaionPoint(Vector3 point)
    {
        if (collider.bounds.Contains(point))
        {
            image.sprite = selectedSprite;

            return true;
        }

        return false;
    }

    public void UnSelect()
    {
        image.sprite = unselectedSprite;
    }
}
