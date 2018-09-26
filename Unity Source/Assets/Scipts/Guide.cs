using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Guide : MonoBehaviour {

    public Button back;

	void Start () {
        back.onClick.AddListener(BackHandler);
	}

    // Return to world map scene. 
    private void BackHandler()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
