using UnityEngine;

namespace Mandelbrot
{
    public class FractalController : MonoBehaviour
    {
        // Unity Inspector fields
        [Header("Persistent")]
        [SerializeField] private ComputeShader juliaShader;

        [Header("Configuration")] 
        [SerializeField] private bool forceStart;
        [SerializeField] private Vector2Int resolution = new Vector2Int(1024,1024);
        [SerializeField] private FilterMode defaultFilterMode = FilterMode.Point;

        [Header("Realtime")] 
        public Vector2 viewOrigin = Vector2.zero;
        [Range(3.0f, 0.001f)] public float viewScalar = 1.0f;
        public Vector2 funcBase = Vector2.zero;
        public byte funcIterations = 30;

        // Changing fields
        private RenderTexture _activeRenderTexture;

        // Cached / Readonly fields
        private int _mainKernel, _textureId, _screenSizeXId,_screenSizeYId;
        private int _viewScalarId, _viewShiftXId, _viewShiftYId;
        private int _funcBaseRealId, _funcBaseImaginaryId, _funcIterationsId;

        // Accessors
        public Vector2Int Resolution => resolution;

        
        
        public void Awake()
        {
            CacheShaderHashes();
            
            if(forceStart) 
                ResetResolution(resolution);
        }

        
        private void CacheShaderHashes()
        {
            _mainKernel = juliaShader.FindKernel("julia_pixel");
            
            _textureId = Shader.PropertyToID("canvas");
            _screenSizeXId = Shader.PropertyToID("canvas_size_x");
            _screenSizeYId = Shader.PropertyToID("canvas_size_y");
            
            _viewScalarId = Shader.PropertyToID("view_scalar");
            _viewShiftXId = Shader.PropertyToID("view_shift_x");
            _viewShiftYId = Shader.PropertyToID("view_shift_y");
            
            _funcBaseRealId = Shader.PropertyToID("func_base_real");
            _funcBaseImaginaryId = Shader.PropertyToID("func_base_imaginary");
            _funcIterationsId = Shader.PropertyToID("func_iterations");
            
#if false
            _colorRealId = Shader.PropertyToID("color_real");
            _colorImaginaryId = Shader.PropertyToID("color_imaginary");
            _colorBaseId = Shader.PropertyToID("color_base");
            _colorScalarId = Shader.PropertyToID("color_scalar");
#endif
        }

        
        private void Update()
        {
            // Update uniform values
            juliaShader.SetFloat(_viewShiftXId, viewOrigin.x);
            juliaShader.SetFloat(_viewShiftYId, viewOrigin.y);
            juliaShader.SetFloat(_viewScalarId, viewScalar);
            
            juliaShader.SetFloat(_funcBaseRealId, funcBase.x);
            juliaShader.SetFloat(_funcBaseImaginaryId, funcBase.y);
            juliaShader.SetInt(_funcIterationsId, funcIterations);
            
            // Update fractal image (by dispatching the shader)
            juliaShader.Dispatch(
                kernelIndex: _mainKernel,
                threadGroupsX: resolution.x/8,
                threadGroupsY: resolution.y/8,
                threadGroupsZ: 1);
        }

        
        public RenderTexture ResetResolution(Vector2Int newResolution)
        {
            resolution = newResolution;
            
            // Recreate target texture and assign it
            _activeRenderTexture = new RenderTexture(resolution.x,resolution.y,32)
            {
                enableRandomWrite = true,
                useMipMap = false,
                filterMode = defaultFilterMode,
            };
            _activeRenderTexture.Create();
            
            
            juliaShader.SetTexture(_mainKernel,_textureId,_activeRenderTexture);
            juliaShader.SetInt(_screenSizeXId,resolution.x);
            juliaShader.SetInt(_screenSizeYId,resolution.y);

            return _activeRenderTexture;
        }
    }
}
