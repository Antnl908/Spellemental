using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class Spell : ScriptableObject
{
    //Made by Daniel.

    [SerializeField]
    protected SpellType type;

    [SerializeField]
    protected int damage;

    [SerializeField]
    protected int effectDamage = 5;

    [SerializeField]
    protected int effectBuildUp = 10;

    [Range(0.001f, 100f)]
    [SerializeField]
    private float destructionTime = 0.01f;

    [SerializeField]
    private float timeBetweenCasts = 0.1f;

    public float TimeBetweenCasts { get => timeBetweenCasts; }

    [SerializeField]
    private int manaCost = 10;

    public int ManaCost { get => manaCost; }

    [SerializeField]
    private float travelDistance = 1f;

    [SerializeField]
    private bool isProjectileNotStationary = true;

    [SerializeField]
    private LayerMask possibleStationarySpawn;

    [SerializeField]
    protected string objectPoolName = "Error";

    public SpellType Type { get => type; }

    [SerializeField]
    private string animationBoolName = "NULL";

    [SerializeField]
    private string leftAnimationName = "NULL";

    public string AnimationBoolName { get => animationBoolName; }

    public string LeftAnimationName { get => leftAnimationName; }

    /// <summary>
    /// If the spell is a projectile then a projectile is taken from the object pool and shot forward.
    /// If the spell is a stationary then it is spawned where the player is looking.
    /// </summary>
    /// <param name="player_Look">A reference to the player to see where they are looking</param>
    /// <param name="position">Position where spell whill be spawned</param>
    /// <param name="rotation">Rotation of the spell</param>
    /// <param name="direction">Direction it will be shot at</param>
    public virtual void CastSpell(Player_Look player_Look, Vector3 position, Quaternion rotation, Vector3 direction)
    {
        if (isProjectileNotStationary)
        {
            Spell_Projectile spawnedProjectile = (Spell_Projectile)Object_Pooler.Pools[objectPoolName].Get();

            spawnedProjectile.Initialize(damage, effectDamage, effectBuildUp, Type, direction * travelDistance,
                                                       position, rotation, Object_Pooler.Pools[objectPoolName], destructionTime);
        }
        else
        {
            if(Physics.Raycast(position, direction, out RaycastHit hitInfo, travelDistance, possibleStationarySpawn))
            {
                SpawnStationary(hitInfo, player_Look);
            }
        }
    }

    /// <summary>
    /// Spawns a stationary spell where the player is looking.
    /// </summary>
    /// <param name="hitInfo">Where the raycast from the player's camera hit the ground</param>
    /// <param name="player_Look">Reference to where the player is looking</param>
    private void SpawnStationary(RaycastHit hitInfo, Player_Look player_Look)
    {
        Spell_Stationary stationary = (Spell_Stationary)Object_Pooler.Pools[objectPoolName].Get();

        Quaternion stationaryRotation;

        if (hitInfo.transform.eulerAngles.x == 0 && hitInfo.transform.eulerAngles.z == 0)
        {
            Quaternion eulerAngles = Quaternion.Euler(hitInfo.normal);

            int additionalDegrees = 0;

            if (hitInfo.transform.position.y > player_Look.transform.position.y)
            {
                additionalDegrees = 180;
            }

            eulerAngles.eulerAngles = new Vector3(eulerAngles.eulerAngles.x + additionalDegrees,
                                                 player_Look.VirtualCamera.transform.eulerAngles.y, eulerAngles.eulerAngles.z);

            stationaryRotation = eulerAngles;
        }
        else
        {
            stationaryRotation = Quaternion.FromToRotation(hitInfo.transform.up, hitInfo.normal) *
                                                                                            hitInfo.transform.rotation;
        }

        stationary.Initialize(hitInfo.point, stationaryRotation, Object_Pooler.Pools[objectPoolName],
                                                                  damage, effectDamage, effectBuildUp, Type, destructionTime);
    }

    public enum SpellType
    {
        Fire,
        Ice,
        Earth,
        Water,
        Lightning,
        Wind,
        None,
    }
}
