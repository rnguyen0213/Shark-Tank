using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Data playerData;
    public Player player;

    public Button pause;
    public GameObject menu;
    public Button continueBtn;
    public Button quit;
    public Text timer;
    public Text foodCounter;

    public GameObject end;
    public GameObject win;
    public GameObject lose;
    public Button again;
    public Button continueBtn2;

    public int numStunEnemy = 0;

    public GameObject gameOver;
    public GameObject gameStart;
    public GameObject success;

    public GameObject music;

    //Timescale = 0 : game is paused.

    void Start () {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<Data>();
        Destroy(GameObject.Find("MenuSound(Clone)"));

        pause.onClick.AddListener(PauseHandler);
        continueBtn.onClick.AddListener(ContinueHandler);
        quit.onClick.AddListener(QuitHandler);
        again.onClick.AddListener(AgainHandler);
        continueBtn2.onClick.AddListener(Continue2Handler);

        //Overlays.
        menu.SetActive(false);
        win.SetActive(false);
        lose.SetActive(false);
        end.SetActive(false);

        Instantiate(gameStart);
    }

    void Update()
    {
        //Timer.
        int time = (int) Time.timeSinceLevelLoad; 
        timer.text = "Time: " + time.ToString();
    }

    // If the player wins.
    public void PlayerWon()
    {
        Time.timeScale = 0;
        end.SetActive(true);
        win.SetActive(true);

        // Unlock the next level.
        Scene scene = SceneManager.GetActiveScene();
        int index = Int32.Parse(scene.name.ToString()) + 1;
        if(index < playerData.levelLock.Count)
            playerData.levelLock[index] = true;

        Instantiate(success);
    }

    // If the player loses.
    public void PlayerLost()
    {
        Time.timeScale = 0;
        end.SetActive(true);
        lose.SetActive(true);
        Instantiate(gameOver);
    }

    // Return to world map scene when play is beaten.
    private void Continue2Handler()
    {
        Time.timeScale = 1;
        DontDestroyOnLoad(Instantiate(music));
        SceneManager.LoadScene("World Map");
    }

    // Play the level again.
    private void AgainHandler()
    {
        Time.timeScale = 1;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name.ToString());
    }

    // Return to world map scene.
    private void QuitHandler()
    {
        Time.timeScale = 1;
        DontDestroyOnLoad(Instantiate(music));
        SceneManager.LoadScene("World Map");
    }

    // Resume pause.
    private void ContinueHandler()
    {
        menu.SetActive(false);
        Time.timeScale = 1;
    }

    // Pause game.
    private void PauseHandler()
    {
        Time.timeScale = 0;
        menu.SetActive(true);
    }
}
