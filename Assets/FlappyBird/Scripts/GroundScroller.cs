using System;
using UnityEngine;

namespace FlappyBird.Scripts
{
    public class GroundScroller : MonoBehaviour
    {
        [SerializeField] private float scrollSpeed = 2f;

        private Material _material;
        private Vector2 _offset;

        private void Awake()
        {
            _material = GetComponent<SpriteRenderer>().material;
        }

        private void Update()
        {
            if (GameManager.Instance.GameState == GameState.GameOver) return;
            
            _offset.x += scrollSpeed * Time.deltaTime;
            _material.mainTextureOffset = _offset;
        }
    }
}