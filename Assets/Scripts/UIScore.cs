using UnityEngine;
using TMPro;

public class UIScore : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();

        if (scoreText == null)
            scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (gameManager == null || scoreText == null) return;

        scoreText.text = "Score: " + gameManager.Score.ToString();
    }
}
