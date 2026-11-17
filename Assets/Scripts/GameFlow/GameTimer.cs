using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    public float ElapsedTime { get; private set; }
    public bool IsRunning { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        ResetTimer();
        StopTimer();
    }

    private void Update()
    {
        if (!IsRunning)
            return;

        ElapsedTime += Time.deltaTime;
    }

    public void StartTimer()
    {
        ElapsedTime = 0f;
        IsRunning = true;
    }

    public void StopTimer()
    {
        IsRunning = false;
    }

    public void ResetTimer()
    {
        ElapsedTime = 0f;
    }

    public static string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 100f) % 100f);

        return $"{minutes:00}:{seconds:00}.{milliseconds:00}";
    }
}