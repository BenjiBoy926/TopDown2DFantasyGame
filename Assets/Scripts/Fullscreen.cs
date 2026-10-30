using System;
using UnityEngine;

public class Fullscreen : MonoBehaviour
{
    public static event Action Changed = delegate { };

    private bool _isFullScreen = false;

    private void Update()
    {
        SetIsFullscreen(Screen.fullScreen);
    }

    private void SetIsFullscreen(bool isFullScreen)
    {
        if (isFullScreen == _isFullScreen)
            return;

        _isFullScreen = isFullScreen;
        Changed.Invoke();
    }
}