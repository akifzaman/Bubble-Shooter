using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class BubbleSpawner : MonoBehaviour
{
    public static BubbleSpawner instance;
    public int counter;
    public List<GameObject> Prefabs = new List<GameObject>();
    public Transform SpawnPosition;
    public float rotationSpeed = 45f; // Rotation speed in degrees per second
    public float minRotation = -45f; // Minimum rotation in degrees
    public float maxRotation = 45f;  // Maximum rotation in degrees
    private InputAction moveAction;
    public int nextColorIndex;
    public UnityEvent OnBubbleShot;
    public void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        nextColorIndex = 1;
    }

    private void Update()
    {
        var leftStickValue = Gamepad.current.leftStick.ReadValue();
        float targetRotation = Mathf.Lerp(minRotation, maxRotation, (-leftStickValue.x + 1f) / 2f);
        transform.rotation = Quaternion.Euler(0f, 0f, targetRotation);

        if (Keyboard.current.spaceKey.wasPressedThisFrame || Gamepad.current.aButton.wasPressedThisFrame)
        {
            OnBubbleShot?.Invoke();
            //GameManager.Instance.connectedBubbles.Clear();
            counter++;
            var bubble = Instantiate(Prefabs[nextColorIndex], SpawnPosition.position, new Quaternion(targetRotation,0,0,0));
            bubble.GetComponent<BubbleController>().isAllowed = true;
            Rigidbody2D rb = bubble.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(leftStickValue.x * 10f, Mathf.Max(10, leftStickValue.y * 10f));
            var randomIndex = Random.Range(0, Prefabs.Count);
            nextColorIndex = randomIndex;
            Debug.Log(Prefabs[nextColorIndex]);
        }
    }
}
