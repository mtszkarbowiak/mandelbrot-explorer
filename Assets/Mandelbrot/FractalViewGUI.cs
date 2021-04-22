using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mandelbrot
{
    public class FractalViewGUI : MonoBehaviour
    {
        [Header("Persistent")]
        [SerializeField] private FractalController controller;

        [SerializeField] private Text fpsCounterText;
        [SerializeField] private Text iterationCounterText;
        [SerializeField] private Scrollbar iterationScrollbar;
        [SerializeField] private Text zoomLabelText;
        [SerializeField] private Text positionLabelText;

        [SerializeField] private ColorPalette[] _colorPalettes;
        [SerializeField] private int _currentColorPalette;

        [Header("Config")]
        [SerializeField] private AnimationCurve scrollbarITerationNumber;

        private void Start()
        {
            if (controller == null) throw new NullReferenceException("Controller not set.");
            
            SwitchColorPalette();
        }

        private void Update()
        {
            {
                var fps = 1.0f / Time.unscaledDeltaTime;
                fpsCounterText.text = $"<b>{fps:0.0}</b> FPS";
            }

            {
                var iterationsCont = scrollbarITerationNumber.Evaluate(iterationScrollbar.value);
                var iterations = Mathf.CeilToInt(iterationsCont);
                
                controller.funcIterations = iterations;
                iterationCounterText.text = controller.funcIterations.ToString();
            }

            {
                zoomLabelText.text = $"<size=10>x10^</size><b>{Mathf.Log10(controller.viewScalar):0.00}</b>";
                positionLabelText.text = $"(x={controller.viewOrigin.x:0.00000} y={controller.viewOrigin.y:0.00000})";
            }
        }

        public void SwitchColorPalette()
        {
            _currentColorPalette++;
            
            if (_currentColorPalette >= _colorPalettes.Length || 
                _currentColorPalette < 0)
                _currentColorPalette = 0;

            controller.colorRealPositive = _colorPalettes[_currentColorPalette].colorRealPositive;
            controller.colorRealNegative = _colorPalettes[_currentColorPalette].colorRealNegative;
            controller.colorImaginaryPositive = _colorPalettes[_currentColorPalette].colorImaginaryPositive;
            controller.colorImaginaryNegative = _colorPalettes[_currentColorPalette].colorImaginaryNegative;
        }


        [Serializable]
        public class ColorPalette
        {
            public Vector4 colorRealPositive, colorImaginaryPositive;
            public Vector4 colorRealNegative, colorImaginaryNegative;
        }
    }
}
