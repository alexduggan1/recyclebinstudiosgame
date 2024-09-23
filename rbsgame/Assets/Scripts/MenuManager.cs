using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public GameManager gameManager;

    public Canvas canv;

    public List<Character> playerChosenChars = new List<Character> { };


    public GameObject elmStageProto;
    public GameObject elmItemWeightProto;


    public GameObject menuPlayerProto;
    public List<Sprite> mpReps;

    public UIElm starterUIElm;


    public List<UIElm> stageElms = new List<UIElm> { };
    public List<UIElm> itemWeightElms = new List<UIElm> { };
    public UIElm confirmElm;

    public RectTransform stageCheckMark;

    public Sprite randomCharacterIcon;


    public int sectionIWScrollAmount;
    public int sectionSScrollAmount;



    public List<MenuPlayer> menuPlayers;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        DontDestroyOnLoad(canv.gameObject);

        GenerateMenu();
    }

    public void GenerateMenu()
    {
        // generate list of stages
        int i = 0;
        stageElms.Clear();
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
            else { stageElms[j].elmBelow = stageElms[0]; }
        }
        stageElms[0].elmAbove = stageElms[^1];

        sectionSScrollAmount = 0;


        // generate list of item weights
        i = 0;
        itemWeightElms.Clear();

        UIElm confirmElm = Instantiate(elmItemWeightProto, canv.transform).GetComponent<UIElm>();
        confirmElm.attachedItemDropLoot = null; confirmElm.iWButtonType = UIElm.IWButtonTypes.Confirm;
        confirmElm.GetComponent<RectTransform>().anchoredPosition = new Vector3(160, 110 - ((i + 1) * 22), 0);
        itemWeightElms.Add(confirmElm);

        UIElm resetElm = Instantiate(elmItemWeightProto, canv.transform).GetComponent<UIElm>();
        resetElm.attachedItemDropLoot = null; resetElm.iWButtonType = UIElm.IWButtonTypes.Reset;
        resetElm.GetComponent<RectTransform>().anchoredPosition = new Vector3(160, 110 - ((i + 2) * 22), 0);
        itemWeightElms.Add(resetElm);

        foreach (BattleController.ItemDropLoot idl in gameManager.chosenItemDropLoots)
        {
            UIElm newElm = Instantiate(elmItemWeightProto, canv.transform).GetComponent<UIElm>();
            newElm.attachedItemDropLoot = idl;

            newElm.GetComponent<RectTransform>().anchoredPosition = new Vector3(160, 110 - ((i + 3) * 22), 0);

            itemWeightElms.Add(newElm);
            i++;
        }
        for (int j = 0; j < itemWeightElms.Count; j++)
        {
            if (j > 0) { itemWeightElms[j].elmAbove = itemWeightElms[j - 1]; }
            if (j < itemWeightElms.Count - 1) { itemWeightElms[j].elmBelow = itemWeightElms[j + 1]; }
            else { itemWeightElms[j].elmBelow = itemWeightElms[0]; }
        }
        itemWeightElms[0].elmAbove = itemWeightElms[^1];

        sectionIWScrollAmount = 0;


        // create menu player 1

        menuPlayers.Clear();

        /*
        for (int j = 0; j < 1; j++)
        {
            MenuPlayer newMP = Instantiate(menuPlayerProto, canv.transform).GetComponent<MenuPlayer>();
            newMP.menuManager = this;
            newMP.ID = j;
            newMP.currentUIElm = starterUIElm;

            newMP.mpRep.sprite = mpReps[j];

            if (j == 0)
            {
                newMP.menuControls = new MenuPlayer.MenuControls(KeyCode.A, KeyCode.D, KeyCode.W, KeyCode.S, KeyCode.Z, KeyCode.X);
            }
            if (j == 1)
            {
                newMP.menuControls = new MenuPlayer.MenuControls(KeyCode.F, KeyCode.H, KeyCode.T, KeyCode.G, KeyCode.V, KeyCode.B);
            }
            if (j == 2)
            {
                newMP.menuControls = new MenuPlayer.MenuControls(KeyCode.J, KeyCode.L, KeyCode.I, KeyCode.K, KeyCode.M, KeyCode.Comma);
            }
            if (j == 3)
            {
                newMP.menuControls = new MenuPlayer.MenuControls(KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.E, KeyCode.R);
            }

            menuPlayers.Add(newMP);

            playerChosenChars.Add(null);
        }
        */


        stageCheckMark.transform.SetAsLastSibling();
    }

    // Update is called once per frame
    void Update()
    {
        gameManager.playerChosenChars = playerChosenChars;

        if (menuPlayers.Count > 0)
        {
            if (menuPlayers[0].currentPanel == UIElm.PanelType.ItemWeight)
            {
                int currentHover = 0;
                int i = 0;
                foreach (UIElm iwe in itemWeightElms)
                {
                    if (iwe == menuPlayers[0].currentUIElm) { currentHover = i; }
                    i += 1;
                }

                if (currentHover > 4)
                {
                    sectionIWScrollAmount = currentHover - 4;
                }
                else { sectionIWScrollAmount = 0; }
            }
            else if (menuPlayers[0].currentPanel == UIElm.PanelType.Stage)
            {
                int currentHover = 0;
                int i = 0;
                foreach (UIElm se in stageElms)
                {
                    if (se == menuPlayers[0].currentUIElm) { currentHover = i; }
                    i += 1;
                }

                if (currentHover > 4)
                {
                    sectionSScrollAmount = currentHover - 4;
                }
                else { sectionSScrollAmount = 0; }
            }
        }
        

        for (int j = 2; j < itemWeightElms.Count; j++)
        {
            if (j + 1 - sectionIWScrollAmount <= 2 || j + 1 - sectionIWScrollAmount >= 9)
            {
                itemWeightElms[j].gameObject.SetActive(false);
            }
            else
            {
                itemWeightElms[j].gameObject.SetActive(true);
                itemWeightElms[j].GetComponent<RectTransform>().anchoredPosition = new Vector3(160, 110 - ((j + 1 - sectionIWScrollAmount) * 22), 0);
            }
        }
        for (int j = 0; j < stageElms.Count; j++)
        {
            if (j + 1 - sectionSScrollAmount <= 0 || j + 1 - sectionSScrollAmount >= 6)
            {
                stageElms[j].gameObject.SetActive(false);
            }
            else
            {
                stageElms[j].gameObject.SetActive(true);
                stageElms[j].GetComponent<RectTransform>().anchoredPosition = new Vector3(50, 110 - ((j + 1 - sectionSScrollAmount) * 40), 0);
            }
        }
    }

    public void ResetItemWeights()
    {
        gameManager.chosenItemDropLoots.Clear();
        foreach (BattleController.ItemDropLoot idl in gameManager.defaultItemDropLoots)
        {
            BattleController.ItemDropLoot nidl = new BattleController.ItemDropLoot();
            nidl.loot = idl.loot;
            nidl.weight = idl.weight;
            gameManager.chosenItemDropLoots.Add(nidl);
        }

        foreach (UIElm iwe in itemWeightElms)
        {
            if(iwe.attachedItemDropLoot != null)
            {
                iwe.weightSlider.value = 1;
            }
        }
    }

    public void GoButtonPressed()
    {
        Debug.Log("go button pressed!");

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
                    gameManager.chosenStage = gameManager.stages[Random.Range((int)1, (int)gameManager.stages.Count)];
                }

                // resolving random characters
                for (int i = 0; i < gameManager.playerChosenChars.Count; i++)
                {
                    if (gameManager.playerChosenChars[i] == null)
                    {
                        gameManager.playerChosenChars[i] = gameManager.characters[Random.Range((int)0, (int)gameManager.characters.Count)];
                    }
                }

                gameManager.listOfMenuPlayers.Clear();
                foreach (MenuPlayer mp in menuPlayers)
                {
                    gameManager.listOfMenuPlayers.Add(mp);
                    //DontDestroyOnLoad(mp.gameObject);
                    //mp.gameObject.SetActive(false);
                }

                // item drop loots
                foreach (BattleController.ItemDropLoot idl in gameManager.chosenItemDropLoots)
                {
                    foreach (UIElm iwe in itemWeightElms)
                    {
                        if(iwe.attachedItemDropLoot != null)
                        {
                            if(iwe.attachedItemDropLoot.loot == idl.loot)
                            {
                                idl.weight = iwe.attachedItemDropLoot.weight;
                            }
                        }
                    }
                }

                canv.enabled = false;

                SceneManager.LoadScene("Battle");
            }
        }
    }


    public void PlayerJoined()
    {
        Debug.Log("player joined!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
    }
}
