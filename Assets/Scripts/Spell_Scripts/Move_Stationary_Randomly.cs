using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Stationary_Randomly : MonoBehaviour
{
    [SerializeField]
    private Spell_Stationary stationary;

    private readonly List<Vector3> positions = new();

    [SerializeField]
    private int positionAmount = 30;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float maxXDistance = 2f;

    [SerializeField]
    private float maxZDistance = 2f;

    [SerializeField]
    private float switchPositionRadius = 1;

    private int currentPosition = 0;

    void Awake()
    {
        stationary.OnInitialisation += BeginMovement;
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            Move();
        }
    }

    /// <summary>
    /// Sets the different positions that the object can randomly move between.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArgs"></param>
    private void BeginMovement(object sender, EventArgs eventArgs) 
    {
        positions.Clear();

        if((transform.eulerAngles.x == 0 && transform.eulerAngles.z == 0)
            || (Math.Abs(transform.eulerAngles.x) == 180)
            || (Math.Abs(transform.eulerAngles.z) == 180))
        {
            for (int i = 0; i < positionAmount; i++)
            {
                positions.Add(new Vector3(UnityEngine.Random.Range(transform.position.x - maxXDistance, 
                                                                   transform.position.x + maxXDistance),
                                                transform.position.y, UnityEngine.Random.Range(transform.position.z - maxZDistance, 
                                                                                                transform.position.z + maxZDistance)));
            }
        }
    }

    /// <summary>
    /// Moves the object between random positions.
    /// </summary>
    private void Move()
    {
        if(positions.Count > 0)
        {
            if (Vector3.Distance(positions[currentPosition], transform.position) < switchPositionRadius)
            {
                currentPosition = UnityEngine.Random.Range(0, positions.Count);

                if(currentPosition > positions.Count)
                {
                    currentPosition = 0;
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, positions[currentPosition], speed * Time.deltaTime);
        }        
    }

    /// <summary>
    /// In the editor, draws the area where the object can move.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(maxXDistance, 0, maxZDistance));
    }
}
