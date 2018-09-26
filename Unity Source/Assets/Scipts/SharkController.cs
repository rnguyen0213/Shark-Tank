using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SharkController : MonoBehaviour
{
    public GameManager GM;
    public Transform[] points;
    public Transform origin;
    public Renderer rend;
    public NavMeshAgent agent;
    public GameObject player;

    public int destPoint = 0;

    public bool detection = false;
    public float speed = 6.0f;
    public float angularSpeed = 240.0f;
    public float acceleration = 8.0f;

    public float stunTime = 0;
    public bool stunnable = true;
    public bool isStun = false;
    public int stunCount = 0;

    public bool isPlayerHidden = false;

    public List<GameObject> food;
    public bool chasingFood = false;
    public GameObject chasingFoodGO;



    void Start()
    {
        food = new List<GameObject>();
        GM = GameObject.FindGameObjectWithTag("UI").GetComponent<GameManager>();
        origin = transform;
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        GotoNextPoint();
    }

    void Update()
    {

        // Normal AI Behavior.
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !detection && !chasingFood)
            GotoNextPoint();

        // If player is detected then chase the player.
        if (detection && !isPlayerHidden && !isStun && !chasingFood)
            FollowPlayer();

        // Stun this enemy object if it is stunnable and the total current stunned enemy is less then 2.
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                if (hit.transform.gameObject.name.CompareTo("Enemy Touch") == 0 && stunnable == true && GM.numStunEnemy < 2)
                    if (hit.transform.parent.gameObject == this.gameObject)
                        StunEnemy();
        }

        // Resume when stun ends.
        if (isStun && Time.time - stunTime > 3)
        {
            isStun = false;
            agent.isStopped = false;
            rend.material.color = Color.white;
            GM.numStunEnemy--;
        }

        // If food exist, chase it within 20 units.
        if (food.Count > 0)
            ClosestFood();

        // Stun cooldown.
        if (Time.time - stunTime > 6)
            stunnable = true;
    }

    // Find the closest food and chase it if it is within 20 units.
    private void ClosestFood()
    {
        GameObject closest = food[0];
        float dist = Vector3.Distance(transform.position, food[0].transform.position);
        for (int i = 1; i < food.Count; i++)
        {
            float tmp = Vector3.Distance(transform.position, food[i].transform.position);
            if (tmp < dist)
            {
                dist = tmp;
                closest = food[i];
            }
        }

        if (!isStun && !detection && dist < 20)
        {
            agent.speed = 10;
            agent.angularSpeed = 300;
            agent.acceleration = 80;
            chasingFood = true;
            chasingFoodGO = closest;
            agent.destination = closest.transform.position;
        }
    }

    // Stun the enemy for 3 seconds.
    public void StunEnemy()
    {
        rend.material.color = Color.red;
        agent.isStopped = true;
        stunTime = Time.time;
        stunnable = false;
        isStun = true;
        ResetEnemy();
        GM.numStunEnemy++;
    }

    // Calculate the next destination point for pathing.
    private void GotoNextPoint()
    {
        if (points.Length == 0)
            return;

        if (points.Length == 1)
            agent.autoBraking = true;

        agent.destination = points[destPoint].position;
        destPoint = (destPoint + 1) % points.Length;
    }

    // Chase the player.
    private void FollowPlayer()
    {
        agent.destination = player.transform.position;
    }

    // Find out if the player is finding or not.
    public void HiddenPlayer(bool hidden)
    {
        isPlayerHidden = hidden;
    }

    // Return to normal movement speed
    private void ResetEnemy()
    {
        agent.speed = 6;
        agent.angularSpeed = 240;
        agent.acceleration = 8;
        detection = false;
    }

    // Populate the food list, called by player script.
    public void AddFood(GameObject newfood)
    {
        food.Add(newfood);
        ClosestFood();
    }

    // Called by Destroyfood script. Once food is eaten, either find next closest food or resume normal behavior.
    public void AteFood(GameObject atefood)
    {
        food.Remove(atefood);
        if (chasingFoodGO != null && chasingFoodGO.Equals(atefood) == true)
        {
            chasingFood = false;
            if (food.Count == 0)
            {           
                agent.destination = points[destPoint].position;
            }
            else
                ClosestFood();
        }
        ResetEnemy();
    }

    // Chase the player if detected.
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isStun && !isPlayerHidden)
        {
            agent.speed = 10;
            agent.angularSpeed = 400;
            agent.acceleration = 80;
            player = other.gameObject;
            detection = true;
        }
    }

    // If the player is no longer detected, reset the enemy.
    void OnTriggerExit(Collider other)
    {
        ResetEnemy();
    }
}
