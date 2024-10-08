using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    private Rigidbody2D rb;
    public string colorName;
    public bool isAllowed;
    public GameObject ExplosionEffect;
    public HashSet<Bubble> connectedMatchedBubbles = new HashSet<Bubble>();

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();  
    }
    private void Update()
    {
        if(rb.velocity == Vector2.zero && transform.position.y < GameManager.Instance.EndLinePoint.transform.position.y)
        {
            GameManager.Instance.isGameOver = true;
            UIManager.Instance.OnGameOver();
            UIManager.Instance.UpdateNotificationText("Game Over");
            return;
        }
    }
    public void OnDestroyBubble()
    {
        Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Bubble") || (collision.transform.CompareTag("Ceiling")))
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
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
