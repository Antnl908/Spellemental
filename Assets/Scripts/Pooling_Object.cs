using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Any object that inherits from this class can be put into the Object_Pooler.
/// </summary>
public class Pooling_Object : MonoBehaviour
{
    //Made by Daniel.

    /// <summary>
    /// Initialization method that any Pooling_Object can override.
    /// </summary>
    /// <param name="position">The position the object can be spawned at</param>
    /// <param name="rotation">The rotation the object can be spawned with</param>
    /// <param name="direction">The direction the object can be moving in</param>
    /// <param name="pool">The pool that this object came from</param>
    public virtual void Initialize(Vector3 position, Quaternion rotation, Vector3 direction, IObjectPool<Pooling_Object> pool)
    {
        Debug.LogError("Initialize not implemented");
    }
}
