/*
* Jacob Buri
* AISpawner.cs
* Assignment 6 - Factory Method
* Spawns the AI based on the type and factory used
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISpawner : MonoBehaviour
{
    public float spawnDistance;
    public Transform allySpawn;
    public Transform enemySpawn;

    public GameObject allyPrefab;
    public GameObject enemyPrefab;

    public GameObject lastSpawned;

    public List<GameObject> currentEnemies;
    public List<GameObject> currentAllies;

    [SerializeField]
    public AICreator aICreator;
    public bool AICreatorIsAlly;
    public string currentType;
    public string AITag;



    // Start is called before the first frame update
    void Start()
    {
        aICreator = new AllyFactory();
        AICreatorIsAlly = true;
        currentType = "Fire";
        currentAllies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //Switch Ally and Enemy Factory
        if (AICreatorIsAlly)
        {
            aICreator = new AllyFactory();
        }
        else
        {
            aICreator = new EnemyFactory();
        }

        
    }

    public GameObject SpawnAI()
    {
        GameObject AIInstance = null;

        //Set the spawn position
        float xRand = Random.Range(-5, 5);
        float zRand = Random.Range(-5, 5);
        Vector3 spawnPos = new Vector3(xRand, 0.1f, zRand);
        Quaternion spawnRotation = enemySpawn.rotation;

        if (AICreatorIsAlly)
        {
            aICreator = new AllyFactory();
            spawnPos.x += allySpawn.position.x;
            spawnPos.z += allySpawn.position.z;
        }
        else
        {
            aICreator = new EnemyFactory();
            spawnPos.x += enemySpawn.position.x;
            spawnPos.z += enemySpawn.position.z;
            spawnRotation = enemySpawn.rotation;
        }

        //Assign prefab to new AI GameObject
        AIInstance = aICreator.CreateAIPrefab(currentType);

        NavMeshHit closestHit;
        if (NavMesh.SamplePosition(spawnPos, out closestHit, 500f, NavMesh.AllAreas))
        {
            AIInstance.transform.position = closestHit.position;

            //Assign prefab to new AI GameObject
            //AIInstance = aICreator.CreateAIPrefab(currentType);

            AIInstance.AddComponent<NavMeshAgent>();
        }

        AIInstance.AddComponent<Projectile>();
        AIInstance.AddComponent<UnitController>();

        //Set Transform;
        AIInstance.transform.position = spawnPos;
        AIInstance.transform.rotation = spawnRotation;

        //return the AI instance
        return AIInstance;

    }

    public void FindCurrentAI()
    {
        //Find starting AI
        GameObject[] playerAI = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] enemyAI = GameObject.FindGameObjectsWithTag("EnemyUnit");

        foreach (GameObject p_Object in playerAI)
        {
            if (!currentAllies.Contains(p_Object))
            {
                currentAllies.Add(p_Object);
            }

        }

        foreach (GameObject e_Object in enemyAI)
        {
            if (!currentEnemies.Contains(e_Object))
            {
                currentEnemies.Add(e_Object);
            }
        }
    }

    //Used in GameManager as Enemy Player
    public void SpawnEnemyAI()
    {
        //Gold handled in GameManager
        AICreatorIsAlly = false;
        AITag = "EnemyUnit";
        GameObject newEnemyAI = SpawnAI();
        lastSpawned = GetAI(newEnemyAI);
    }

    //UI Button Control Methods
    public void SpawnAllyAI()
    {
        if(GameManager.instance.PlayerGold >= 4)
        {
            AICreatorIsAlly = true;
            AITag = "Player";
            GameObject newAllyAI = SpawnAI();
            //newAllyAI.GetComponent<NavMeshAgent>().speed = 40;
            GameManager.instance.PlayerGold -= 5;
        }
        else
        {
            // -- TODO -- Add a pop-up for the player
            Debug.Log("Player is out of Gold");
        }
    }

    public void FireType()
    {
        currentType = "Fire";
    }

    public void WaterType()
    {
        currentType = "Water";
    }

    public void GrassType()
    {
        currentType = "Grass";
    }

    public GameObject GetAI(GameObject thisAI)
    {
        return thisAI;
    }
}
