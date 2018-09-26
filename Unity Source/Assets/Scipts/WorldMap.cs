using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldMap : MonoBehaviour {

    public Data playerData;
    public Button[] levelList;
    public Button back;
    public Button levelZero;
    public Button levelOne;
    public Button levelTwo;
    public Button levelThree;
    public Button levelFour;
    public Button levelFive;
    public Button levelSix;
    public Button levelSeven;
    public Button levelEight;
    public Button levelNine;
    public Button levelTen;

    public GameObject wrongAction; // sound

    //Manage what levels have been unlocked.
    void Start () {
        playerData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<Data>();

        for (int i = 0; i < levelList.Length; i++)
        {
            if (playerData.levelLock[i] == true)
            {
                ColorBlock cb = levelList[i].colors;
                cb.normalColor = Color.white;
                cb.highlightedColor = Color.white;
                levelList[i].colors = cb;
            }
        }

        back.onClick.AddListener(BackHandler);
        levelZero.onClick.AddListener(LevelZeroHandler);
        levelOne.onClick.AddListener(LevelOneHandler);
        levelTwo.onClick.AddListener(LevelTwoHandler);
        levelThree.onClick.AddListener(LevelThreeHandler);
        levelFour.onClick.AddListener(LevelFourHandler);
        levelFive.onClick.AddListener(LevelFiveHandler);
        levelSix.onClick.AddListener(LevelSixHandler);
        levelSeven.onClick.AddListener(LevelSevenHandler);
        levelEight.onClick.AddListener(LevelEightHandler);
        levelNine.onClick.AddListener(LevelNineHandler);
        levelTen.onClick.AddListener(LevelTenHandler);
    }

    // All Handlers will manage a level scene and will play wrong action sound if the player trys to play a locked level.

    private void BackHandler()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private void LevelZeroHandler()
    {
        SceneManager.LoadScene("0");
    }

    private void LevelOneHandler()
    {
        if(playerData.levelLock[1])
            SceneManager.LoadScene("1");
        else
            Instantiate(wrongAction);
    }

    private void LevelTwoHandler()
    {
        if (playerData.levelLock[2])
            SceneManager.LoadScene("2");
        else
            Instantiate(wrongAction);
    }

    private void LevelThreeHandler()
    {
        if (playerData.levelLock[3])
            SceneManager.LoadScene("3");
        else
            Instantiate(wrongAction);
    }

    private void LevelFourHandler()
    {
        if (playerData.levelLock[4])
            SceneManager.LoadScene("4");
        else
            Instantiate(wrongAction);
    }

    private void LevelFiveHandler()
    {
        if (playerData.levelLock[5])
            SceneManager.LoadScene("5");
        else
            Instantiate(wrongAction);
    }

    private void LevelTenHandler()
    {
        if (playerData.levelLock[10])
            SceneManager.LoadScene("10");
        else
            Instantiate(wrongAction);
    }

    private void LevelNineHandler()
    {
        if (playerData.levelLock[9])
            SceneManager.LoadScene("9");
        else
            Instantiate(wrongAction);
    }

    private void LevelEightHandler()
    {
        if (playerData.levelLock[8])
            SceneManager.LoadScene("8");
        else
            Instantiate(wrongAction);
    }

    private void LevelSevenHandler()
    {
        if (playerData.levelLock[7])
            SceneManager.LoadScene("7");
        else
            Instantiate(wrongAction);
    }

    private void LevelSixHandler()
    {
        if (playerData.levelLock[6])
            SceneManager.LoadScene("6");
        else
            Instantiate(wrongAction);
    }
}
