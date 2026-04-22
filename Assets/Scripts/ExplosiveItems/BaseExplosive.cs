using System.Collections;
using UnityEngine;

public abstract class BaseExplosive : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] protected float triggerDelay = 0f;
    [SerializeField] protected float explosionRadius = 5f;
    protected float baseExplosionRadius = 5f;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected ParticleSystem explosionParticle;
    private float difficultyDelta = 0.3f;

    protected AudioSource audioSource;
    protected bool isTriggered = false;

    private void Awake()
    {
        Messenger<RaycastHit>.AddListener(GameEvent.PLAYER_BULLET_HIT, OnPlayerBulletHit);
    }
    private void OnDestroy()
    {
        Messenger<RaycastHit>.RemoveListener(GameEvent.PLAYER_BULLET_HIT, OnPlayerBulletHit);
    }
    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        explosionRadius = baseExplosionRadius;
    }

    public void SetExplosionRadius(int value)
    {
        explosionRadius = baseExplosionRadius + (value * difficultyDelta);
    }
    virtual protected void OnPlayerBulletHit(RaycastHit hit) { }
    protected IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(triggerDelay);
        Explode();
    }

    protected virtual void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, targetLayer);

        foreach (Collider hit in hits)
        {
            PlayerCharacter player = hit.GetComponent<PlayerCharacter>();
            if (player != null)
            {
                player.Hit();
            }

            Enemy enemy = hit.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.Die();
            }
        }


        explosionParticle.Play();
        Destroy(explosionParticle.gameObject, 2f);

        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.enabled = false;

        audioSource.PlayOneShot(SoundLibrary.Instance.explosion);
        Destroy(gameObject, SoundLibrary.Instance.explosion.length);
    }

    protected void StartExplosion()
    {
        if (isTriggered) return;
        isTriggered = true;
        StartCoroutine(ExplodeAfterDelay());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}