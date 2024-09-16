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


    public GameObject menuPlayerProto;

    public UIElm starterUIElm;


    public List<UIElm> stageElms = new List<UIElm> { };
    public List<UIElm> itemWeightElms = new List<UIElm> { };
    public UIElm confirmElm;

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

            stageElms.Add(newElm);
            i++;
        }
        for (int j = 0; j < stageElms.Count; j++)
        {
            if(j > 0) { stageElms[j].elmAbove = stageElms[j - 1]; }
            if(j < stageElms.Count - 1) { stageElms[j].elmBelow = stageElms[j + 1]; }
        }


        // create menu players automatically for now I guess

        for (int j = 0; j < 2; j++)
        {
            MenuPlayer newMP = Instantiate(menuPlayerProto).GetComponent<MenuPlayer>();
            newMP.menuManager = this;
            newMP.ID = j;
            newMP.currentUIElm = starterUIElm;

            if(j == 0)
            {
                newMP.menuControls = new MenuPlayer.MenuControls(KeyCode.A, KeyCode.D, KeyCode.W, KeyCode.S, KeyCode.Z, KeyCode.X);
            }
            if (j == 1)
            {
                newMP.menuControls = new MenuPlayer.MenuControls(KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.Keypad1, KeyCode.Keypad3);
            }
            playerChosenChars.Add(null);
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
            bool valid = true;
            foreach (MenuPlayer mp in FindObjectsByType<MenuPlayer>(FindObjectsSortMode.None))
            {
                if(!mp.hasChar) { valid = false; }
            }

            if (valid)
            {
                // do all the stuff to make sure things work

                gameManager.playerChosenChars = playerChosenChars;


                // resolving random stages
                if (gameManager.chosenStage == null)
                {
                    gameManager.chosenStage = gameManager.stages[Random.Range((int)0, (int)gameManager.stages.Count)];
                }

                // resolving random characters
                for (int i = 0; i < gameManager.playerChosenChars.Count; i++)
                {
                    if (gameManager.playerChosenChars[i] == null)
                    {
                        gameManager.playerChosenChars[i] = gameManager.characters[Random.Range((int)0, (int)gameManager.characters.Count)];
                    }
                }

                SceneManager.LoadScene("Battle");
            }
        }
    }
}
