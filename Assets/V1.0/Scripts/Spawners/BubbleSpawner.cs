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
    public float rotationSpeed;
    public float minRotation;
    public float maxRotation;
    public int nextColorIndex;
    public Vector2 leftStickValue;
    public float keyBoardValueLeft;
    public float keyBoardValueRight;
    public float targetRotation;
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
            //Spawner rotation
            keyBoardValueLeft = Keyboard.current.aKey.ReadValue();
            keyBoardValueRight = Keyboard.current.dKey.ReadValue();
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                targetRotation += rotationSpeed;
                targetRotation = Mathf.Clamp(targetRotation, minRotation, maxRotation);
            }
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                targetRotation -= rotationSpeed;
                targetRotation = Mathf.Clamp(targetRotation, minRotation, maxRotation);
            }
            else if (Gamepad.current.leftStick.IsPressed())
            {
                leftStickValue = Gamepad.current.leftStick.ReadValue();
                if(leftStickValue.x < 0)
                {
                    targetRotation += rotationSpeed;
                    targetRotation = Mathf.Clamp(targetRotation, minRotation, maxRotation);
                }
                else
                {
                    targetRotation -= rotationSpeed;
                    targetRotation = Mathf.Clamp(targetRotation, minRotation, maxRotation);
                }
            }

            transform.rotation = Quaternion.Euler(0f, 0f, targetRotation);

            //Bubble shoot
            if (Keyboard.current.spaceKey.wasPressedThisFrame || Gamepad.current.aButton.wasPressedThisFrame)
            {
                OnBubbleShot?.Invoke();
                counter++;
                if (nextColorIndex < Prefabs.Count)
                {
                    var bubble = Instantiate(Prefabs[nextColorIndex], SpawnPosition.position, new Quaternion(targetRotation, 0, 0, 0));
                    bubble.GetComponent<BubbleController>().isAllowed = true;
                    Rigidbody2D rb = bubble.GetComponent<Rigidbody2D>();
                    rb.velocity = new Vector2(-(targetRotation / 90) * 10f, 10f);
                    var randomIndex = Random.Range(0, Prefabs.Count);
                    nextColorIndex = randomIndex;
                    UIManager.Instance.UpdateNextBubbleImage(Prefabs[nextColorIndex]);
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
                        Prefabs.Remove(prefab);
                        var randomIndex = Random.Range(0, Prefabs.Count);
                        nextColorIndex = randomIndex;
                        randomIndex = Random.Range(0, Prefabs.Count);
                        nextColorIndex = randomIndex;
                        if(Prefabs.Count > 0) UIManager.Instance.UpdateNextBubbleImage(Prefabs[nextColorIndex]);
                        break;
                    }
                }
            }
        }
    }


}
