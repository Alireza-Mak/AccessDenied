using NUnit.Framework.Internal;
using System.Collections;
using UnityEngine;

public class MineController : BaseExplosive
{


    [Header("Light Settings")]
    [SerializeField] private Light warningLight;
    [SerializeField] private float flashSpeed = 5f;
    [SerializeField] private float minIntensity = 0f;
    [SerializeField] private float maxIntensity = 5f;



    private Coroutine flashRoutine;

    protected override void Start()
    {
        base.Start();
        warningLight.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (isTriggered || !other.CompareTag("Player")) return;

        isTriggered = true;

        warningLight.enabled = true;
        audioSource.PlayOneShot(SoundLibrary.Instance.beep);
        flashRoutine = StartCoroutine(FlashLight());

        StartCoroutine(ExplodeAfterDelay());
    }

    private IEnumerator FlashLight()
    {
        float t = 0f;

        while (true)
        {
            t += Time.deltaTime * flashSpeed;
            float intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(t, 1f));
            warningLight.intensity = intensity;
            yield return null;
        }
    }

    protected override void Explode()
    {
        warningLight.enabled = false;
        StopCoroutine(flashRoutine);
        base.Explode();
    }
}