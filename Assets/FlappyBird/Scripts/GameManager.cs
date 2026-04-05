using System.Collections;
using FlappyBird.Scripts;
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
    
    [Header("Camera Shake")]
    [SerializeField] private float shackDuration = 0.5f;
    [SerializeField] private float shakeAmount= 0.05f;
    
    private const string BestScoreKey = "BestScore";

    private GameState _gameState = GameState.StartScreen;

    public GameState GameState => _gameState;

    private Camera _mainCamera;
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
        _mainCamera = Camera.main;
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
        StartCoroutine(ShakeCamera());
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
        AudioManager.Instance.PlayScore();
    }

    IEnumerator ShakeCamera()
    {
        Vector3 originalPos = _mainCamera.transform.position;
        float timer = 0;

        while (timer < shackDuration)
        {
            timer += Time.deltaTime;
            float x = Random.Range(-shakeAmount, shakeAmount);
            float y = Random.Range(-shakeAmount, shakeAmount);
            _mainCamera.transform.position = originalPos + new Vector3(x, y, 0);
            yield return null;
        }
        _mainCamera.transform.position = originalPos;
    }
}