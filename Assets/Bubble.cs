using UnityEngine;

public class Bubble : MonoBehaviour
{
    public bool isVisited;
    public bool isLoose;
    public Color bubbleColor;
    public string colorName;
    
    private void Start()
    {
        GetComponent<SpriteRenderer>().color = bubbleColor;
        GameManager.Instance.RegisterBubble(this);
        isLoose = true;
        BubbleSpawner.instance.OnBubbleShot.AddListener(() =>
        {
            isVisited = false;
            isLoose = true;
        });
    }
    public void MoveDownWard()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - 1);
    }
}
