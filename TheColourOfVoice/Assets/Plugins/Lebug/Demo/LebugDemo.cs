using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LebugDemo : MonoBehaviour {

    int RifleAmmo;
    int RocketAmmo;

    float PlayerHealth;
    float PlayerArmor;
    float EnemyHealth;
    float EnemyArmor;

    string LevelName = "Fortress of Mystery";
    int EpisodeNo = 2;
    int MissionNo = 9;
    int SongIndex = 4;

    // Update is called once per frame
    void Update()
    {
        UpdateValues();
        PrintViaDebugLog();
        PrintViaLebugLog();
    }

    void UpdateValues()
    {
        RifleAmmo = Random.Range(0, 50);
        RocketAmmo = Random.Range(0, 50);

        PlayerHealth = Random.Range(0f, 100f);
        PlayerArmor = Random.Range(0f, 100f);
        EnemyHealth = Random.Range(0f, 100f);
        EnemyArmor = Random.Range(0f, 100f);
    }


    void PrintViaDebugLog()
    {

        Debug.Log("Rifle Ammo : " + RifleAmmo);
        Debug.Log("Rocket Ammo : " + RocketAmmo);
        Debug.Log("LevelName : " + LevelName);
        Debug.Log("EpisodeNo : " + EpisodeNo);
        Debug.Log("MissionNo : " + MissionNo);
        Debug.Log("Player Health : " + PlayerHealth);
        Debug.Log("Player Armor : " + PlayerArmor);
        Debug.Log("Enemy Health : " + EnemyHealth);
        Debug.Log("Enemy Armor : " + EnemyArmor);
        Debug.Log("SongIndex : " + SongIndex);

    }

    void PrintViaLebugLog()
    {

        Lebug.Log("Rifle Ammo", RifleAmmo, "Ammunition");
        Lebug.Log("Rocket Ammo", RocketAmmo, "Ammunition");

        Lebug.Log("LevelName", LevelName, "LevelInfo", false); // Start the category collapsed
        Lebug.Log("EpisodeNo", EpisodeNo, "LevelInfo", false); // Start the category collapsed
        Lebug.Log("MissionNo", MissionNo, "LevelInfo", false); // Start the category collapsed

        Lebug.Log("Health", PlayerHealth, "PlayerStats");
        Lebug.Log("Armor", PlayerArmor, "PlayerStats");

        Lebug.Log("Health", EnemyHealth, "EnemyStats"); // Can use same keys in different categories
        Lebug.Log("Armor", EnemyArmor, "EnemyStats"); // Can use same keys in different categories

        Lebug.Log("SongIndex", SongIndex); // Default category

        if (RocketAmmo == 0)
            Lebug.Del("Rocket Ammo"); // Delete a value.

        if (EnemyHealth == 0)
        {
            Lebug.DelCategory("EnemyStats"); // Delete a category
        }

    }
}