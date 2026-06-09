using UnityEngine;

public class ShopZone : MonoBehaviour
{
    private GameObject shopUI;

    void Start()
    {
        // Спочатку шукаємо активний Canvas
        GameObject canvas = GameObject.Find("Canvas");

        if (canvas != null)
        {
            // transform.Find вміє знаходити навіть ВИМКНЕНІ дочірні об'єкти!
            Transform shopTransform = canvas.transform.Find("ShopUI");

            if (shopTransform != null)
            {
                shopUI = shopTransform.gameObject;
                shopUI.SetActive(false); // Про всяк випадок вимикаємо кодом
            }
            else
            {
                Debug.LogError("Не вдалося знайти 'ShopUI' всередині Canvas! Перевір точність назви.");
            }
        }
        else
        {
            Debug.LogError("Головний об'єкт 'Canvas' не знайдено в сцені!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && shopUI != null)
        {
            shopUI.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}