using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Start Screen")] 
    [SerializeField] private GameObject logo;

    [SerializeField] private GameObject startButton;

    [Header("Score")] [SerializeField] private TMP_Text score;

    [Header("Game Ready Session")] 
    [SerializeField] private GameObject gameReadyPanel;

    [Header("Game Over Session")] 
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private TMP_Text gameOverScore;
    [SerializeField] private TMP_Text gameOverBestScore;
    
    [Header("References")] 
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PipeSpawner pipeSpawner;

    private const string BestScoreKey = "BestScore";

    private GameState _gameState = GameState.StartScreen;

    public GameState GameState => _gameState;

    private int _currentScore;

    public int CurrentScore
    {
        get => _currentScore;
        set
        {
            _currentScore = value;
            score.text = _currentScore.ToString();
        }
    }

    public int BestScore
    {
        get => PlayerPrefs.GetInt(BestScoreKey);
        set { PlayerPrefs.SetInt(BestScoreKey, value); }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _gameState = GameState.StartScreen;
        logo.SetActive(true);
        startButton.SetActive(true);

        gameOverPanel.SetActive(false);
        gameReadyPanel.SetActive(false);
        score.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        _gameState = GameState.GameReady;
        CurrentScore = 0;
        
        logo.SetActive(false);
        startButton.SetActive(false);

        gameOverPanel.SetActive(false);
        gameReadyPanel.SetActive(true);
        score.gameObject.SetActive(true);

        ResetGame();
    }

    private void ResetGame()
    {
        playerController.ResetPlayer();
        pipeSpawner.ResetSpawner();
    }

    public void GamePlay()
    {
        _gameState = GameState.Playing;
        gameReadyPanel.SetActive(false);
    }

    public void GameOver()
    {
        _gameState = GameState.GameOver;
        score.gameObject.SetActive(false);
        gameOverPanel.SetActive(true);
        startButton.SetActive(true);
        
        gameOverScore.text = CurrentScore.ToString();
        if (CurrentScore > BestScore)
        {
            BestScore = CurrentScore;
        }
        gameOverBestScore.text = BestScore.ToString();
    }

    public void AddScore()
    {
        if (_gameState != GameState.Playing) return;
        
        CurrentScore++;
    }
}