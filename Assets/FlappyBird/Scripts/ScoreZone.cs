using UnityEngine;

namespace FlappyBird.Scripts
{
    public class ScoreZone : MonoBehaviour
    {
        private bool _hasScored = false;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (_hasScored) return;

            if (collider.CompareTag("Player"))
            {
                _hasScored = true;
                GameManager.Instance.AddScore();
                
            }
        }

        public void ResetScore()
        {
            _hasScored = false;
        }
    }
}