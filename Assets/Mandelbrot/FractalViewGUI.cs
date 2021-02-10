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
       
        private void Start()
        {
            if (controller == null) throw new NullReferenceException("Controller not set.");
        }

        private void Update()
        {
            {
                var fps = 1.0f / Time.unscaledDeltaTime;
                fpsCounterText.text = $"<b>{fps:0.0}</b> FPS";
            }

            {
                controller.funcIterations = (byte) Mathf.RoundToInt(iterationScrollbar.value * 255f);
                iterationCounterText.text = controller.funcIterations.ToString();
            }

            {
                zoomLabelText.text = $"<size=10>x10^</size><b>{Mathf.Log10(controller.viewScalar):0.00}</b>";
                positionLabelText.text = $"(x={controller.viewOrigin.x:0.00000} y={controller.viewOrigin.y:0.00000})";
            }
        }
    }
}
