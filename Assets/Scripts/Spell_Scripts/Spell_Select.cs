using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spell_Select : MonoBehaviour
{
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
        UnSelect(true);
        UnSelect(false);
    }

    /// <summary>
    /// Makes this Spell_Select selected if it is the closest to the player out of all the selects.
    /// </summary>
    /// <param name="playerPos">The player's position</param>
    /// <param name="selects">A list of all the selects</param>
    /// <param name="isLeftSprite">Wheter this is selecting the left or right sprite</param>
    /// <returns>True if selected</returns>
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

    /// <summary>
    /// Selects this Spell_Select.
    /// </summary>
    /// <param name="isLeftSprite">Wheter the left or right sprite is selected</param>
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

    /// <summary>
    /// Unselects this Spell_Select.
    /// </summary>
    /// <param name="isLeftSprite">Wheter the left or right sprite is unselected</param>
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
