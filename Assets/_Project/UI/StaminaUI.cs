using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    public CharacterStats stats;
    public Image staminaFill;

    void Update()
    {
        staminaFill.fillAmount =
            stats.currentStamina / stats.maxStamina;
    }
}