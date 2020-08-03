/*
* Jacob Buri
* .cs
* Assignment 6 - Factory Method
* Not Implemented - Handled in GameManager
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public static int PlayerHealth;
    public static int EnemyHealth;
    public static bool GameOver = false;

    public Text playerHealthText;
    public Text enemyHealthText;
    public Slider playerSlider;
    public Slider enemySlider;
    public Image fill;

    public void SetMaxHealth(int health)
    {
        playerSlider.maxValue = health;
        enemySlider.value = health;
    }

    public void SetHealth(int health)
    {
        playerSlider.value = health;
        enemySlider.value = health;
    }

    public static void CheckWinLossConditions()
    {
        Debug.Log("Checking win/loss conditions.");
        Debug.Log("Your health is now " + Health.PlayerHealth + ".");
        Debug.Log("Enemy health is now " + Health.EnemyHealth + ".");

        if (PlayerHealth <= 0)
        {
            Debug.Log("You Lose!");
            GameOver = true;
        }
        else if (EnemyHealth <= 0)
        {
            Debug.Log("You Win!");
            GameOver = true;
        }

        //Could add a method call here
        //to handle allowing game restart here if GameOver == true

    }
}
