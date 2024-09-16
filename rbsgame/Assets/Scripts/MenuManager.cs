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


    public GameObject elmStageProto;
    public GameObject elmItemWeightProto;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        GenerateMenu();
    }

    public void GenerateMenu()
    {
        int i = 0;
        foreach (GameObject stage in gameManager.stages)
        {
            UIElm newElm = Instantiate(elmStageProto, canv.transform).GetComponent<UIElm>();
            newElm.attachedStage = stage;

            newElm.GetComponent<RectTransform>().anchoredPosition = new Vector3(50, 110 - ((i + 1) * 40), 0);
            i++;
        }


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
            // do all the stuff to make sure things work

            gameManager.playerChosenChars = playerChosenChars;

            if(gameManager.chosenStage == null)
            {
                gameManager.chosenStage = gameManager.stages[Random.Range((int)0, (int)gameManager.stages.Count)];
            }


            SceneManager.LoadScene("Battle");
        }
    }
}
