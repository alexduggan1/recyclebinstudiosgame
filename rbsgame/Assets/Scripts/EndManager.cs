using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EndManager : MonoBehaviour
{
    public GameManager gameManager;

    public Canvas canv;

    public bool creditsOpen;
    public Image creditsScreen;


    public TextMeshProUGUI winnerText;


    public int winnerID;


    // Start is called before the first frame update
    void Awake()
    {
        creditsOpen = false;
        creditsScreen.gameObject.SetActive(false);

        foreach (HUDPlayer hudPlayer in FindObjectsByType<HUDPlayer>(FindObjectsSortMode.None))
        {
            Destroy(hudPlayer.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        List<Color> playerColors = new List<Color> { Color.red, Color.blue, Color.green, Color.yellow };
        winnerText.color = playerColors[winnerID];
        winnerText.text = "WINNER: Player " + (winnerID + 1).ToString();
    }

    public void ReceiveLeftOption(int idx)
    {
        if(idx == winnerID)
        {
            // the winner pressed the thing

            SceneManager.LoadScene("Menu");
            gameManager.menuManager.MenuReturn();
        }
    }

    public void ReceiveRightOption(int idx)
    {
        if (idx == winnerID)
        {
            // the winner pressed the thing

            if(!creditsOpen)
            {
                creditsOpen = true;
                creditsScreen.gameObject.SetActive(true);
            }
            else
            {
                creditsOpen = false;
                creditsScreen.gameObject.SetActive(false);
            }
        }
    }
}
