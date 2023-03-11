using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private Spell.SpellType attackType;

    [SerializeField]
    private Transform center;

    [SerializeField]
    private float range = 10;

    [SerializeField]
    private float destructionTime = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        destructionTime -= Time.deltaTime;

        if (destructionTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        CheckHits();
    }

    private void CheckHits()
    {
        Collider[] colliders = Physics.OverlapSphere(center.position, range);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                IDamageable damagable = colliders[i].transform.GetComponent<IDamageable>();

                damagable?.TryToDestroyDamageable(damage, null);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(center.position, range);
    }
}
