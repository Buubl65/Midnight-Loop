using UnityEngine;

public class FlashlightFlickerBaseColor : MonoBehaviour
{
    [Header("Компонент")]
    public Light flashlightLight;      
    public Renderer flashlightModel;   
    [Tooltip("Увімкнутий")]
    public Color colorOn = Color.white;
    [Tooltip("Вимкнутий")]
    public Color colorOff = Color.gray;

    [Header("Таймер")]
    public float minTime = 0.05f;
    public float maxTime = 0.3f;

    private float timer;
    private Material flashlightMaterial;

    void Start()
    {
        if (flashlightLight == null) flashlightLight = GetComponent<Light>();
        if (flashlightModel == null) flashlightModel = GetComponent<Renderer>();

        if (flashlightModel != null)
        {
            flashlightMaterial = flashlightModel.material;
            flashlightMaterial.color = colorOn;
        }

        timer = Random.Range(minTime, maxTime);
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            flashlightLight.enabled = !flashlightLight.enabled;
            if (flashlightMaterial != null)
            {
                if (flashlightLight.enabled)
                {
                    flashlightMaterial.color = colorOn;
                }
                else
                {
                    flashlightMaterial.color = colorOff;
                }
            }

            timer = Random.Range(minTime, maxTime);
        }
    }
}