using System.Collections;
using GameEvents;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Sirenix.OdinInspector;

public enum GameModeEnum
{
    NEW_GAME,
    TUNNELING,
    SUPERPOSITION,
    ENTANGLEMENT
}
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

    [FoldoutGroup("UI Callbacks", expanded: true)]
    // Reference to the CanvasGroup used to show/hide the game over screen.
    [SerializeField] private CanvasGroup _gameOver;

    [FoldoutGroup("UI Callbacks", expanded: true)]
    // Reference to the TextMeshProUGUI component that displays the current score.
    [SerializeField] private FloatEventAsset _scoreUpdateEvent;

    [FoldoutGroup("UI Callbacks", expanded: true)]
    // Reference to the TextMeshProUGUI component that displays the highest score.
    [SerializeField] private FloatEventAsset _highScoreUpdateEvent;
    
    [SerializeField] private GameModeObject _gameModeObject;
    private TutorialManager _tutorialManager;
    
    // Read-only property to retrieve the current game score.
    public int Score { get; private set; }
    // Reference to the TileBoard, which handles tile placement and merging.
    private TileBoard _board;


    // Unity's Awake method to handle singleton setup. Ensures that only one instance of GameManager exists.
    private void Awake()
    {
        if (Instance != null && Instance != this) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        
        _board = GetComponentInChildren<TileBoard>();
        _tutorialManager = GetComponent<TutorialManager>();
    }

    // Unity's Start method, called before the first frame update. Automatically starts a new game.
    private void Start()
    {
        switch (_gameModeObject.GameMode)
        {
            case GameModeEnum.NEW_GAME:
                NewGame();
                Debug.Log($"Game Mode is now {_gameModeObject.GameMode}");
                break;
            case GameModeEnum.TUNNELING:
                _tutorialManager.HandleResetTutorialStage();
                _tutorialManager.HandleTunnelingTutorialStageChanged(0);
                Debug.Log($"Game Mode is now {_gameModeObject.GameMode}");
                break;
            case GameModeEnum.SUPERPOSITION:
                _tutorialManager.HandleResetTutorialStage();
                _tutorialManager.HandleSuperpositionTutorialStageChanged(0);
                Debug.Log($"Game Mode is now {_gameModeObject.GameMode}");
                break;
            case GameModeEnum.ENTANGLEMENT:
                _tutorialManager.HandleResetTutorialStage();
                _tutorialManager.HandleEntanglementTutorialStageChanged(0);
                Debug.Log($"Game Mode is now {_gameModeObject.GameMode}");
                break;
        }
    }
    
    #region New Game

    // Initializes a new game session. Resets score, updates display, clears the board, and creates initial tiles.
    public void NewGame()
    {
        Debug.Log("Started a new game.");
        // reset score
        SetScore(0);
        _highScoreUpdateEvent.Invoke(LoadHighScore());
        
        // hide game over screen
        _gameOver.alpha = 0f;
        _gameOver.interactable = false;

        // update board state
        GlobalData.level = "tunnelling1";
        _board.ClearBoard();
        _board.CreateTile();
        _board.CreateTile();
        _board.enabled = true;
    }

    // Called when the game ends (no more moves or merges). Disables board interactions and fades in the game over screen.
    public void GameOver()
    {
        Debug.Log("Game Over.");
        _board.enabled = false;
        _gameOver.interactable = true;

        StartCoroutine(Fade(_gameOver, 1f, 1f));
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
        SetScore(Score + points);
    }

    // Internal method to set the current score, update its display, and attempt to save the new high score.
    private void SetScore(int score)
    {
        Score = score;
        _scoreUpdateEvent.Invoke(Score);

        SaveHighScore();
    }

    // Saves the current score as a new high score if it exceeds the previously saved high score in PlayerPrefs.
    private void SaveHighScore()
    {
        int hiscore = LoadHighScore();

        if (Score > hiscore) {
            PlayerPrefs.SetInt("hiscore", Score);
        }
    }

    // Retrieves the stored high score from PlayerPrefs.
    private int LoadHighScore()
    {
        return PlayerPrefs.GetInt("hiscore", 0);
    }
    
    #endregion
}