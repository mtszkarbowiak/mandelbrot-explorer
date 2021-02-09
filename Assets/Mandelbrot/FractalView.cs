using UnityEngine;
using UnityEngine.UI;

namespace Mandelbrot
{
    public class FractalView : MonoBehaviour
    {
        [SerializeField] private float resolutionAwaiting = 0.5f;
        [SerializeField] private FractalController controller;
        [SerializeField] private RawImage targetImage;

        private Vector2Int _currentResolution;
        private float _timePastLastReset;
        private bool _isTargetImageNull;


        private void Start()
        {
            _isTargetImageNull = targetImage == null;
            ResetControllerResolution(GetDesiredResolution());
        }

        
        private void Update()
        {
            _currentResolution = GetDesiredResolution();

            if (_currentResolution != controller.Resolution && _timePastLastReset > resolutionAwaiting)
            {
                _timePastLastReset = 0f;
    
                ResetControllerResolution(_currentResolution);
            }
            else
            {
                _timePastLastReset += Time.deltaTime;
            }
        }

        private void ResetControllerResolution(Vector2Int resolution)
        {
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
