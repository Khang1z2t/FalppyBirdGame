using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pipePrefab;
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private float heightOffset = .8f;
    [SerializeField] private int poolSize = 10;

    private float _timer;
    private Camera _mainCamera;
    private Queue<PipeController> _pipePool = new Queue<PipeController>();
    private List<PipeController> _activePipes = new List<PipeController>();

    private void Awake()
    {
        _mainCamera = Camera.main;
        SetSpawnPositon();
        CreatePool();
    }

    private void Update()
    {
        if (GameManager.Instance.GameState != GameState.Playing) return;
        _timer += Time.deltaTime;

        while (_timer >= spawnRate)
        {
            SpawnPipe();
            _timer -= spawnRate;
        }
    }

    private void SetSpawnPositon()
    {
        var screenRight = _mainCamera.orthographicSize * _mainCamera.aspect;
        var spawnX = screenRight + 0.5f;
        Vector3 pos = transform.position;
        pos.x = spawnX;
        transform.position = pos;
    }

    private void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject pipeObj = Instantiate(pipePrefab, transform);
            pipeObj.SetActive(false);
            
            PipeController pipe = pipeObj.GetComponent<PipeController>();
            pipe.Initialize(this);
            _pipePool.Enqueue(pipe);
            _activePipes.Add(pipe);
        }
    }
    
    private void SpawnPipe()
    {
        if (_pipePool.Count == 0)
        {
            Debug.Log("There's no pipe in list");
            return;
        }
        
        PipeController pipe = _pipePool.Dequeue();
        float randomY = Random.Range(-heightOffset, heightOffset);
        pipe.transform.localPosition = transform.position + new Vector3(0, randomY, 0);
        
        pipe.gameObject.SetActive(true);
        
    }
    
    public void ReturnPipeToPool(PipeController pipe)
    {
        pipe.gameObject.SetActive(false);
        _pipePool.Enqueue(pipe);
    }

    public void ResetSpawner()
    {
        _timer = 0f;
        _pipePool.Clear();

        foreach (PipeController pipe in _activePipes)
        {
            pipe.gameObject.SetActive(false);
            _pipePool.Enqueue(pipe);
        }
    }
}