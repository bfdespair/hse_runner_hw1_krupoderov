using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private float difficultyIncreaseInterval = 10f;
    [SerializeField] private float difficultyMultiplier = 1.1f;

    [Header("Score")]
    [SerializeField] private float scorePerSecond = 10f;
    private float score = 0f;

    private float timer = 0f;
    private bool gameOver = false;

    public int Score => Mathf.FloorToInt(score);

    private void Start()
    {
        if (player == null)
            player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (gameOver) return;

        // Увеличиваем сложность
        timer += Time.deltaTime;
        if (timer >= difficultyIncreaseInterval)
        {
            timer = 0f;
            if (player != null)
                player.IncreaseDifficulty(difficultyMultiplier);
        }

        // Счёт
        score += scorePerSecond * Time.deltaTime;
    }

    public void OnPlayerDied()
    {
        if (gameOver) return;
        gameOver = true;

        // Перезапуск сцены
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
