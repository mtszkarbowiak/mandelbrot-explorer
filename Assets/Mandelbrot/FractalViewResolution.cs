using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mandelbrot
{
    // Tasks:
    // - Binding fractal view (unity.ui target image) with controller
    // - Reacting to fractal view resolution changes
    
    public class FractalViewResolution : MonoBehaviour
    {
        [Header("Persistent")]
        [SerializeField] private FractalController controller;
        [SerializeField] private RawImage targetImage;
        
        [Header("Configuration")] 
        [SerializeField] private float resolutionResetInterval = 0.5f;
        

        private float _timeSinceLastReset;


        private void Start()
        {
            if (controller == null) throw new NullReferenceException("Controller not set.");
            if (targetImage == null) throw new NullReferenceException("Target Unity.UI.RawImage not set.");
            
            ResetControllerResolution(GetDesiredResolution());
        }

        
        private void Update()
        {
            if (GetDesiredResolution() != controller.Resolution && 
                _timeSinceLastReset > resolutionResetInterval)
            {
                ResetControllerResolution(GetDesiredResolution());
            }
            else _timeSinceLastReset += Time.deltaTime;
        }

        private void ResetControllerResolution(Vector2Int resolution)
        {
            _timeSinceLastReset = 0f;
            
            targetImage.texture = controller.ResetResolution(resolution);
        }

        
        private Vector2Int GetDesiredResolution()
        {
            return Vector2Int.RoundToInt(targetImage.rectTransform.rect.size);
        }
    }
}
