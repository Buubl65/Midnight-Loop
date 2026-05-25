using System.Collections;
using UnityEngine;

// Эта строчка гарантирует, что таймер не будет работать без контроллера
[RequireComponent(typeof(TrafficLightController))]
public class TrafficLightTimer : MonoBehaviour
{
    [Header("Тайминги (в секундах)")]
    public float greenDuration = 8f;
    public float yellowDuration = 2f;
    public float redDuration = 8f;

    [Header("Смещение (для перекрестков)")]
    [Tooltip("Задержка перед стартом цикла.")]
    public float startDelay = 0f;

    private TrafficLightController controller;

    private void Start()
    {
        controller = GetComponent<TrafficLightController>();

        StartCoroutine(TrafficLightRoutine());
    }

    private IEnumerator TrafficLightRoutine()
    {
        if (startDelay > 0)
        {
            yield return new WaitForSeconds(startDelay);
        }

        while (true)
        {
            controller.SetGreen();
            yield return new WaitForSeconds(greenDuration);

            controller.SetYellow();
            yield return new WaitForSeconds(yellowDuration);

            controller.SetRed();
            yield return new WaitForSeconds(redDuration);
        }
    }
}