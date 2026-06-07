using UnityEngine;

public class ShopButtons : MonoBehaviour
{
    public ShopSystem shop;
    public GameObject shopUI;

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

    public void CloseShop()
    {
        shopUI.SetActive(false);
    }
}