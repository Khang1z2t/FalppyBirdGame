using System;
using UnityEngine;

namespace FlappyBird.Scripts
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        
        [Header("Audio Clips")]
        [SerializeField] private AudioClip fly;
        [SerializeField] private AudioClip score;
        [SerializeField] private AudioClip hit;
        [SerializeField] private AudioClip die;
        
        private AudioSource _audioSource;

        private void Awake()
        {
            Instance = this;
            _audioSource = GetComponent<AudioSource>();
        }
        
        public void PlayFly()
        {
            _audioSource.PlayOneShot(fly);
        }

        public void PlayScore()
        {
            _audioSource.PlayOneShot(score);
        }

        public void PlayHit()
        {
            _audioSource.PlayOneShot(hit);
        }

        public void PlayDie()
        {
            _audioSource.PlayOneShot(die);
        }
    }
}