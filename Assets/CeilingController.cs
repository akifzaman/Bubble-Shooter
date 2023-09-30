using UnityEngine;

public class CeilingController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Bubble"))
        {
            collision.transform.GetComponent<Bubble>().isLoose = false;
            GameManager.Instance.CeilingBubbles.Add(collision.transform.GetComponent<Bubble>());
        }
    }
}
