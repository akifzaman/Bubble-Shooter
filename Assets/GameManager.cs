using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public HashSet<Bubble> BubblesInBoard = new HashSet<Bubble>();
    public Stack<Bubble> Bubbles = new Stack<Bubble>();
    public Stack<Bubble> LooseBubblesCheckerStack = new Stack<Bubble>();
    public HashSet<Bubble> connectedBubbles = new HashSet<Bubble>();
    public List<Bubble> CeilingBubbles = new List<Bubble>();
    public UnityEvent OnDestroyCluster;
    public CeilingController CeilingController;
    public void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        OnDestroyCluster.AddListener(() =>
        {
            IdentifyLooseBubbleAndPop();
        });
        InvokeRepeating("MoveDownWard", 15f, 15f);
    }
    public void MoveDownWard()
    {
        foreach (var item in BubblesInBoard)
        {
            item.MoveDownWard();
        }
        CeilingController.MoveDownWard();
    }
    public void RegisterBubble(Bubble bubble)
    {
        BubblesInBoard.Add(bubble);
    }
    public void MakeAllBubbleVisitable()
    {
        if(BubblesInBoard.Count > 0)
        {
            foreach (var item in BubblesInBoard)
            {
                if (item != null) item.isVisited = false;
            }
        }   
    }
    public void CheckAndPopNeighbors()
    {
        while (Bubbles.Count > 0)
        {
            Bubble currentBubble = Bubbles.Pop();
            connectedBubbles.Add(currentBubble);
            if (currentBubble.isVisited)
            {
                if (Bubbles.Count > 0) currentBubble = Bubbles.Pop();
                continue;
            }
            currentBubble.isVisited = true;
            Collider2D[] neighbors = Physics2D.OverlapCircleAll(currentBubble.transform.position, 0.45f);

            for(int i = 0; i < neighbors.Length; i++)
            {
                Bubble neighborBubble = neighbors[i].GetComponent<Bubble>();
                if (neighborBubble == null) continue;
                else if (neighborBubble.CompareTag("Bubble") && neighborBubble.colorName == currentBubble.colorName)
                {
                    if (!neighborBubble.isVisited)
                    {
                        Bubbles.Push(neighborBubble);
                        connectedBubbles.Add(neighborBubble);
                    }
                }
            }
        }
        if(connectedBubbles.Count >= 3)
        {
            foreach (Bubble currentBubble in connectedBubbles)
            {
                BubblesInBoard.Remove(currentBubble);
                CeilingBubbles.Remove(currentBubble);
                Destroy(currentBubble.gameObject);
            }
            OnDestroyCluster?.Invoke();
        }
    }
    
    public void IdentifyLooseBubbleAndPop()
    {
        foreach (var item in CeilingBubbles)
        {
            if (item == null)
            {
                continue;
            }
            item.isLoose = false;
            LooseBubblesCheckerStack.Push(item);
            while (LooseBubblesCheckerStack.Count > 0)
            {
                Bubble currentBubble = LooseBubblesCheckerStack.Pop();
                Collider2D[] neighbors = Physics2D.OverlapCircleAll(currentBubble.transform.position, 0.45f);

                foreach (Collider2D neighbor in neighbors)
                {
                    Bubble neighborBubble = neighbor.GetComponent<Bubble>();
                    if (neighborBubble == null) continue;
                    else if (neighborBubble.CompareTag("Bubble"))
                    {
                        if (neighborBubble.isLoose)
                        {
                            neighborBubble.isLoose = false;
                            LooseBubblesCheckerStack.Push(neighborBubble);
                        }
                    }
                }
            }
        }
        foreach (var item in BubblesInBoard)
        {
            if (item.isLoose)
            {
                if (item.gameObject != null)
                {
                    item.gameObject.GetComponent<CircleCollider2D>().enabled = false;
                    item.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
                    item.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                    item.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
                    //Destroy(item.gameObject);
                }
            }
        }
        BubblesInBoard.RemoveWhere(bubble => bubble.isLoose == true);
        BubbleSpawner.instance.ModifyPrefabList();
        if (BubbleSpawner.instance.Prefabs.Count == 0)
        {
            Debug.Log("Congratulations!!!");
        }
    }
}
