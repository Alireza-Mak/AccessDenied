using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] private Light spotLight;
    [SerializeField] private Light pointLight;
    [SerializeField] private float minSpotIntensity = 10f;
    [SerializeField] private float minPointIntensity = 0.5f;
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float speedRotation = 0.5f;

    void Update()
    {
        spotLight.intensity = minSpotIntensity + Mathf.PingPong(Time.time * speed, minSpotIntensity);
        pointLight.intensity = minPointIntensity + Mathf.PingPong(Time.time * speed, minPointIntensity);
        spotLight.transform.Rotate(speedRotation * Time.deltaTime, 0f, 0f);
    }
}

