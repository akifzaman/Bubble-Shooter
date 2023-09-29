using UnityEngine;

public class CeilingController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Bubble"))
        {
            GameManager.Instance.LooseBubblesCheckerSList.Add(collision.transform.GetComponent<Bubble>());
            //Debug.Log(GameManager.Instance.LooseBubblesCheckerSList.Count);
        }
    }
}
