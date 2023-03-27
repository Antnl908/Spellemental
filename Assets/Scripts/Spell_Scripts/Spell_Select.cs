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

    public bool SelectIfHit(Vector3 playerPos, List<Spell_Select> selects)
    {
        foreach(Spell_Select s in selects)
        {
            if(Vector3.Distance(transform.position, playerPos) > Vector3.Distance(s.transform.position, playerPos))
            {
                return false;
            }
        }

        image.sprite = selectedSprite;

        Debug.Log("Selected spell!");

        return true;
    }

    public void Select()
    {
        image.sprite = selectedSprite;
    }

    public void UnSelect()
    {
        image.sprite = unselectedSprite;
    }
}
