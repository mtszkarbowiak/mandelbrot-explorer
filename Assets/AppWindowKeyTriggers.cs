using UnityEngine;

public class AppWindowKeyTriggers : MonoBehaviour
{
    public KeyCode fullScreenTrigger = KeyCode.F11;
    public KeyCode quitAppTrigger = KeyCode.Escape;
    private bool _isFullscreen;

    private void Awake()
    {
        _isFullscreen = true;
        Toggle();
    }

    private void Update()
    {
        if (Input.GetKeyDown(fullScreenTrigger))
            Toggle();
            
        if (Input.GetKeyDown(quitAppTrigger))
            Application.Quit();
    }

    private void Toggle()
    {
        _isFullscreen = !_isFullscreen;
            
        if (_isFullscreen)
        {
            Screen.SetResolution(
                Display.main.systemWidth,
                Display.main.systemHeight,
                true, 60);
        }
        else
        {
            Screen.SetResolution(
                1000,
                600,
                false, 60);
        }
    }
}