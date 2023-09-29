using UnityEngine;

public class CeilingController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Bubble"))
        {
            GameManager.Instance.CeilingBubbles.Add(collision.transform.GetComponent<Bubble>());
        }
    }
}
