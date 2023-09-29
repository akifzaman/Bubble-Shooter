using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public HashSet<Bubble> BubblesInBoard = new HashSet<Bubble>();
    public Stack<Bubble> Bubbles = new Stack<Bubble>();
    public Stack<Bubble> LooseBubblesCheckerStack = new Stack<Bubble>();
    public List<Bubble> LooseBubblesCheckerSList = new List<Bubble>();
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
        foreach (var item in BubblesInBoard)
        {
            item.isVisited = false;
        }
    }
    public void CheckAndPopNeighbors(string bubbleColor)
    {
        Debug.Log(bubbleColor);
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
            Collider2D[] neighbors = Physics2D.OverlapCircleAll(currentBubble.transform.position, 0.51f);

            for(int i = 0; i < neighbors.Length; i++)
            {
                Bubble neighborBubble = neighbors[i].GetComponent<Bubble>();
                if (neighborBubble == null) continue;
                else if (neighborBubble.CompareTag("Bubble") && neighborBubble.colorName == bubbleColor)
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
                //BubblesInBoard.Remove(currentBubble);
                Destroy(currentBubble.gameObject);
            }
        }
        //StartCoroutine(DelayForHalfSecond());
    }

    IEnumerator DelayForHalfSecond()
    {
        yield return new WaitForSeconds(10f);
        IdentifyLooseBubbleAndPop();
        MakeAllBubbleVisitable();
    }
    public void IdentifyLooseBubbleAndPop()
    {
        foreach (var item in LooseBubblesCheckerSList)
        {
            item.isLoose = false;
            LooseBubblesCheckerStack.Push(item);
            while (LooseBubblesCheckerStack.Count > 0)
            {
                Bubble currentBubble = LooseBubblesCheckerStack.Pop();
                if (currentBubble.isVisited)
                {
                    if (LooseBubblesCheckerStack.Count > 0) currentBubble = LooseBubblesCheckerStack.Pop();
                    continue;
                }
                Collider2D[] neighbors = Physics2D.OverlapCircleAll(item.transform.position, 0.45f);

                foreach (Collider2D neighbor in neighbors)
                {
                    Bubble neighborBubble = neighbor.GetComponent<Bubble>();
                    if (neighborBubble == null) continue;
                    else if (neighborBubble.CompareTag("Bubble"))
                    {
                        if (!neighborBubble.isVisited)
                        {
                            neighborBubble.isVisited = true;
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
                if(item.gameObject != null) Destroy(item.gameObject);
            }
        }
    }
}
