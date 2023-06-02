using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Lightning_Hit : Pooling_Object
{
    //Made by Daniel.

    [SerializeField]
    private Transform point1;
    [SerializeField]
    private Transform point2;
    [SerializeField]
    private Transform point3;
    [SerializeField]
    private Transform point4;

    [SerializeField]
    private float destructionTime = 0.5f;

    private float currentDestructionTime;

    [SerializeField]
    private float randomXRange = 2f;
    [SerializeField]
    private float randomYRange = 0.5f;
    [SerializeField]
    private float randomZRange = 2f;

    private IObjectPool<Pooling_Object> pool;

    // Update is called once per frame
    void Update()
    {
        //Releases the object to its pool after a while.
        currentDestructionTime -= Time.deltaTime;

        if(currentDestructionTime <= 0 && enabled)
        {
            pool.Release(this);
        }
    }

    /// <summary>
    /// Sets lightning's starting values.
    /// </summary>
    /// <param name="position">The start position</param>
    /// <param name="rotation">The rotation of the object</param>
    /// <param name="direction">The destination where the lightning will finally land</param>
    /// <param name="pool">The pool the object came from</param>
    public override void Initialize(Vector3 position, Quaternion rotation, Vector3 direction, IObjectPool<Pooling_Object> pool)
    {
        this.pool = pool;

        point1.position = position;

        point2.position = Vector3.Lerp(direction, position, 0.5f);
        point3.position = Vector3.Lerp(direction, position, 0.5f);

        point2.position += new Vector3(Random.Range(-randomXRange, randomXRange + 1), Random.Range(-randomYRange, randomYRange + 1), 
                                                                                        Random.Range(-randomZRange, randomZRange + 1));
        point3.position += new Vector3(Random.Range(-randomXRange, randomXRange + 1), Random.Range(-randomYRange, randomYRange + 1),
                                                                                        Random.Range(-randomZRange, randomZRange + 1));

        point4.position = direction;

        currentDestructionTime = destructionTime;
    }
}
