using UnityEngine;

namespace Mandelbrot
{
    // Tasks:
    // - Handling user input (mouse)
    
    public class FractalViewInput : MonoBehaviour
    {
        [Header("Persistent")]
        [SerializeField] private FractalController controller;
        
        [Header("Realtime")] 
        [Range(0.0f,10.0f)]public float wheelSensivity = 1.0f;
        [Range(-12.0f,3.0f)]public float wheelValue = 0.0f;
        public int mouseButtonIndex = 1;
        
        private Vector2 initOrigin;
        private Vector2 initMousePosition;
        
        private void Start()
        {
            if (controller == null) throw new System.NullReferenceException("Controller not set.");
        }
        
        private void Update()
        {
            wheelValue += -Input.mouseScrollDelta.y * wheelSensivity * Time.deltaTime;
            controller.viewScalar = Mathf.Exp(wheelValue);

            
            if (Input.GetMouseButtonDown(mouseButtonIndex))
            {
                initOrigin = controller.viewOrigin;
                initMousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(mouseButtonIndex))
            {
                var deltaMousePosition = (Vector2)Input.mousePosition - initMousePosition;
                controller.viewOrigin = initOrigin - controller.viewScalar * deltaMousePosition 
                    / Mathf.Min(controller.Resolution.x,controller.Resolution.y);
            }
        }
    }
}