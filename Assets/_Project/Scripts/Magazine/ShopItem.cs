using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public enum ItemType
    {
        IncreaseMaxHP,
        IncreaseDefense,
        IncreaseDamage
    }

    public string itemName;
    public int price = 10;
    public Transform visual; 

    public void Rotate(float speed)
    {
        if (visual != null)
            visual.Rotate(0, speed * Time.deltaTime, 0);
    }
}