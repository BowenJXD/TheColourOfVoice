using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIManager : MonoBehaviour
{
    [SerializeField] GameObject player;

    public GameObject heartPrefab;  
    public Transform heartContainer;  
    private List<GameObject> hearts = new List<GameObject>();

    private float currentHealth;
    private float maxHealth;
    void Start()
    {
        player = GameObject.FindWithTag("Player"); 
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



        foreach (GameObject heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();



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

            heartImage.color = i < currentHealth ? Color.white : Color.clear;

        }

        Debug.Log("Total hearts created: " + hearts.Count);
    }


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

            heartImage.color = i < currentHealth ? Color.white : Color.clear;
        }
    }

    void Update()
    {
        if (player != null)
        {
            float newHealth = player.GetComponent<Health>().GetCurrentHealth();
            if (newHealth != currentHealth)
            {
                if (newHealth < currentHealth)  
                {
                    StartCoroutine(FlashHeart((int)newHealth));
                }
                currentHealth = newHealth;
                UpdateHeartColor();
            }

        }
    }
    private IEnumerator FlashHeart(int index)
    {
        Image heartImage = hearts[index].transform.Find("Image").GetComponent<Image>();
        if (heartImage == null)
        {
            yield break;
        }

        Color originalColor = heartImage.color;
        Color flashColor = Color.black;

        for (int i = 0; i < 5; i++)  
        {
            heartImage.color = flashColor;
            yield return new WaitForSeconds(0.1f);
            heartImage.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }

        heartImage.color = Color.clear; 
    }
    /*    public void RestoreHealth()
        {
            currentHealth = maxHealth;
            player.GetComponent<Health>().SetCurrentHealth(currentHealth);
            UpdateHeartColor();
            Debug.Log("Health restored to max.");
        }*/
}
