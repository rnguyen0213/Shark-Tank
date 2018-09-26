using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public GameObject playerDataPrefab;
    public GameObject playerData;
    public Button play;
    public Button guide;
    public Button quit;
    public Button delete;

    public GameObject music;

	void Start () {
        // Stop menu music.
        if(GameObject.Find("MenuSound(Clone)") == null)
            DontDestroyOnLoad(Instantiate(music));

        playerData = GameObject.FindGameObjectWithTag("PlayerData");

        play.onClick.AddListener(PlayHandler);
        guide.onClick.AddListener(GuideHandler);
        quit.onClick.AddListener(QuitHandler);
        delete.onClick.AddListener(DeleteHandler);
    }

    // Wipe user data.
    private void DeleteHandler()
    {
        if (playerData != null)
            Destroy(playerData);
    }

    // Close game.
    private void QuitHandler()
    {
        Application.Quit();
    }

    // Load guide scene.
    private void GuideHandler()
    {
        SceneManager.LoadScene("Guide");
    }

    // Load world map.
    private void PlayHandler()
    {
        // Make player data if it doesnt exist.
        if(playerData == null)
        {
            playerData = Instantiate(playerDataPrefab);
            playerData.name = "PlayerData";
            DontDestroyOnLoad(playerData);      
        }
        SceneManager.LoadScene("World Map");
    }
}
