using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    private Rigidbody2D rb;
    public Color bubbleColor;
    public string colorName;
    public bool isAllowed;
    public HashSet<Bubble> connectedMatchedBubbles = new HashSet<Bubble>();

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();  
    }    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation; //for testing purpose only
        if (collision.transform.CompareTag("Bubble") && isAllowed && !collision.transform.GetComponent<Bubble>().isVisited)
        {
            //rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            Bubble bc = collision.transform.GetComponent<Bubble>();
            GameManager.Instance.Bubbles.Push(bc);
            GameManager.Instance.CheckAndPopNeighbors(transform.GetComponent<Bubble>().colorName);
            isAllowed = false;
        }
    }
}
