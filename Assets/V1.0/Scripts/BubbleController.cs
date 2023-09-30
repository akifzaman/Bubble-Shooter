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
        if (collision.transform.CompareTag("Bubble") || (collision.transform.CompareTag("Ceiling")))
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            //rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        if (collision.transform.CompareTag("Bubble") && isAllowed && !collision.transform.GetComponent<Bubble>().isVisited)
        {
            Bubble bc = collision.transform.GetComponent<Bubble>();
            if (transform.GetComponent<Bubble>().colorName != bc.colorName)
            {
                isAllowed = false;
                return;
            }
            GameManager.Instance.Bubbles.Clear();
            GameManager.Instance.connectedBubbles.Clear();
            GameManager.Instance.Bubbles.Push(bc);
            GameManager.Instance.CheckAndPopNeighbors();
            isAllowed = false;
            return;
        }
    }
}
