using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text fpsText;
    [SerializeField] private float updateInterval = 0.5f;

    private float timer;
    private int frameCount;

    private float currentFPS;
    private float averageFPS;
    private float minFPS = float.MaxValue;

    private float totalFPS;
    private int sampleCount;

    private void Update()
    {
        float fps = 1f / Time.unscaledDeltaTime;

        frameCount++;
        timer += Time.unscaledDeltaTime;

        if (fps < minFPS)
            minFPS = fps;

        totalFPS += fps;
        sampleCount++;

        if (timer >= updateInterval)
        {
            currentFPS = frameCount / timer;
            averageFPS = totalFPS / sampleCount;

            fpsText.text =
                $"FPS: {currentFPS:0}\n" +
                $"AVG: {averageFPS:0}\n" +
                $"MIN: {minFPS:0}";

            frameCount = 0;
            timer = 0f;
        }
    }
}
