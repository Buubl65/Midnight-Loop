using UnityEngine;

public class ShopButtons : MonoBehaviour
{
    public ShopSystem shop;

    public void OnLeft()
    {
        shop.Previous();
    }

    public void OnRight()
    {
        shop.Next();
    }

    public void OnBuy()
    {
        shop.Buy();
    }
}