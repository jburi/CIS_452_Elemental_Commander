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
    public bool GameOver = false;

    public GameObject Menu;
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
        EnemyHealthBar.SetMaxHealth(100);
    }

    public void Awake()
    {
    }

    public void Update()
    {
        gameTime.SetTime((int) timeRemaining);

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timeRemaining = 0;
        }

        if (aISpawner.lastSpawned != null && aISpawner.currentAllies != null)
        {
            aISpawner.lastSpawned.GetComponent<NavMeshAgent>().destination = aISpawner.allySpawn.position;

            foreach (GameObject targetEnemy in aISpawner.currentEnemies)
            {
                AI thisEnemy = targetEnemy.GetComponent<AI>();

                if (thisEnemy.currentTarget == null)
                {
                    thisEnemy.FindClosestEnemy(aISpawner.currentAllies);
                }

            }

            /*
            foreach (GameObject targetAlly in aISpawner.currentAllies)
            {
                AI thisAlly = targetAlly.GetComponent<AI>();

                if (thisAlly.currentTarget == null)
                {
                    thisAlly.FindClosestEnemy(aISpawner.currentEnemies);
                }
            }
            */
        }

        

        if (PlayerHealth > 0 || EnemyHealth > 0)
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
        Menu.SetActive(false);
        StartCoroutine("SpawnEnemyAI");
    }
    
    IEnumerator SpawnEnemyAI()
    {
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

            //Spawn Enemy
            aISpawner.SpawnEnemyAI();

            
        }

    }

    public void Reset()
    {
        StopAllCoroutines();

        foreach(GameObject destroy in GameObject.FindGameObjectsWithTag("Player"))
        {
            Destroy(destroy);
        }

        foreach (GameObject destroy in GameObject.FindGameObjectsWithTag("Player"))
        {
            Destroy(destroy);
        }

        aISpawner.currentAllies.Clear();
        aISpawner.currentEnemies.Clear();

        Menu.SetActive(true);

        GameOver = false;
    }

}
