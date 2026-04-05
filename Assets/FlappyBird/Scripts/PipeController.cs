using System;
using FlappyBird.Scripts;
using UnityEngine;

public class PipeController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float hideXPosition = -10f;

    private PipeSpawner _spawner;
    private ScoreZone _scoreZone;

    private void Awake()
    {
        _scoreZone = GetComponentInChildren<ScoreZone>();
    }

    private void OnEnable()
    {
        _scoreZone.ResetScore();
    }

    private void Update()
    {
        if (GameManager.Instance.GameState != GameState.Playing) return;
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        if (transform.position.x <= hideXPosition)
        {
            _spawner.ReturnPipeToPool(this);
        }
    }

    public void Initialize(PipeSpawner spawner)
    {
        _spawner = spawner;
    }
}