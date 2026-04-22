using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject Player { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public List<Transform> WayPoints { get; private set; }
    public Animator EnemyAC { get; private set; }
    private int waypointIndex = 0;
    private bool isDead = false;
    private const float difficultyDelta = 0.3f;

    [Header("Enemy Setting")]
    public float IdleTime = 3.0f;
    public float ChaseRange = 10f;
    [SerializeField] float DefaultChaseRange = 20f;
    public float AttackRange = 10f;
    [SerializeField] float DefaultAttackRange = 10f;

    [Header("Weapon Setting")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] WeaponManager WeaponManager;
    [SerializeField] AudioSource audioSrc;

    void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        EnemyAC = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        Agent.updateUpAxis = false;
        Player = GameObject.FindGameObjectWithTag("Player");

        WayPoints = new List<Transform>();
        GameObject waypointsParent = transform.parent.Find("WayPoints").gameObject;
        foreach (Transform t in waypointsParent.transform)
        {
            WayPoints.Add(t);
        }
        AttackRange = DefaultAttackRange;
        ChaseRange = DefaultChaseRange;
    }
    public void SetExplosionRadius(int value)
    {
        AttackRange = DefaultAttackRange + (value * difficultyDelta);
        ChaseRange = DefaultChaseRange + (value * difficultyDelta);
    }

    public void Die()
    {

        if (isDead)
        {
            return;
        }
        isDead = true;
        Messenger.Broadcast(GameEvent.ENEMY_DEAD);

        Agent.isStopped = true;
        EnemyAC.SetTrigger("die");
        audioSrc.PlayOneShot(SoundLibrary.Instance.sfxEnemyDead);
    }

    private void DeadEvent()
    {
        Destroy(this.gameObject, 1f);
    }

    public void SetNextWaypoint()
    {
        int newIndex;
        do
        {
            newIndex = Random.Range(0, WayPoints.Count);
        }
        while (waypointIndex == newIndex);
        waypointIndex = newIndex;
    }

    public Vector3 GetCurrentWaypoint()
    {
        //return the current waypoint
        return WayPoints[waypointIndex].position;
    }

    public float GetDistanceFromPlayer()
    {
        return Vector3.Distance(transform.position, Player.transform.position);
    }

    public void ShootEvent()
    {
        WeaponManager.GetCurrentWeapon().PlayMuzzleFlash();

        Vector3 spawnPos = WeaponManager.GetCurrentWeapon().FireSpawnPoint.position;
        Vector3 target = Player.transform.Find("PlayerCamera").transform.position;
        Vector3 direction = (target - spawnPos).normalized;

        Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90f, 0f, 0f);

        GameObject bulletInstance = Instantiate(bulletPrefab, spawnPos, rotation);

        bulletInstance.GetComponent<BulletController>().Initialize(direction);
        audioSrc.PlayOneShot(SoundLibrary.Instance.sfxPistol);
    }

    private void OnDrawGizmos()
    {
        //Draw a sphere to show chase range in Scene
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);
    }
}
