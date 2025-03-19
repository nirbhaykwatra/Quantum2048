using System.Collections;
using TMPro;
using UnityEngine;

/*
 * GameManager Class
 * -----------------
 * Singleton that controls the overall game flow and score handling.
 * - Manages score tracking, loading, and saving high scores via PlayerPrefs.
 * - Coordinates the board state: starting a new game, clearing the board, 
 *   and enabling/disabling gameplay when the game ends.
 * - Displays and updates game over screens and score fields.
 */
public class GameManager : MonoBehaviour
{
    // Static singleton instance of GameManager. Ensures only one GameManager exists at a time.
    public static GameManager Instance { get; private set; }

    // Reference to the TileBoard, which handles tile placement and merging.
    [SerializeField] private TileBoard board;

    // Reference to the CanvasGroup used to show/hide the game over screen.
    [SerializeField] private CanvasGroup gameOver;

    // Reference to the TextMeshProUGUI component that displays the current score.
    [SerializeField] private TextMeshProUGUI scoreText;

    // Reference to the TextMeshProUGUI component that displays the highest score.
    [SerializeField] private TextMeshProUGUI hiscoreText;

    // Internal field to track the current game score.
    private int score;

    // Read-only property to retrieve the current game score.
    public int Score => score;

    // Unity's Awake method to handle singleton setup. Ensures that only one instance of GameManager exists.
    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Unity's Start method, called before the first frame update. Automatically starts a new game.
    private void Start()
    {
        NewGame();
    }

    // Initializes a new game session. Resets score, updates display, clears the board, and creates initial tiles.
    public void NewGame()
    {
        // reset score
        SetScore(0);
        hiscoreText.text = LoadHiscore().ToString();

        // hide game over screen
        gameOver.alpha = 0f;
        gameOver.interactable = false;

        // update board state
        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true;
    }

    // Called when the game ends (no more moves or merges). Disables board interactions and fades in the game over screen.
    public void GameOver()
    {
        board.enabled = false;
        gameOver.interactable = true;

        StartCoroutine(Fade(gameOver, 1f, 1f));
    }

    // Coroutine to smoothly fade a CanvasGroup from its current alpha to a specified value.
    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        float duration = 0.5f;
        float from = canvasGroup.alpha;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = to;
    }

    // Increments the current score by the specified amount, then updates score display and high score.
    public void IncreaseScore(int points)
    {
        SetScore(score + points);
    }

    // Internal method to set the current score, update its display, and attempt to save the new high score.
    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();

        SaveHiscore();
    }

    // Saves the current score as a new high score if it exceeds the previously saved high score in PlayerPrefs.
    private void SaveHiscore()
    {
        int hiscore = LoadHiscore();

        if (score > hiscore) {
            PlayerPrefs.SetInt("hiscore", score);
        }
    }

    // Retrieves the stored high score from PlayerPrefs.
    private int LoadHiscore()
    {
        return PlayerPrefs.GetInt("hiscore", 0);
    }
}