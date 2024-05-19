using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIManager : MonoBehaviour
{
    [SerializeField] GameObject player;

    public GameObject heartPrefab;  // 心型预制体
    public Transform heartContainer;  // 包含心型的容器
    private List<GameObject> hearts = new List<GameObject>();

    private float currentHealth;
    private float maxHealth;
    void Start()
    {
        // 在游戏开始时获取 TestPlayer 对象
        player = GameObject.FindWithTag("Player");  // 假设 TestPlayer 对象有一个 "Player" 标签
        if (player != null)
        {
            SetHealth(player);
        }
    }
    public void SetHealth(GameObject player)
    {
        this.currentHealth = player.GetComponent<Health>().GetCurrentHealth();
        this.maxHealth = player.GetComponent<Health>().GetMaxHealth(); ;
        Debug.Log("Current Health: " + currentHealth);
        Debug.Log("Max Health: " + maxHealth);
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {


        // 清空现有的心型
        foreach (GameObject heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();

        // 根据当前血量和最大血量重新生成心型

        for (int i = 0; i < maxHealth; i++)
        {

            GameObject heart = Instantiate(heartPrefab, heartContainer);
            if (heart == null)
            {
                Debug.LogError("Failed to instantiate heart prefab.");
                continue;
            }

            hearts.Add(heart);

            Image heartImage = heart.transform.Find("Image").GetComponent<Image>();
            if (heartImage == null)
            {
                Debug.LogError("Heart prefab does not have an Image component.");
                continue;
            }

            heartImage.color = i < currentHealth ? Color.white : Color.grey;

        }

        Debug.Log("Total hearts created: " + hearts.Count);
    }

    // 新增方法：更新心型图标的颜色
    public void UpdateHeartColor()
    {
        if (hearts.Count == 0)
        {
            Debug.LogError("No hearts available to update.");
            return;
        }

        for (int i = 0; i < maxHealth; i++)
        {
            Image heartImage = hearts[i].transform.Find("Image").GetComponent<Image>();
            if (heartImage == null)
            {
                Debug.LogError("Heart prefab does not have an Image component.");
                continue;
            }

            heartImage.color = i < currentHealth ? Color.white : Color.grey;
        }
    }

    void Update()
    {
        if (player != null)
        {
            float newHealth = player.GetComponent<Health>().GetCurrentHealth();
            if (newHealth != currentHealth)
            {
                currentHealth = newHealth;
                UpdateHeartColor();
            }
/*            if (currentHealth <= 0)
            {
                RestoreHealth();
            }*/
        }
    }

/*    public void RestoreHealth()
    {
        currentHealth = maxHealth;
        player.GetComponent<Health>().SetCurrentHealth(currentHealth);
        UpdateHeartColor();
        Debug.Log("Health restored to max.");
    }*/
}
