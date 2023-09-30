using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class BubbleSpawner : MonoBehaviour
{
    public static BubbleSpawner instance;
    public int counter;
    public List<Bubble> Prefabs = new List<Bubble>();
    public Transform SpawnPosition;
    public float rotationSpeed = 45f; // Rotation speed in degrees per second
    public float minRotation = -45f; // Minimum rotation in degrees
    public float maxRotation = 45f;  // Maximum rotation in degrees
    public int nextColorIndex;
    public Dictionary<string, int> IdenticalBubbles = new Dictionary<string, int>();

    public UnityEvent OnBubbleShot;
    public void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        nextColorIndex = Random.Range(0, Prefabs.Count);
        UIManager.Instance.UpdateNextBubbleImage(Prefabs[nextColorIndex]);
        IdenticalBubbles["blue"] = 0;
        IdenticalBubbles["red"] = 0;
        IdenticalBubbles["yellow"] = 0;
        IdenticalBubbles["green"] = 0;
    }

    private void Update()
    {
        if (Prefabs.Count > 0)
        {
            var leftStickValue = Gamepad.current.leftStick.ReadValue();
            float targetRotation = Mathf.Lerp(minRotation, maxRotation, (-leftStickValue.x + 1f) / 2f);
            transform.rotation = Quaternion.Euler(0f, 0f, targetRotation);

            if (Keyboard.current.spaceKey.wasPressedThisFrame || Gamepad.current.aButton.wasPressedThisFrame)
            {
                OnBubbleShot?.Invoke();
                counter++;
                if (nextColorIndex < Prefabs.Count)
                {
                    var bubble = Instantiate(Prefabs[nextColorIndex], SpawnPosition.position, new Quaternion(targetRotation, 0, 0, 0));
                    bubble.GetComponent<BubbleController>().isAllowed = true;
                    Rigidbody2D rb = bubble.GetComponent<Rigidbody2D>();
                    rb.velocity = new Vector2(leftStickValue.x * 10f, Mathf.Max(10, leftStickValue.y * 10f));
                    var randomIndex = Random.Range(0, Prefabs.Count);
                    nextColorIndex = randomIndex;
                    UIManager.Instance.UpdateNextBubbleImage(Prefabs[nextColorIndex]);
                    //Debug.Log(Prefabs[nextColorIndex]);
                }
                else
                {
                    var randomIndex = Random.Range(0, Prefabs.Count);
                    nextColorIndex = randomIndex;
                    var bubble = Instantiate(Prefabs[nextColorIndex], SpawnPosition.position, new Quaternion(targetRotation, 0, 0, 0));
                    bubble.GetComponent<BubbleController>().isAllowed = true;
                    Rigidbody2D rb = bubble.GetComponent<Rigidbody2D>();
                    rb.velocity = new Vector2(leftStickValue.x * 10f, Mathf.Max(10, leftStickValue.y * 10f));
                    randomIndex = Random.Range(0, Prefabs.Count);
                    nextColorIndex = randomIndex;
                    UIManager.Instance.UpdateNextBubbleImage(Prefabs[nextColorIndex]);
                    //Debug.Log(Prefabs[nextColorIndex]);
                }
            }
        }
    }
    public void ModifyPrefabList()
    {
        IdenticalBubbles.Clear();
        IdenticalBubbles["blue"] = 0;
        IdenticalBubbles["red"] = 0;
        IdenticalBubbles["yellow"] = 0;
        IdenticalBubbles["green"] = 0;
        var activeBubbles = GameObject.FindObjectsOfType<Bubble>().ToList();
        foreach (var item in activeBubbles)
        {
            if(!item.isLoose) IdenticalBubbles[item.colorName]++;
        }
        foreach (var item in IdenticalBubbles)
        {
            if (item.Value == 0)
            {
                foreach (var prefab in Prefabs)
                {
                    if (prefab.colorName == item.Key)
                    {
                        Debug.Log("Removed");
                        Prefabs.Remove(prefab);
                        break;
                    }
                }
            }
        }
    }


}
