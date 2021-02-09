using UnityEngine;
using UnityEngine.UI;

namespace Mandelbrot
{
    // Tasks:
    // - Binding fractal view (unity.ui target image) with controller
    // - Reacting to fractal view resolution changes
    
    public class FractalView : MonoBehaviour
    {
        [Header("Persistent")]
        [SerializeField] private FractalController controller;
        [SerializeField] private RawImage targetImage;
        
        [Header("Realtime")] 
        [SerializeField] private float resolutionResetInterval = 0.5f;

        
        private Vector2Int _currentResolution;
        private float _timeSinceLastReset;
        private bool _isTargetImageNull;


        private void Start()
        {
            _isTargetImageNull = targetImage == null;
            
            ResetControllerResolution(GetDesiredResolution());
        }

        
        private void Update()
        {
            _currentResolution = GetDesiredResolution();

            if (_currentResolution != controller.Resolution && _timeSinceLastReset > resolutionResetInterval)
                ResetControllerResolution(_currentResolution);
            else 
                _timeSinceLastReset += Time.deltaTime;
        }

        private void ResetControllerResolution(Vector2Int resolution)
        {
            _timeSinceLastReset = 0f;
            
            targetImage.texture = controller.ResetResolution(resolution);
        }

        
        private Vector2Int GetDesiredResolution()
        {
            if (_isTargetImageNull)
            {
                return new Vector2Int(
                    Screen.width,
                    Screen.height);
            }
            
            return Vector2Int.RoundToInt(targetImage.rectTransform.rect.size);
        }
    }
}
