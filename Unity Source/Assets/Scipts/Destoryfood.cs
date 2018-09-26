using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destoryfood : MonoBehaviour {

    public GameObject[] enemyList;

    private void Start()
    {
        enemyList = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Handle collision with shark and food.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Food")
        {
            GameObject tmp = other.gameObject;

            // Let every enemy shark know that the food has been eaten, so normal behavior can resume.
            foreach (GameObject go in enemyList)
            {     
                if (go.name == "Shark")
                    go.GetComponent<SharkController>().AteFood(tmp);
            }

            Destroy(other.gameObject);
        }
    }
}
