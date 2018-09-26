using UnityEngine;
using UnityEngine.AI;

public class BlowFishController : MonoBehaviour
{
    public GameManager GM;
    public Transform[] points;
    public NavMeshAgent agent;
    public Renderer rend;

    public bool stunnable = true;
    public bool isStun = false;
    public bool isPlayerHidden = false;

    public int destPoint = 0;
    public float speed = 6.0f;
    public float angularSpeed = 240.0f;
    public float acceleration = 8.0f;
    public float stunTime = 0;

    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("UI").GetComponent<GameManager>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        GotoNextPoint();
    }

    void Update()
    {
        // Normal AI behavior.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();

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

        // Six second cooldown for stunning.
        if (Time.time - stunTime > 6)
            stunnable = true;
    }

    // Handle stunning.
    public void StunEnemy()
    {
        rend.material.color = Color.red;
        agent.isStopped = true;
        stunTime = Time.time;
        stunnable = false;
        isStun = true;
        GM.numStunEnemy++;
    }

    // Handle pathing points for AI.
    private void GotoNextPoint()
    {
        if (points.Length == 0)
            return;

        if (points.Length == 1)
            agent.autoBraking = true;

        agent.destination = points[destPoint].position;
        destPoint = (destPoint + 1) % points.Length;
    }

    // Let this enemy object know if the player is hidden.
    public void HiddenPlayer(bool hidden)
    {
        isPlayerHidden = hidden;
    }

    // If collision with player is detected while player is not hiding then the player will lose the game.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isStun && !isPlayerHidden)
        {
            GameObject tmp = transform.Find("Blowfish").gameObject;

            //Expand blowfish for effect
            tmp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            tmp.transform.localPosition = new Vector3(0.86f, -0.87f, 0.98f);
            
            //Let the game manager know the player has lost.
            GM.PlayerLost();
        }
    }
}
