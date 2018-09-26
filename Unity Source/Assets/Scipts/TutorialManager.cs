using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

    public GameObject[] tutorial;
    public Button next;
    public int nextCount = 0;

	void Start () {
        Time.timeScale = 0;
        tutorial[4].SetActive(false);
        tutorial[5].SetActive(false);
        tutorial[6].SetActive(false);
        next.onClick.AddListener(NextHandler);
    }

    // cycle through tutorial panel.
    private void NextHandler()
    {
        if (nextCount == 0)
        {
            tutorial[0].SetActive(false);
            tutorial[1].SetActive(false);
            tutorial[2].SetActive(false);
            tutorial[3].SetActive(false);
            tutorial[4].SetActive(true);
            tutorial[5].SetActive(true);
            tutorial[6].SetActive(true);
            nextCount++;
        }
        else if (nextCount == 1)
        {
            tutorial[0].SetActive(false);
            tutorial[1].SetActive(false);
            tutorial[2].SetActive(false);
            tutorial[3].SetActive(false);
            tutorial[4].SetActive(false);
            tutorial[5].SetActive(false);
            tutorial[6].SetActive(false);
            next.gameObject.SetActive(false);
            nextCount++;
            Time.timeScale = 1;
        }

    }
}
