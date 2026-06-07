using UnityEngine;
using TMPro;

public class DefenseUI : MonoBehaviour
{
    public CharacterStats stats;
    public TMP_Text defenseText;

    void Update()
    {
        defenseText.text = stats.defense.ToString("0");
    }
}