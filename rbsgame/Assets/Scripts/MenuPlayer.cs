using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuPlayer : MonoBehaviour
{
    // have a thing for the controls here
    [System.Serializable]
    public class MenuControls
    {
        public Vector2 navigation = Vector2.zero;
        public float lastNav = 0;

        public InputActionMap actionMap;
        public InputActionAsset inputActionAsset;

        public KeyCode leftButton;
        public KeyCode rightButton;
        public KeyCode upButton;
        public KeyCode downButton;
        public KeyCode confirmButton;
        public KeyCode backButton;

        public enum KeyboardType
        {
            None, WASD, Arrows
        }
        public KeyboardType keyboardType;

        public MenuControls(KeyCode _leftButton, KeyCode _rightButton, KeyCode _upButton, KeyCode _downButton,
            KeyCode _confirmButton, KeyCode _backButton)
        {
            leftButton = _leftButton;
            rightButton = _rightButton;
            upButton = _upButton;
            downButton = _downButton;
            confirmButton = _confirmButton;
            backButton = _backButton;
        }
    }

    public MenuControls menuControls;

    public int ID;

    public Character chosenChar;


    public UIElm currentUIElm;

    public UIElm.PanelType currentPanel;

    public MenuManager menuManager;

    public bool hasChar;

    public Image mpRep;

    public Image mpStatusBar;
    public Image mpStatusCircle;
    public Image mpStatusChar;


    public Player myPlayer;

    public float existTimer;

    // Start is called before the first frame update
    void Awake()
    {
        currentPanel = UIElm.PanelType.Character;
        hasChar = false;

        menuManager = FindFirstObjectByType<MenuManager>();
        transform.SetParent(menuManager.canv.transform);


        int j = menuManager.menuPlayers.Count;

        ID = j;
        currentUIElm = menuManager.starterUIElm;

        mpRep.sprite = menuManager.mpReps[j];



        menuManager.menuPlayers.Add(this);

        menuManager.playerChosenChars.Add(null);


        menuControls.actionMap = menuControls.inputActionAsset.FindActionMap("UI");
        //Debug.Log(menuControls.actionMap);
        menuControls.actionMap.Enable();
        //Debug.Log(menuControls.actionMap.enabled);

        existTimer = 0.2f;
        Debug.Log(GetComponent<PlayerInput>().devices[0].name);
        if(GetComponent<PlayerInput>().devices[0].name == "WASD") { menuControls.keyboardType = MenuControls.KeyboardType.WASD; }
        else if(GetComponent<PlayerInput>().devices[0].name == "Arrows") { menuControls.keyboardType = MenuControls.KeyboardType.Arrows; }
        else { menuControls.keyboardType = MenuControls.KeyboardType.None; }
    }

    // Update is called once per frame
    void Update()
    {
        if(existTimer >= 0) { existTimer -= Time.deltaTime; }

        mpRep.sprite = menuManager.mpReps[ID];

        RectTransform elmRectOn = currentUIElm.gameObject.GetComponent<RectTransform>();
        if (ID == 0)
        {
            Vector3[] v = new Vector3[4]; elmRectOn.GetLocalCorners(v);
            GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-180, -65, 0);
            GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            mpRep.rectTransform.anchoredPosition = elmRectOn.anchoredPosition3D + v[1] - GetComponent<RectTransform>().anchoredPosition3D;
            mpStatusBar.color = Color.red;
            mpStatusCircle.color = Color.red;
        }
        if (ID == 1)
        {
            Vector3[] v = new Vector3[4]; elmRectOn.GetLocalCorners(v);
            GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-100, -65, 0);
            GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            mpRep.rectTransform.anchoredPosition = elmRectOn.anchoredPosition3D + v[2] - GetComponent<RectTransform>().anchoredPosition3D;
            mpStatusBar.color = Color.blue;
            mpStatusCircle.color = Color.blue;
        }
        if (ID == 2)
        {
            mpRep.rectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
            Vector3[] v = new Vector3[4]; elmRectOn.GetLocalCorners(v);
            GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-180, -100, 0);
            GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            mpRep.rectTransform.anchoredPosition = elmRectOn.anchoredPosition3D + v[0] - GetComponent<RectTransform>().anchoredPosition3D;
            mpStatusBar.color = Color.green;
            mpStatusCircle.color = Color.green;
        }
        if (ID == 3)
        {
            mpRep.rectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
            Vector3[] v = new Vector3[4]; elmRectOn.GetLocalCorners(v);
            GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-100, -100, 0);
            GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            mpRep.rectTransform.anchoredPosition = elmRectOn.anchoredPosition3D + v[3] - GetComponent<RectTransform>().anchoredPosition3D;
            mpStatusBar.color = Color.yellow;
            mpStatusCircle.color = Color.yellow;
        }

        if (hasChar)
        {
            chosenChar = menuManager.playerChosenChars[ID];
            if (chosenChar != null)
            {
                mpStatusChar.sprite = chosenChar.render;
            }
            else
            {
                mpStatusChar.sprite = menuManager.randomCharacterIcon;
            }
            mpStatusChar.preserveAspect = true;
            mpStatusChar.enabled = true;
        }
        else
        {
            mpStatusChar.sprite = null;
            mpStatusChar.enabled = false;
        }


        if (! ((ID != 0) && hasChar))
        {
            if (menuControls.navigation.x < -0.5f && menuControls.lastNav <= 0)
            {
                if (currentPanel != UIElm.PanelType.ItemWeight)
                {
                    if (currentUIElm.elmLeft != null)
                    {
                        currentUIElm = currentUIElm.elmLeft;
                        menuControls.lastNav = 0.2f;
                    }
                }
                else
                {
                    // change the item weight
                    currentUIElm.weightSlider.value -= 1;
                    menuControls.lastNav = 0.1f;
                }
            }
            if (menuControls.navigation.x > 0.5f && menuControls.lastNav <= 0)
            {
                if (currentPanel != UIElm.PanelType.ItemWeight)
                {
                    if (currentUIElm.elmRight != null)
                    {
                        currentUIElm = currentUIElm.elmRight;
                        menuControls.lastNav = 0.2f;
                    }
                }
                else
                {
                    // change the item weight
                    currentUIElm.weightSlider.value += 1;
                    menuControls.lastNav = 0.1f;
                }
            }


            if (menuControls.navigation.y > 0.5f && menuControls.lastNav <= 0)
            {
                if (currentUIElm.elmAbove != null) { currentUIElm = currentUIElm.elmAbove; }
                menuControls.lastNav = 0.2f;
            }
            if (menuControls.navigation.y < -0.5f && menuControls.lastNav <= 0)
            {
                if (currentUIElm.elmBelow != null) { currentUIElm = currentUIElm.elmBelow; }
                menuControls.lastNav = 0.2f;
            }

            if (menuControls.lastNav > 0)
            {
                menuControls.lastNav -= Time.deltaTime;
            }
        }

        /*
        if (menuControls.confirmAction.action.phase == InputActionPhase.Performed)
        {
            if (ID == 0)
            {
                // i am player 1
                if (currentPanel == UIElm.PanelType.Character)
                {
                    menuManager.playerChosenChars[ID] = currentUIElm.attachedCharacter;
                    hasChar = true;
                    currentPanel = UIElm.PanelType.Stage;
                    currentUIElm = menuManager.stageElms[0];
                }
                else if (currentPanel == UIElm.PanelType.Stage)
                {
                    menuManager.gameManager.chosenStage = currentUIElm.attachedStage;
                    menuManager.stageCheckMark.anchoredPosition = currentUIElm.GetComponent<RectTransform>().anchoredPosition;
                    menuManager.stageCheckMark.gameObject.SetActive(true);
                    currentPanel = UIElm.PanelType.ItemWeight;
                    currentUIElm = menuManager.itemWeightElms[0];
                }
                else if (currentPanel == UIElm.PanelType.ItemWeight)
                {
                    if(currentUIElm.attachedItemDropLoot != null)
                    {
                        currentPanel = UIElm.PanelType.Confirm;
                        currentUIElm = menuManager.confirmElm;
                    }
                    else
                    {
                        if(currentUIElm.iWButtonType == UIElm.IWButtonTypes.Confirm)
                        {
                            currentPanel = UIElm.PanelType.Confirm;
                            currentUIElm = menuManager.confirmElm;
                        }
                        else
                        {
                            menuManager.ResetItemWeights();
                        }
                    }
                }
                else if (currentPanel == UIElm.PanelType.Confirm)
                {
                    menuManager.GoButtonPressed();
                }
            }
            else
            {
                menuManager.playerChosenChars[ID] = currentUIElm.attachedCharacter;
                hasChar = true;
            }
        }
        if (menuControls.backAction.action.phase == InputActionPhase.Performed)
        {
            // back button stuff
            if (ID == 0)
            {
                if (currentPanel == UIElm.PanelType.Character)
                {
                    menuManager.playerChosenChars[ID] = null;
                    hasChar = false;
                }
                else if (currentPanel == UIElm.PanelType.Stage)
                {
                    currentPanel = UIElm.PanelType.Character;
                    menuManager.gameManager.chosenStage = null;
                    menuManager.stageCheckMark.gameObject.SetActive(false);
                    currentUIElm = menuManager.starterUIElm;
                }
                else if (currentPanel == UIElm.PanelType.ItemWeight)
                {
                    currentPanel = UIElm.PanelType.Stage;
                    menuManager.stageCheckMark.gameObject.SetActive(false);
                    currentUIElm = menuManager.stageElms[0];
                }
                else if (currentPanel == UIElm.PanelType.Confirm)
                {
                    currentPanel = UIElm.PanelType.ItemWeight;
                    currentUIElm = menuManager.itemWeightElms[0];
                }
            }
            else
            {
                menuManager.playerChosenChars[ID] = null;
                hasChar = false;
            }
        }
        */
    }

    public void OnNavigate(InputValue value)
    {
        Debug.Log("navigate for player " + GetComponent<PlayerInput>().playerIndex + value.Get<Vector2>());

        menuControls.navigation = value.Get<Vector2>();
    }

    public void OnConfirm()
    {
        Debug.Log("confirm for player " + GetComponent<PlayerInput>().playerIndex);

        if (existTimer <= 0)
        {
            if (SceneManager.GetActiveScene().name == "Menu")
            {
                if (menuControls.keyboardType == MenuControls.KeyboardType.None)
                {
                    if (ID == 0)
                    {
                        // i am player 1
                        if (currentPanel == UIElm.PanelType.Character)
                        {
                            menuManager.playerChosenChars[ID] = currentUIElm.attachedCharacter;
                            hasChar = true;
                            currentPanel = UIElm.PanelType.Stage;
                            currentUIElm = menuManager.stageElms[0];
                        }
                        else if (currentPanel == UIElm.PanelType.Stage)
                        {
                            menuManager.gameManager.chosenStage = currentUIElm.attachedStage;
                            menuManager.stageCheckMark.anchoredPosition = currentUIElm.GetComponent<RectTransform>().anchoredPosition;
                            menuManager.stageCheckMark.gameObject.SetActive(true);
                            currentPanel = UIElm.PanelType.ItemWeight;
                            currentUIElm = menuManager.itemWeightElms[0];
                        }
                        else if (currentPanel == UIElm.PanelType.ItemWeight)
                        {
                            if (currentUIElm.attachedItemDropLoot != null)
                            {
                                currentPanel = UIElm.PanelType.Confirm;
                                currentUIElm = menuManager.confirmElm;
                            }
                            else
                            {
                                if (currentUIElm.iWButtonType == UIElm.IWButtonTypes.Confirm)
                                {
                                    currentPanel = UIElm.PanelType.Confirm;
                                    currentUIElm = menuManager.confirmElm;
                                }
                                else
                                {
                                    menuManager.ResetItemWeights();
                                }
                            }
                        }
                        else if (currentPanel == UIElm.PanelType.Confirm)
                        {
                            menuManager.GoButtonPressed();
                        }
                    }
                    else
                    {
                        menuManager.playerChosenChars[ID] = currentUIElm.attachedCharacter;
                        hasChar = true;
                    }
                }
                else if (menuControls.keyboardType == MenuControls.KeyboardType.WASD)
                {
                    Debug.Log("WASD CONFIRM");
                    if (ID == 0)
                    {
                        // i am player 1
                        if (currentPanel == UIElm.PanelType.Character)
                        {
                            menuManager.playerChosenChars[ID] = currentUIElm.attachedCharacter;
                            hasChar = true;
                            currentPanel = UIElm.PanelType.Stage;
                            currentUIElm = menuManager.stageElms[0];
                        }
                        else if (currentPanel == UIElm.PanelType.Stage)
                        {
                            menuManager.gameManager.chosenStage = currentUIElm.attachedStage;
                            menuManager.stageCheckMark.anchoredPosition = currentUIElm.GetComponent<RectTransform>().anchoredPosition;
                            menuManager.stageCheckMark.gameObject.SetActive(true);
                            currentPanel = UIElm.PanelType.ItemWeight;
                            currentUIElm = menuManager.itemWeightElms[0];
                        }
                        else if (currentPanel == UIElm.PanelType.ItemWeight)
                        {
                            if (currentUIElm.attachedItemDropLoot != null)
                            {
                                currentPanel = UIElm.PanelType.Confirm;
                                currentUIElm = menuManager.confirmElm;
                            }
                            else
                            {
                                if (currentUIElm.iWButtonType == UIElm.IWButtonTypes.Confirm)
                                {
                                    currentPanel = UIElm.PanelType.Confirm;
                                    currentUIElm = menuManager.confirmElm;
                                }
                                else
                                {
                                    menuManager.ResetItemWeights();
                                }
                            }
                        }
                        else if (currentPanel == UIElm.PanelType.Confirm)
                        {
                            menuManager.GoButtonPressed();
                        }
                    }
                    else
                    {
                        menuManager.playerChosenChars[ID] = currentUIElm.attachedCharacter;
                        hasChar = true;
                    }
                }
                else if (menuControls.keyboardType == MenuControls.KeyboardType.Arrows)
                {
                    Debug.Log("ARROWS CONFIRM");
                    if (ID == 0)
                    {
                        // i am player 1
                        if (currentPanel == UIElm.PanelType.Character)
                        {
                            menuManager.playerChosenChars[ID] = currentUIElm.attachedCharacter;
                            hasChar = true;
                            currentPanel = UIElm.PanelType.Stage;
                            currentUIElm = menuManager.stageElms[0];
                        }
                        else if (currentPanel == UIElm.PanelType.Stage)
                        {
                            menuManager.gameManager.chosenStage = currentUIElm.attachedStage;
                            menuManager.stageCheckMark.anchoredPosition = currentUIElm.GetComponent<RectTransform>().anchoredPosition;
                            menuManager.stageCheckMark.gameObject.SetActive(true);
                            currentPanel = UIElm.PanelType.ItemWeight;
                            currentUIElm = menuManager.itemWeightElms[0];
                        }
                        else if (currentPanel == UIElm.PanelType.ItemWeight)
                        {
                            if (currentUIElm.attachedItemDropLoot != null)
                            {
                                currentPanel = UIElm.PanelType.Confirm;
                                currentUIElm = menuManager.confirmElm;
                            }
                            else
                            {
                                if (currentUIElm.iWButtonType == UIElm.IWButtonTypes.Confirm)
                                {
                                    currentPanel = UIElm.PanelType.Confirm;
                                    currentUIElm = menuManager.confirmElm;
                                }
                                else
                                {
                                    menuManager.ResetItemWeights();
                                }
                            }
                        }
                        else if (currentPanel == UIElm.PanelType.Confirm)
                        {
                            menuManager.GoButtonPressed();
                        }
                    }
                    else
                    {
                        menuManager.playerChosenChars[ID] = currentUIElm.attachedCharacter;
                        hasChar = true;
                    }
                }
            }
            else
            {
                if (menuControls.keyboardType == MenuControls.KeyboardType.WASD)
                {
                    Debug.Log("WASD CONFIRM");
                    OnJump();
                }
            }
        }
    }

    public void OnBack()
    {
        // back button stuff
        Debug.Log("back for player " + GetComponent<PlayerInput>().playerIndex);

        if (SceneManager.GetActiveScene().name == "Menu")
        {
            if (ID == 0)
            {
                if (currentPanel == UIElm.PanelType.Character)
                {
                    menuManager.playerChosenChars[ID] = null;
                    hasChar = false;
                }
                else if (currentPanel == UIElm.PanelType.Stage)
                {
                    currentPanel = UIElm.PanelType.Character;
                    menuManager.playerChosenChars[ID] = null;
                    hasChar = false;
                    menuManager.gameManager.chosenStage = null;
                    menuManager.stageCheckMark.gameObject.SetActive(false);
                    currentUIElm = menuManager.starterUIElm;
                }
                else if (currentPanel == UIElm.PanelType.ItemWeight)
                {
                    currentPanel = UIElm.PanelType.Stage;
                    menuManager.stageCheckMark.gameObject.SetActive(false);
                    currentUIElm = menuManager.stageElms[0];
                }
                else if (currentPanel == UIElm.PanelType.Confirm)
                {
                    currentPanel = UIElm.PanelType.ItemWeight;
                    currentUIElm = menuManager.itemWeightElms[0];
                }
            }
            else
            {
                menuManager.playerChosenChars[ID] = null;
                hasChar = false;
            }
        }
    }

    public void OnJump()
    {
        //Debug.Log("JUMP for player " + GetComponent<PlayerInput>().playerIndex);

        if (SceneManager.GetActiveScene().name == "Menu")
        {
            if (menuControls.keyboardType == MenuControls.KeyboardType.Arrows)
            {
                menuControls.navigation = Vector2.up;
            }
        }
        else
        {
            if (myPlayer != null)
            {
                myPlayer.Jump();
                //Debug.Log("player not null");
            }
        }
    }

    public void OnMove(InputValue value)
    {
        //Debug.Log("MOVE for player " + GetComponent<PlayerInput>().playerIndex);

        if (myPlayer != null)
        {
            myPlayer.playerInputs.hMoveAxis = value.Get<Vector2>().x;

            myPlayer.playerInputs.dropPressed = value.Get<Vector2>().y <= -0.7f;
        }
    }

    public void OnLeftHand()
    {
        //Debug.Log("LEFTHAND for player " + GetComponent<PlayerInput>().playerIndex);

        if (myPlayer != null)
        {
            myPlayer.LeftHand();
        }
    }

    public void OnRightHand()
    {
        //Debug.Log("RIGHTHAND for player " + GetComponent<PlayerInput>().playerIndex);

        if (SceneManager.GetActiveScene().name == "Battle")
        {
            if (myPlayer != null)
            {
                myPlayer.RightHand();
                //Debug.Log("player not null");
            }
        }
        else
        {
            OnBack();
        }
    }

    public void OnHat()
    {
        //Debug.Log("HAT for player " + GetComponent<PlayerInput>().playerIndex);

        if (myPlayer != null)
        {
            myPlayer.Hat();
        }
    }

    public void OnRemovePlayer()
    {
        if (existTimer <= 0)
        {
            if (SceneManager.GetActiveScene().name == "Battle")
            {

            }
            else
            {
                menuManager.menuPlayers.Remove(this);
                menuManager.playerChosenChars.RemoveAt(ID);

                foreach (MenuPlayer mp in FindObjectsByType<MenuPlayer>(FindObjectsSortMode.None))
                {
                    if (mp.ID > ID) { mp.ID--; }
                }

                Destroy(gameObject);
            }
        }
    }
}
