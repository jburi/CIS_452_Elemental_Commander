/*
* Jacob Buri
* AI.cs
* Assignment 6 - Factory Method
* - Concrete Product for the enemy and ally units
* - Controlls unit's attacks and statisitcs
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AI : MonoBehaviour
{
    //AI Stats
    public string AIType;
    public enum AllegianceType { ALLY, ENEMY }
    public AllegianceType Allegiance;
    public string element;
    public string theEnemy;

    //Projectile
    public int health = 20;
    public float range = 10f;
    //public Projectile projectile;
    public Transform prjSpawn;

    //gameObject components
    public Animator anim;
    public NavMeshAgent navAgent;
    public GameObject body;
    public Transform currentTarget;
    public HealthBar hb;

    //Booleans
    public bool isAttacking = false;

    //Misc
    public float closest;
    public UnitController controller;

    public bool inRange;

    private void Start()
    {
        //aIClient = GetComponent<AIClient>();
        anim = gameObject.GetComponent<Animator>();
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        body = gameObject.GetComponentInChildren<Text>().gameObject;
        controller = gameObject.GetComponent<UnitController>();
        hb = gameObject.GetComponentInChildren<HealthBar>();
        /*
        PlayerMovement movement = gameObject.AddComponent<PlayerMovement>();
        CharacterController controller = gameObject.AddComponent<CharacterController>();
        */

        if (this.Allegiance == AllegianceType.ALLY)
        {
            theEnemy = "EnemyUnit";

        }
        else if (this.Allegiance == AllegianceType.ENEMY)
        {
            theEnemy = "Player";
        }
    }

    private void Awake()
    {
        GameObject found = new List<GameObject>(GameObject.FindGameObjectsWithTag("P_Spawn")).
            Find(g => g.transform.IsChildOf(this.transform));
        prjSpawn = found.transform;
    }

    public void Update()
    {
        if (currentTarget != null)
        {
            navAgent.destination = currentTarget.position;

            if (!controller.shouldMove)
            {
                inRange = true;
            }
        }

        

        hb.SetHealth(health);

        if (health <= 0)
        {
            if(tag == "Player")
            {
                GameManager.instance.PlayerHealth -= 5;
                GameManager.instance.EnemyGold += 2;
                GameManager.instance.aISpawner.currentAllies.Remove(gameObject);
                GameManager.instance.gameObject.GetComponent<UnitManager>().selectedAI.Remove(gameObject.GetComponent<AI>());
            }
            else
            {
                //Remove the enemy from the list and then check for last spawned reference
                GameManager.instance.aISpawner.currentEnemies.Remove(gameObject);
                GameManager.instance.EnemyHealth -= 5;
                GameManager.instance.PlayerGold += 2;

                if (gameObject == GameManager.instance.aISpawner.lastSpawned)
                {
                    GameManager.instance.aISpawner.lastSpawned = null;
                }
            }
            Destroy(gameObject);
        }

    }

    public void Attack()
    {
        FaceTarget(currentTarget);
        //GameObject newProjectile = Instantiate(projectile.prefab, prjSpawn.position, Quaternion.identity);
        //newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * 100);
    }

    IEnumerator ContinueAttacking()
    {
        while (inRange)
        {
            Attack();
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(1);
    }



    public override string ToString()
    {
        return "EnemyType: " + this.AIType + "\n" +
                  "element: " + this.element + "\n" +
                  "damage: " + this.health;
    }

    public void FaceTarget(Transform currentTarget)
    {
        Vector3 direction = (currentTarget.position - body.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(body.transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void FindClosestEnemy(List<GameObject> targets)
    {
        if (targets == null)
        {
            SetAllegianceTargets(targets);
        }

        foreach (GameObject target in targets)
        {
            float temp = Vector3.Distance(target.transform.position, transform.position);

            if (closest == 0 || temp < closest)
            {
                currentTarget = target.transform;
                closest = Vector3.Distance(target.transform.position, transform.position);
            }
        }
    }

    
    public void SetAllegianceTargets(List<GameObject> targets)
    {
        if (this.Allegiance == AllegianceType.ALLY)
        {
            targets = GameManager.instance.aISpawner.currentEnemies;
            theEnemy = "EnemyUnit";

        }
        else if (this.Allegiance == AllegianceType.ENEMY)
        {
            targets = GameManager.instance.aISpawner.currentAllies;
            theEnemy = "Player";
        }
        else
        {
            Debug.Log("No Allegiance");
        }
    }
    
    IEnumerator Damage()
    {
        while (true)
        {
            AI thisEnemy = currentTarget.gameObject.GetComponent<AI>();
            isAttacking = true;

            //Check Elemental Damage
            if (element == ("Fire"))
            {
                if (thisEnemy.element == "Grass")
                {
                    thisEnemy.health -= 4;
                }
                else if (thisEnemy.element == "Water")
                {
                    thisEnemy.health -= 1;
                }
                else if (thisEnemy.element == "Fire")
                {
                    thisEnemy.health -= 2;
                }
            }
            else if (element == ("Water"))
            {
                if (thisEnemy.element == "Fire")
                {
                    thisEnemy.health -= 4;
                }
                else if (thisEnemy.element == "Grass")
                {
                    thisEnemy.health -= 1;
                }
                else if (thisEnemy.element == "Water")
                {
                    thisEnemy.health -= 2;
                }
            }
            else if (element == ("Grass"))
            {
                if (thisEnemy.element == "Water")
                {
                    thisEnemy.health -= 4;
                }
                else if (thisEnemy.element == "Fire")
                {
                    thisEnemy.health -= 1;
                }
                else if (thisEnemy.element == "Grass")
                {
                    thisEnemy.health -= 2;
                }
            }

            yield return new WaitForSeconds(2);
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == theEnemy)
        {
            if (isAttacking == false)
            {
                StartCoroutine("Damage");
            }
        }
        else
        {
            isAttacking = false;
            StopCoroutine("Damage");
        }

        /*
        if (collision.gameObject.tag == theEnemy)
        {
            AI thisEnemy = collision.gameObject.GetComponent<AI>();

            //Check Elemental Damage
            if (element == ("Fire"))
            {
                if (thisEnemy.element == "Grass")
                {
                    thisEnemy.health -= 4;
                }
                else if (thisEnemy.element == "Water")
                {
                    thisEnemy.health -= 1;
                }
            }
            else if (element == ("Water"))
            {
                if (thisEnemy.element == "Fire")
                {
                    thisEnemy.health -= 4;
                }
                else if (thisEnemy.element == "Grass")
                {
                    thisEnemy.health -= 1;
                }
            }
            else if (element == ("Grass"))
            {
                if (thisEnemy.element == "Water")
                {
                    thisEnemy.health -= 4;
                }
                else if (thisEnemy.element == "Fire")
                {
                    thisEnemy.health -= 1;
                }
            }
            else
            {
                thisEnemy.health -= 2;
            }

            thisEnemy.hb.SetHealth(thisEnemy.health);


            if (thisEnemy.health <= 0)
            {
                Destroy(gameObject);
            }
            */
    }


    public void SetSelected(bool isSelected)
    {
        transform.Find("Highlight").gameObject.SetActive(isSelected);
    }

}
