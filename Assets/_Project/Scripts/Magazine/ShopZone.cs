using UnityEngine;

public class ShopZone : MonoBehaviour
{
    public GameObject shopUI;

    void Start()
    {
        shopUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shopUI.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}