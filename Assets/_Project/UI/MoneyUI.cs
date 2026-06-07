using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    public CharacterStats stats;
    public TMP_Text moneyText;

    void Update()
    {
        moneyText.text = stats.money.ToString("0");
    }
}