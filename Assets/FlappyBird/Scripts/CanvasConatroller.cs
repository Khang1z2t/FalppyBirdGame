using System;
using UnityEngine;
using UnityEngine.UI;

namespace FlappyBird.Scripts
{
    public class CanvasConatroller : MonoBehaviour
    {
        private void Awake()
        {
            Canvas canvas = GetComponent<Canvas>();
            Camera uiCamera = canvas.worldCamera;

            bool isTable = uiCamera.aspect > (9f / 16f);
            CanvasScaler scaler = GetComponent<CanvasScaler>();
            scaler.matchWidthOrHeight = isTable ? 1 : 0;
        }
    }
}