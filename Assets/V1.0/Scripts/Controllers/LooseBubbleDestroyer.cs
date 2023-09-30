using UnityEngine;

public class LooseBubbleDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Bubble")) Destroy(collision.gameObject);
    }
}
