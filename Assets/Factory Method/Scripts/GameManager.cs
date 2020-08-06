/*
* Jacob Buri
* GameManager.cs
* Assignment 6 - Factory Method
* Handles all of the overarching conditions 
*/
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    //Game Timer
    float timeRemaining = 1000f;

    public AISpawner aISpawner;
    public GameObjectSearcher gos;
    public Timer gameTime;

    public int PlayerHealth;
    public int EnemyHealth;
    public Text playerHealthText;
    public Text enemyHealthText;
    public int PlayerGold = 100;
    public int EnemyGold = 100;
    public Text PlayerGoldText;
    public Text PlayerUnitText;

    public bool GameOver = false;

    public GameObject Menu;
    public GameObject winCanvas;
    public GameObject loseCanvas;
    public HealthBar PlayerHealthBar;
    public HealthBar EnemyHealthBar;

    //Singleton
    public static GameManager instance;

    void Start()
    {
        instance = this;
        gos = gameObject.AddComponent<GameObjectSearcher>();

        //Countdown starting number
        timeRemaining = 1000f;
        PlayerHealthBar.SetMaxHealth(100);
        PlayerHealth = 100;
        PlayerGold = 100;
        EnemyHealthBar.SetMaxHealth(100);
        EnemyHealth = 100;
        EnemyGold = 100;

    }

    public void Awake()
    {
    }

    public void Update()
    {
        //Timer implemented but not used
        gameTime.SetTime((int)timeRemaining);

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timeRemaining = 0;
        }

        //Unit Auto-Attack
        if (aISpawner.lastSpawned != null && aISpawner.currentAllies != null)
        {
            //Default transform for a new enemy unit is to attack the player's base
            aISpawner.lastSpawned.GetComponent<NavMeshAgent>().destination = aISpawner.allySpawn.position;

            //Enemy Units search for an enemy in range to attack
            foreach (GameObject Enemy in aISpawner.currentEnemies)
            {
                AI thisEnemy = Enemy.GetComponent<AI>();

                if (thisEnemy.currentTarget == null)
                {
                    thisEnemy.FindClosestEnemy(aISpawner.currentAllies);
                }

            }

            /*
            //Ally Units will search for enemies in range to attack groups of enemies
            foreach (GameObject Ally in aISpawner.currentAllies)
            {
                AI thisAlly = Ally.GetComponent<AI>();

                if (thisAlly.currentTarget == null && thisAlly.inRange)
                {
                    thisAlly.FindClosestEnemy(aISpawner.currentEnemies);
                }
            }
            */
        }

        //UI Info
        PlayerGoldText.text = PlayerGold.ToString();
        PlayerUnitText.text = aISpawner.currentAllies.Count.ToString();


        //Check if the game is over
        if (PlayerHealth > 0 && EnemyHealth > 0)
        {
            PlayerHealthBar.SetHealth(PlayerHealth);
            playerHealthText.text = PlayerHealth + " / 100";
            EnemyHealthBar.SetHealth(EnemyHealth);
            enemyHealthText.text = EnemyHealth + " / 100";
        }
        else
        {
            GameOver = true;
        }

        if (GameOver)
        {
            Reset();
        }


    }



    public void StartGame()
    {
        //Lose/Win -> Game
        if (winCanvas.activeInHierarchy)
        {
            winCanvas.SetActive(false);
        }
        if (loseCanvas.activeInHierarchy)
        {
            loseCanvas.SetActive(false);
        }

        Menu.SetActive(false);
        StartCoroutine("SpawnEnemyAI");
    }

    public void ReturnToMenu()
    {
        //Lose/Win -> Menu
        if (winCanvas.activeInHierarchy)
        {
            winCanvas.SetActive(false);
        }
        if (loseCanvas.activeInHierarchy)
        {
            loseCanvas.SetActive(false);
        }

        Menu.SetActive(true);
    }


    IEnumerator SpawnEnemyAI()
    {
        if (EnemyGold >= 4) 
        {
            //Starting Spawn time
            int randomSpawnTime = 20;

            while (timeRemaining > 0)
            {
                //Wait the random spawn time
                yield return new WaitForSeconds(randomSpawnTime);

                //Get Random Numbers
                randomSpawnTime = UnityEngine.Random.Range(10, 20);
                int randomeType = UnityEngine.Random.Range(1, 4);

                //Select Random Type
                if (randomeType == 1)
                {
                    aISpawner.FireType();
                }
                else if (randomeType == 2)
                {
                    aISpawner.WaterType();
                }
                else
                {
                    aISpawner.GrassType();
                }

                //Spawn X Enemies
                for (int i = 0; i <= randomeType; i++)
                {
                    aISpawner.SpawnEnemyAI();

                    //Double check transaction for multiple spawns
                    if (EnemyGold > 0)
                        EnemyGold -= 5;
                }
            }
        }

    }

    public void Reset()
    {
        StopAllCoroutines();

        foreach(GameObject destroy in GameObject.FindGameObjectsWithTag("Player"))
        {
            Destroy(destroy);
        }

        foreach (GameObject destroy in GameObject.FindGameObjectsWithTag("EnemyUnit"))
        {
            Destroy(destroy);
        }

        aISpawner.currentAllies.Clear();
        aISpawner.currentEnemies.Clear();

        if (PlayerHealth > 0)
            winCanvas.SetActive(true);
        else if (EnemyHealth > 0)
            loseCanvas.SetActive(true);
        else
            Menu.SetActive(true);


        PlayerHealth = 100;
        EnemyHealth = 100;

        GameOver = false;
    }

}
