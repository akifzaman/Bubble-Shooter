using UnityEngine;

public class CeilingController : MonoBehaviour
{
    public float distance;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Bubble"))
        {
            collision.transform.GetComponent<Bubble>().isLoose = false;
            GameManager.Instance.CeilingBubbles.Add(collision.transform.GetComponent<Bubble>());
        }
    }
    public void MoveDownWard()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - distance);
    }
}
