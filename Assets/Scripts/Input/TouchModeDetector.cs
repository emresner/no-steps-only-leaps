using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public static class TouchModeDetector
{
    public static bool IsTouchMode { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        IsTouchMode = false;

        InputSystem.onEvent -= OnInputEvent;
        InputSystem.onEvent += OnInputEvent;
    }

    private static void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
    {
        if (device is Touchscreen)
        {
            IsTouchMode = true;
            return;
        }

        if (device is Keyboard)
        {
            IsTouchMode = false;
        }
    }
}
