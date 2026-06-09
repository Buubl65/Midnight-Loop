using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public CharacterStats stats;
    public TMP_Text healthText;

    void Update()
    {
        healthText.text = stats.currentHealth.ToString("0") + " <size=70%>/ " + stats.maxHealth.ToString("0") + "</size>";
    }
}