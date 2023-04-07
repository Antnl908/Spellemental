using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructables : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject Partial;
    [SerializeField] private GameObject Destroyed;
    [SerializeField] private Renderer render;
    [SerializeField] private Renderer partialRender;
    [SerializeField] private Collider collider;
    private float health;
    public float partial_health;
    [SerializeField] private float deathLength;
    private float timer;
    [SerializeField] public float maxHealth;
    private bool dead;
    ObjectRemover remover;

    // Start is called before the first frame update
    void Start()
    {
        timer = deathLength;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(dead)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                respawn();
            }
        }
    }

    public bool TryToDestroyDamageable(int damage, Spell.SpellType? spellType)
    {
        health -= damage;
        if (health <= partial_health)
        {
            Partial.SetActive(true);
            render.enabled = false;
        }
        if (health <= 0)
        {
            
            remover = Instantiate(Destroyed, transform.position, transform.rotation).GetComponent<ObjectRemover>();
            Partial.SetActive(false);
            collider.enabled = false;
            dead = true;
        }
        return false; }

    private void respawn()
    {
        dead = false;
        render.enabled = true;
        collider.enabled = true;
        health = maxHealth;
        timer = deathLength;
        remover.RemoveObject();
    }
    
    public void KnockBack(float knockBack)
    { }
    
}
