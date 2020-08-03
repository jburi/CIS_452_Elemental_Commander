using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : AICreator
{
    public Material fireMaterial;
    public Material waterMaterial;
    public Material grassMaterial;

    public GameObject enemyPrefab;
    GameManager gm;

    //Does not use Monobehavior - Custom Start
    private void Start()
    {
        enemyPrefab = Resources.Load("Enemy") as GameObject;
        fireMaterial = Resources.Load("EthanRed") as Material;
        waterMaterial = Resources.Load("EthanBlue") as Material;
        grassMaterial = Resources.Load("EthanGreen") as Material;

        gm = GameManager.instance;
    }

    public override GameObject CreateAIPrefab(string type)
    {
        if (enemyPrefab == null)
        {
            Start();
        }

        GameObject newEnemy = GameObject.Instantiate(enemyPrefab);

        SkinnedMeshRenderer body = newEnemy.GetComponentInChildren<SkinnedMeshRenderer>();


        if (type.Equals("Fire"))
        {
            body.material = fireMaterial;
            newEnemy.AddComponent<EnemyFire>();
        }
        else if (type.Equals("Water"))
        {
            body.material = waterMaterial;
            newEnemy.AddComponent<EnemyWater>();
        }
        else if (type.Equals("Grass"))
        {
            body.material = grassMaterial;
            newEnemy.AddComponent<EnemyGrass>();
        }

        if (gm.aISpawner.currentEnemies == null)
        {
            gm.aISpawner.currentEnemies = new List<GameObject>();
            gm.aISpawner.currentEnemies.Add(newEnemy);
        }
        else
        {
            gm.aISpawner.currentEnemies.Add(newEnemy);
        }

        return newEnemy;

    }

}
