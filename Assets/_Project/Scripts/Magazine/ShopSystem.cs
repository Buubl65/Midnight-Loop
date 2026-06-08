using UnityEngine;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    public CharacterStats playerStats;

    [Header("Items")]
    public Transform itemsContainer;
    public ShopItem[] items;

    [Header("UI")]
    public TMP_Text nameText;
    public TMP_Text priceText;

    [Header("Settings")]
    public float spacing = 2.5f;
    public float moveSpeed = 8f;
    public float rotateSpeed = 30f;

    private int currentIndex = 0;
    private Vector3 targetPos;
    private ShopItem currentSpawnedItem;

    void Start()
    {
        Debug.Log("ShopSystem START");
        UpdateTarget();
        UpdateUI();
        SpawnCurrentItem();
    }

    void Update()
    {
        if (currentSpawnedItem != null)
        {
            currentSpawnedItem.Rotate(rotateSpeed);
        }
    }

    public void Next()
    {
        currentIndex++;
        if (currentIndex >= items.Length)
            currentIndex = items.Length - 1;

        UpdateTarget();
        UpdateUI();
        SpawnCurrentItem();
    }

    public void Previous()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = 0;

        UpdateTarget();
        UpdateUI();
        SpawnCurrentItem();
    }

    void UpdateTarget()
    {
        targetPos = new Vector3(-currentIndex * spacing, 0, 0);
    }

    void UpdateUI()
    {
        nameText.text = items[currentIndex].itemName;
        priceText.text = items[currentIndex].price.ToString();
    }
    void SpawnCurrentItem()
    {
        Debug.Log("Spawn called");

        foreach (Transform child in itemsContainer)
        {
            Destroy(child.gameObject);
        }

        GameObject spawnedObj = Instantiate(items[currentIndex].gameObject, itemsContainer);

        spawnedObj.transform.localPosition = Vector3.zero;

        spawnedObj.transform.localRotation = Quaternion.identity;

        currentSpawnedItem = spawnedObj.GetComponent<ShopItem>();
    }

    public void Buy()
    {
        ShopItem item = items[currentIndex];

        if (playerStats.SpendMoney(item.price))
        {
            Debug.Log("Bought: " + item.itemName);
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }
}