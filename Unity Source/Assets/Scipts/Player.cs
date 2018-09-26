using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public Button screenShake;
    public Renderer rend;
    public GameManager GM;
    public BoxCollider box1;
    public BoxCollider box2;
    public Material orange;

    public bool isHidden = false;
    public bool hiddable = true;
    public bool moveable = true;
    public float hideTime = 0.0f;
    public float hideResetTimer = 0.0f;
    public bool resetHide = false;
    public int screenShakeCount = 1;

    public GameObject food;
    public int foodCount = 2;

    public GameObject[] enemyList;

    void Start () {
        GM = GameObject.FindGameObjectWithTag("UI").GetComponent<GameManager>();
        enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        GetComponent<AudioSource>().Pause();

        GM.foodCounter.text = "Food: " + foodCount;
        screenShake.onClick.AddListener(StunAll);
	}

    void Update () {
        
        // Player movement.
        if(moveable)
            MovePlayer();

        // Handle player clicking.
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                // If player is clicked, hide the player.
                if (hit.transform.gameObject.name.CompareTo("Player Touch") == 0 && hiddable)
                {
                    rend.material.color = Color.green;
                    hideTime = Time.time;
                    isHidden = true;
                    hiddable = false;
                    moveable = false;
                    box1.isTrigger = false;
                    box2.isTrigger = false;
                }

                // Spawn food onto the map.
                else if (hit.transform.gameObject.name.CompareTo("Background") == 0 || hit.transform.gameObject.name.CompareTo("Shark") == 0 || hit.transform.gameObject.name.CompareTo("Vision2") == 0)
                {
                    // If there is food left and the game is not paused.
                    if (foodCount > 0 && Time.timeScale == 1)
                    {
                        // Adjust height so it can collide with player.
                        Vector3 pos = hit.point;
                        pos.y = 3;

                        GameObject tmp = Instantiate(food, pos, Quaternion.identity);
                        foodCount--;
                        GM.foodCounter.text = "Food: " + foodCount;

                        // Tell every enemy that food has spawn.
                        foreach (GameObject go in enemyList)
                        {
                            if(go.name == "Shark")
                                go.GetComponent<SharkController>().AddFood(tmp);
                        }
                    }
                }
            }
        }

        // hiding cooldown.
        if(resetHide && Time.time > hideResetTimer + 3)
        {
            hiddable = true;
            resetHide = false;
        }

        // A delay so the player doesnt accidently unhide.
        if(Time.time > hideTime + 1)
            moveable = true;      
    }

    // Shake screen feature. Stun all enemies
    private void StunAll()
    {
        if(screenShakeCount > 0)
        {
            foreach (GameObject go in enemyList)
            {
                if (go.name == "Shark" || go.name == "Blowfish")
                    go.SendMessage("StunEnemy");
            }
            screenShakeCount--;
        }
    }

    // Handle player movement
    private void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        float z = (Input.GetAxis("Vertical") * Time.deltaTime * 10f) * -1;

        // If the player is moving then play movement sound
        if((x != 0 || z != 0))
            GetComponent<AudioSource>().UnPause();
        else
            GetComponent<AudioSource>().Pause();


        transform.position = new Vector3(transform.position.x, 3, transform.position.z);
        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
        
        // If the player moves then unhide if player was hiding
        if (z != 0 && isHidden)
        {
            box1.isTrigger = true;
            box2.isTrigger = true;
            isHidden = false;
            resetHide = true;
            hideResetTimer = Time.time;
            rend.material = orange;
        }
    }


    void OnTriggerStay(Collider other)
    {
        // Let the enemy know if the player is hiding.
        if (other != null && other.gameObject.tag.CompareTo("Enemy") == 0)
        {
            SharkController enemy = other.gameObject.GetComponent<SharkController>();
            if (enemy != null)
                enemy.HiddenPlayer(isHidden);
        }          

        // Player had won, let game manager know.
        if (other.gameObject.tag == "Finish")
            GM.PlayerWon();

        // Player has lost, let the game manage know.
        if (other.gameObject.tag == "Caught")
        {
            GM.PlayerLost();
            //Edge case, where the blowfish main body(not the vision) collides with a hidden player.
            if(other.gameObject.name == "Blowfish" && isHidden)
            {
                other.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                other.transform.localPosition = new Vector3(0.86f, -0.87f, 0.98f);
            }
        }
            
    }
}
