using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    public int targetFPS = 60; // Set your desired FPS here
    void Awake()
    {
        QualitySettings.vSyncCount = 0; // VSync must be disabled for targetFrameRate to work
        Application.targetFrameRate = targetFPS;
    }
}
