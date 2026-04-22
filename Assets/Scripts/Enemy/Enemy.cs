using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
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

    [Header("Enemy Setting")]
    public float IdleTime = 3.0f;
    public float ChaseRange = 40f;
    public float AttackRange = 20f;

    [Header("Weapon Setting")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] WeaponManager WeaponManager;
    [SerializeField] AudioSource audioSrc;

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
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
    }

    private void DeadEvent()
    {
        Destroy(this.gameObject, 1f);
    }

    private void OnDrawGizmos()
    {
        //Draw a sphere to show chase range in Scene
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);
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
}
