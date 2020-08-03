/*
* Jacob Buri
* AllyFactory.cs
* Assignment 6 - Factory Method
* Factory to create allies of a certain type
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyFactory : AICreator
{
    public Material fireMaterial;
    public Material waterMaterial;
    public Material grassMaterial;

    public GameObject allyPrefab;
    GameManager gm;

    //Does not use Monobehavior - Custom Start
    void Start()
    {
        allyPrefab = Resources.Load("Ally") as GameObject;
        fireMaterial = Resources.Load("EthanRed") as Material;
        waterMaterial = Resources.Load("EthanBlue") as Material;
        grassMaterial = Resources.Load("EthanGreen") as Material;

        gm = GameManager.instance;
    }

    public override GameObject CreateAIPrefab(string type)
    {
        if (allyPrefab == null)
        {
            Start();
        }

        GameObject newAlly = GameObject.Instantiate(allyPrefab);

        SkinnedMeshRenderer body = newAlly.GetComponentInChildren<SkinnedMeshRenderer>();

        if (type.Equals("Fire"))
        {
            body.material = fireMaterial;
            newAlly.AddComponent<AllyFire>();
        }
        else if (type.Equals("Water"))
        {
            body.material = waterMaterial;
            newAlly.AddComponent<AllyWater>();
        }
        else if (type.Equals("Grass"))
        {
            body.material = grassMaterial;
            newAlly.AddComponent<AllyGrass>();
        }

        gm.aISpawner.currentAllies.Add(newAlly);

        return newAlly;

    }

}
