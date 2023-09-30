using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject GameOverPanel;
    public Image NextBubbleImage;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI NotificationText;

    public void Start()
    {
        NextBubbleImage.gameObject.SetActive(true);
    }
    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void OnGameOver()
    {
        GameOverPanel.SetActive(true);
    }
    public void UpdateNextBubbleImage(Bubble bubble)
    {
        NextBubbleImage.sprite = bubble.bubbleIcon;
    }
    public void OnScoreUpdate(int value)
    {
        ScoreText.text = $"Score: {value}";
    }
    public void UpdateNotificationText(string text)
    {
        NotificationText.text = text ;
    }
}
