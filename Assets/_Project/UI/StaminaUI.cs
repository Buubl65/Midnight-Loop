using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    public CharacterStats stats;
    public TMP_Text staminaText;

    void Update()
    {
        staminaText.text =
            stats.currentStamina.ToString("0") + " / " + stats.maxStamina.ToString("0");
    }
}