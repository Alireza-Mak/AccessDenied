using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Transform doorModel;
    [SerializeField] private Light[] doorLights;
    [SerializeField] private int requiredKeys = 1;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 3f;
    public SceneController SceneController { get; private set; }

    private Quaternion closedRotation;
    private Color closedColor;
    private Quaternion openRotation;
    private Color openColor;
    private bool shouldOpen = false;

    private void Start()
    {
        SceneController = GameObject.Find("SceneController").GetComponent<SceneController>();

        closedRotation = doorModel.rotation;
        closedColor = Color.red;
        openColor = Color.green;
        openRotation = closedRotation * Quaternion.Euler(0, 0, openAngle);
    }

    private void Update()
    {
        Quaternion targetRotation = shouldOpen ? openRotation : closedRotation;
        Color targetColor = SceneController.KeyCards == requiredKeys ? openColor : closedColor;
        doorModel.rotation = Quaternion.Slerp(doorModel.rotation, targetRotation, openSpeed * Time.deltaTime);
        foreach (var light in doorLights)
        {
            light.color = targetColor;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        int currentKeys = SceneController.KeyCards;

        if (currentKeys == requiredKeys)
        {
            shouldOpen = true;
        }
        else
        {
            shouldOpen = false;
            Debug.Log("Door locked. Required keys: " + requiredKeys);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        shouldOpen = false;
    }
}
