using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public HashSet<Bubble> BubblesInBoard = new HashSet<Bubble>();
    public Stack<Bubble> Bubbles = new Stack<Bubble>();
    public Stack<Bubble> LooseBubblesCheckerStack = new Stack<Bubble>();
    public List<Bubble> CeilingBubblesList = new List<Bubble>();
    public HashSet<Bubble> connectedBubbles = new HashSet<Bubble>();
    public HashSet<Bubble> CeilingBubbles = new HashSet<Bubble>();
    public void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        BubbleSpawner.instance.OnBubbleShot.AddListener(() =>
        {
            MakeAllBubbleVisitable();
        });
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
            Debug.Log(connectedBubbles.Count);
            foreach (Bubble currentBubble in connectedBubbles)
            {
                if(BubblesInBoard.Contains(currentBubble)) BubblesInBoard.Remove(currentBubble);
                else if(CeilingBubbles.Contains(currentBubble)) CeilingBubbles.Remove(currentBubble);
                Destroy(currentBubble.gameObject);
            }
            StartCoroutine(DelayForHalfSecond());
        }   
    }

    IEnumerator DelayForHalfSecond()
    {
        yield return new WaitForSeconds(0.5f);
        IdentifyLooseBubbleAndPop();
    }
    public void IdentifyLooseBubbleAndPop()
    {
        foreach (var item in CeilingBubbles)
        {
            if(item == null) continue;
            item.isLoose = false;
            LooseBubblesCheckerStack.Push(item);
            while (LooseBubblesCheckerStack.Count > 0)
            {
                Bubble currentBubble = LooseBubblesCheckerStack.Pop();
                if (currentBubble.isLoose)
                {
                    if (LooseBubblesCheckerStack.Count > 0) currentBubble = LooseBubblesCheckerStack.Pop();
                    continue;
                }
                //currentBubble.isVisited = true;
                Collider2D[] neighbors = Physics2D.OverlapCircleAll(currentBubble.transform.position, 0.45f);

                foreach (Collider2D neighbor in neighbors)
                {
                    Bubble neighborBubble = neighbor.GetComponent<Bubble>();
                    if (neighborBubble == null) continue;
                    else if (neighborBubble.CompareTag("Bubble"))
                    {
                        if (neighborBubble.isLoose)
                        {
                            //neighborBubble.isVisited = true;
                            neighborBubble.isLoose = false;
                            LooseBubblesCheckerStack.Push(neighborBubble);
                        }
                    }
                }

            }
        }
        Debug.Log(BubblesInBoard.Count);
        foreach (var item in BubblesInBoard)
        {
            if (item.isLoose)
            {
                Debug.Log(item.colorName);
                if (item.gameObject != null) Destroy(item.gameObject);
                if (BubblesInBoard.Contains(item)) BubblesInBoard.Remove(item);
            }
        }
        //BubblesInBoard.RemoveWhere(bubble => bubble.isLoose == true);
        MakeAllBubbleVisitable();
    }
}
