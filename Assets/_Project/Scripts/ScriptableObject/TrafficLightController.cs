using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    [Header("Индексы материалов (Mesh Renderer -> Materials)")]
    public int greenIndex = 1;  // Зеленый
    public int yellowIndex = 2; // Желтый
    public int redIndex = 3;    // Красный

    [Header("Цвета свечения")]
    [ColorUsage(false, true)] public Color greenColor = Color.green;
    [ColorUsage(false, true)] public Color yellowColor = Color.yellow;
    [ColorUsage(false, true)] public Color redColor = Color.red;

    private Renderer rend;
    private MaterialPropertyBlock propBlock;
    private string emissionProperty = "_EmissionColor";

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();
        TurnOffAll();
    }

    public void TurnOffAll()
    {
        SetEmission(greenIndex, Color.black);
        SetEmission(yellowIndex, Color.black);
        SetEmission(redIndex, Color.black);
    }

    public void SetGreen()
    {
        TurnOffAll();
        SetEmission(greenIndex, greenColor);
    }

    public void SetYellow()
    {
        TurnOffAll();
        SetEmission(yellowIndex, yellowColor);
    }

    public void SetRed()
    {
        TurnOffAll();
        SetEmission(redIndex, redColor);
    }

    private void SetEmission(int index, Color color)
    {
        rend.GetPropertyBlock(propBlock, index);
        propBlock.SetColor(emissionProperty, color);
        rend.SetPropertyBlock(propBlock, index);
    }
}