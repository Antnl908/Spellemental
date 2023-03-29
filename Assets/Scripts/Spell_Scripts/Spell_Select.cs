using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Spell_Select : MonoBehaviour
{
    private Collider collider;

    [SerializeField]
    private Image imageLeft;

    [SerializeField] 
    private Image imageRight;

    [SerializeField]
    private Sprite selectedLeftSprite;

    [SerializeField]
    private Sprite unselectedLeftSprite;

    [SerializeField]
    private Sprite selectedRightSprite;

    [SerializeField]
    private Sprite unselectedRightSprite;

    [SerializeField]
    private Spell spell;

    public Spell SelectedSpell { get => spell; }

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();

        UnSelect(true);
        UnSelect(false);
    }

    public bool SelectIfHit(Vector3 playerPos, List<Spell_Select> selects, bool isLeftSprite)
    {
        foreach(Spell_Select s in selects)
        {
            if(Vector3.Distance(transform.position, playerPos) > Vector3.Distance(s.transform.position, playerPos))
            {
                return false;
            }
        }

        Select(isLeftSprite);

        return true;
    }

    public void Select(bool isLeftSprite)
    {
        if(isLeftSprite)
        {
            imageLeft.sprite = selectedLeftSprite;
        }
        else
        {
            imageRight.sprite = selectedRightSprite;
        }
    }

    public void UnSelect(bool isLeftSprite)
    {
        if (isLeftSprite)
        {
            imageLeft.sprite = unselectedLeftSprite;
        }
        else
        {
            imageRight.sprite = unselectedRightSprite;
        }
    }
}
