using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameManager gameManager;

    public Canvas canv;

    public List<Character> playerChosenChars = new List<Character> { };


    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        

        gameManager.playerChosenChars = playerChosenChars;
    }

    public void GoButtonPressed()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            SceneManager.LoadScene("Battle");
        }
    }
}
