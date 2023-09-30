using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject GameOverPanel;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI NotificationText;

    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void OnGameOver()
    {
        GameOverPanel.SetActive(true);
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
