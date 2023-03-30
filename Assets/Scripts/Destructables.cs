using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructables : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject Partial;
    [SerializeField] private GameObject Destroyed;
    public float health;
    public float partial_health;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool TryToDestroyDamageable(int damage, Spell.SpellType? spellType)
    {
        health -= damage;
        if (health <= partial_health)
        {
            Partial.SetActive(true);
        }
        if (health <= 0)
        {
            Instantiate(Destroyed, transform.position, transform.rotation);
            Destroy(gameObject); }
        return false; }
    
    public void KnockBack(float knockBack)
    { }
    
}
