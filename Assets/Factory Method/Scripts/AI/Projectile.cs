using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        prefab = Resources.Load("Projectile") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag ==  "EnemyUnit" )
        {
            collision.gameObject.GetComponent<AI>().health -= 20;
            Destroy(gameObject);
        }
    }
}
