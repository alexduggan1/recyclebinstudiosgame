using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

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

        public enum ControllerType
        {
            None, SwitchPro, Xbox, Playstation, Arcade
        }
        public ControllerType controllerType;

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
    public Image mpStatusControllerType;


    public Player myPlayer;

    public float existTimer;

    public string sceneName;

    public TextMeshProUGUI debugText;


    public BattleController bc;

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


        menuControls.actionMap = menuControls.inputActionAsset.FindActionMap("ControlBinds");
        //Debug.Log(menuControls.actionMap);
        menuControls.actionMap.Enable();
        //Debug.Log(menuControls.actionMap.enabled);

        existTimer = 0.2f;
        Debug.Log(GetComponent<PlayerInput>().devices[0].name);
        menuControls.controllerType = MenuControls.ControllerType.None;
        if(GetComponent<PlayerInput>().devices[0].name == "WASD") { menuControls.keyboardType = MenuControls.KeyboardType.WASD; }
        else if(GetComponent<PlayerInput>().devices[0].name == "Arrows") { menuControls.keyboardType = MenuControls.KeyboardType.Arrows; }
        else 
        { 
            menuControls.keyboardType = MenuControls.KeyboardType.None;

            string desc = GetComponent<PlayerInput>().devices[0].description.ToString().ToLower();
            if (desc.Contains("xinput") || desc.Contains("microsoft"))
            {
                menuControls.controllerType = MenuControls.ControllerType.Xbox;
            }
            else if (desc.Contains("pro"))
            {
                menuControls.controllerType = MenuControls.ControllerType.SwitchPro;
            }
            else if (desc.Contains("playstation") || desc.Contains("sony") || desc.Contains("duals"))
            {
                menuControls.controllerType = MenuControls.ControllerType.Playstation;
            }
            else if (desc.Contains("dragon"))
            {
                menuControls.controllerType = MenuControls.ControllerType.Arcade;
            }


        }
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

        if(menuControls.keyboardType == MenuControls.KeyboardType.WASD) { mpStatusControllerType.sprite = menuManager.controllerTypeReps[1]; }
        else if(menuControls.keyboardType == MenuControls.KeyboardType.Arrows) { mpStatusControllerType.sprite = menuManager.controllerTypeReps[2]; }
        else { 
            if (menuControls.controllerType == MenuControls.ControllerType.Arcade)
            {
                mpStatusControllerType.sprite = menuManager.controllerTypeReps[3];
            }
            else
            {
                mpStatusControllerType.sprite = menuManager.controllerTypeReps[0];
            }
        }

        sceneName = SceneManager.GetActiveScene().name;

        debugText.text = GetComponent<PlayerInput>().devices[0].name + " ||| " + menuControls.controllerType + 
            " ||| " + GetComponent<PlayerInput>().devices[0].description;
    }

    #region menuButtonEffects

    public void Confirm()
    {
        Debug.Log("confirm for player " + GetComponent<PlayerInput>().playerIndex);

        if (existTimer <= 0)
        {
            if (sceneName == "Menu")
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
                    //Debug.Log("WASD CONFIRM");
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
                    //Debug.Log("ARROWS CONFIRM");
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
        }
    }

    
    public void Back()
    {
        // back button stuff
        Debug.Log("back for player " + GetComponent<PlayerInput>().playerIndex);

        if (sceneName == "Menu")
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

    public void RemovePlayer()
    {
        if (existTimer <= 0)
        {
            if (sceneName == "Battle")
            {

            }
            else if (sceneName == "Menu")
            {
                menuManager.menuPlayers.Remove(this);
                menuManager.playerChosenChars.RemoveAt(ID);

                foreach (MenuPlayer mp in FindObjectsByType<MenuPlayer>(FindObjectsSortMode.None))
                {
                    if (mp.ID > ID) { mp.ID--; }
                }

                if (ID == 0)
                {
                    menuManager.stageCheckMark.gameObject.SetActive(false);
                }

                Destroy(gameObject);
            }
        }
    }

    #endregion

    #region battleButtonEffects

    public void Jump()
    {
        Debug.Log("JUMP for player " + GetComponent<PlayerInput>().playerIndex);

        if (myPlayer != null)
        {
            myPlayer.Jump();
        }
    }
    
    public void LeftHand()
    {
        Debug.Log("LEFTHAND for player " + GetComponent<PlayerInput>().playerIndex);

        if (myPlayer != null)
        {
            if (bc.gamePaused && bc.whoPaused == this)
            {
                bc.BackToMenu();
            }
            else if (myPlayer.playerState.hasControl)
            {
                myPlayer.LeftHand();
            }
        }
    }

    public void RightHand()
    {
        Debug.Log("RIGHTHAND for player " + GetComponent<PlayerInput>().playerIndex);

        if (myPlayer != null)
        {
            if (bc.gamePaused && bc.whoPaused == this)
            {
                Pause();
            }
            else if (myPlayer.playerState.hasControl)
            {
                myPlayer.RightHand();
            }
        }
    }

    public void Hat()
    {
        Debug.Log("HAT for player " + GetComponent<PlayerInput>().playerIndex);

        if (myPlayer != null)
        {
            if (myPlayer.playerState.hasControl)
            {
                myPlayer.Hat();
            }
        }
    }

    public void Pause()
    {
        if (sceneName == "Battle")
        {
            Debug.Log("PAUSE!!!!");


            // TODO fix pausing stuff
            /*
            if (bc != null)
            {
                if (bc.gamePaused)
                {
                    if (bc.whoPaused == this)
                    {
                        bc.gamePaused = false;
                    }
                }
                else
                {
                    bc.whoPaused = this;
                    Time.timeScale = 0.0f;
                }
            }
            */
        }
    }

    #endregion

    #region stickControls

    public void OnKeyboardLStick(InputValue value)
    {
        if (sceneName == "Menu")
        {
            Debug.Log("navigate for player " + GetComponent<PlayerInput>().playerIndex + value.Get<Vector2>());

            menuControls.navigation = value.Get<Vector2>();
        }
        else if (sceneName == "Battle")
        {
            Debug.Log("MOVE for player " + GetComponent<PlayerInput>().playerIndex + value.Get<Vector2>());

            if (myPlayer != null)
            {
                if (myPlayer.playerState.hasControl)
                {
                    myPlayer.playerInputs.hMoveAxis = value.Get<Vector2>().x;

                    myPlayer.playerInputs.dropPressed = value.Get<Vector2>().y <= -0.7f;

                    if (menuControls.keyboardType == MenuControls.KeyboardType.Arrows)
                    {
                        if (value.Get<Vector2>().y > 0.7f)
                        {
                            Jump();
                        }
                    }
                }
            }
        }
    }

    public void OnXboxLStick(InputValue value)
    {
        if (sceneName == "Menu")
        {
            Debug.Log("navigate for player " + GetComponent<PlayerInput>().playerIndex + value.Get<Vector2>());

            menuControls.navigation = value.Get<Vector2>();
        }
        else if (sceneName == "Battle")
        {
            Debug.Log("MOVE for player " + GetComponent<PlayerInput>().playerIndex + value.Get<Vector2>());

            if (myPlayer != null)
            {
                if (myPlayer.playerState.hasControl)
                {
                    myPlayer.playerInputs.hMoveAxis = value.Get<Vector2>().x;

                    myPlayer.playerInputs.dropPressed = value.Get<Vector2>().y <= -0.7f;
                }
            }
        }
    }

    public void OnSwitchLStick(InputValue value)
    {
        if (sceneName == "Menu")
        {
            Debug.Log("navigate for player " + GetComponent<PlayerInput>().playerIndex + value.Get<Vector2>());

            menuControls.navigation = value.Get<Vector2>();
        }
        else if (sceneName == "Battle")
        {
            Debug.Log("MOVE for player " + GetComponent<PlayerInput>().playerIndex + value.Get<Vector2>());

            if (myPlayer != null)
            {
                if (myPlayer.playerState.hasControl)
                {
                    myPlayer.playerInputs.hMoveAxis = value.Get<Vector2>().x;

                    myPlayer.playerInputs.dropPressed = value.Get<Vector2>().y <= -0.7f;
                }
            }
        }
    }

    public void OnWebLStick(InputValue value)
    {
        if (sceneName == "Menu")
        {
            Debug.Log("navigate for player " + GetComponent<PlayerInput>().playerIndex + value.Get<Vector2>());

            menuControls.navigation = value.Get<Vector2>();
        }
        else if (sceneName == "Battle")
        {
            Debug.Log("MOVE for player " + GetComponent<PlayerInput>().playerIndex + value.Get<Vector2>());

            if (myPlayer != null)
            {
                if (myPlayer.playerState.hasControl)
                {
                    myPlayer.playerInputs.hMoveAxis = value.Get<Vector2>().x;

                    myPlayer.playerInputs.dropPressed = value.Get<Vector2>().y <= -0.7f;

                    if (menuControls.controllerType == MenuControls.ControllerType.Arcade)
                    {
                        if (value.Get<Vector2>().y > 0.7f)
                        {
                            Jump();
                        }
                    }
                }
            }
        }
    }

    public void OnPlaystationLStick(InputValue value)
    {
        if (sceneName == "Menu")
        {
            Debug.Log("navigate for player " + GetComponent<PlayerInput>().playerIndex + value.Get<Vector2>());

            menuControls.navigation = value.Get<Vector2>();
        }
        else if (sceneName == "Battle")
        {
            Debug.Log("MOVE for player " + GetComponent<PlayerInput>().playerIndex + value.Get<Vector2>());

            if (myPlayer != null)
            {
                if (myPlayer.playerState.hasControl)
                {
                    myPlayer.playerInputs.hMoveAxis = value.Get<Vector2>().x;

                    myPlayer.playerInputs.dropPressed = value.Get<Vector2>().y <= -0.7f;

                    if (menuControls.controllerType == MenuControls.ControllerType.Arcade)
                    {
                        if (value.Get<Vector2>().y > 0.7f)
                        {
                            Jump();
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region keyboardBindings

    public void OnKeyboardLShift()
    {
        if (sceneName == "Menu")
        {
            Debug.Log("keyboardlshift for player " + GetComponent<PlayerInput>().playerIndex);

            Back();
        }
    }

    public void OnKeyboardEscape()
    {
        Debug.Log("keyboardescape for player " + GetComponent<PlayerInput>().playerIndex);

        if (sceneName == "Menu")
        {
            RemovePlayer();
        }
        else if (sceneName == "Battle")
        {
            Pause();
        }
    }

    public void OnKeyboardSpace()
    {
        Debug.Log("keyboardspace for player " + GetComponent<PlayerInput>().playerIndex);

        if (sceneName == "Menu")
        {
            if (menuControls.keyboardType == MenuControls.KeyboardType.WASD)
            {
                Confirm();
            }
        }
        else if (sceneName == "Battle")
        {
            Jump();
        }
    }

    public void OnKeyboardQ()
    {
        if (sceneName == "Battle")
        {
            LeftHand();
        }
    }

    public void OnKeyboardE()
    {
        if (sceneName == "Battle")
        {
            Hat();
        }
    }

    public void OnKeyboardR()
    {
        if (sceneName == "Battle")
        {
            RightHand();
        }
        else if (sceneName == "Menu")
        {
            if (menuControls.keyboardType == MenuControls.KeyboardType.Arrows)
            {
                Confirm();
            }
        }
    }

    #endregion

    #region xboxBindings

    public void OnXboxA()
    {
        Debug.Log("xboxa for player " + GetComponent<PlayerInput>().playerIndex);

        if (sceneName == "Menu")
        {
            Confirm();
        }
        else if (sceneName == "Battle")
        {
            Jump();
        }
    }

    public void OnXboxB()
    {
        Debug.Log("xboxb for player " + GetComponent<PlayerInput>().playerIndex);

        if (sceneName == "Menu")
        {
            Back();
        }
        else if (sceneName == "Battle")
        {
            RightHand();
        }
    }

    public void OnXboxY()
    {
        Debug.Log("xboxy for player " + GetComponent<PlayerInput>().playerIndex);

        if (sceneName == "Battle")
        {
            Hat();
        }
    }

    public void OnXboxX()
    {
        Debug.Log("xboxx for player " + GetComponent<PlayerInput>().playerIndex);

        if (sceneName == "Battle")
        {
            LeftHand();
        }
    }

    public void OnXboxView()
    {
        Debug.Log("xboxview for player " + GetComponent<PlayerInput>().playerIndex);

        RemovePlayer();
    }

    public void OnXboxMenu()
    {
        Debug.Log("xboxmenu for player " + GetComponent<PlayerInput>().playerIndex);

        if (sceneName == "Battle")
        {
            Pause();
        }
    }

    #endregion

    #region switchBindings

    public void OnSwitchA()
    {
        Debug.Log("switcha for player " + GetComponent<PlayerInput>().playerIndex);

        if (sceneName == "Menu")
        {
            Confirm();
        }
        else if (sceneName == "Battle")
        {
            RightHand();
        }
    }

    public void OnSwitchB()
    {
        Debug.Log("switchb for player " + GetComponent<PlayerInput>().playerIndex);

        if (sceneName == "Menu")
        {
            Back();
        }
        else if (sceneName == "Battle")
        {
            Jump();
        }
    }

    public void OnSwitchY()
    {
        Debug.Log("switchy for player " + GetComponent<PlayerInput>().playerIndex);

        if (sceneName == "Battle")
        {
            LeftHand();
        }
    }

    public void OnSwitchX()
    {
        Debug.Log("switchx for player " + GetComponent<PlayerInput>().playerIndex);

        if (sceneName == "Battle")
        {
            Hat();
        }
    }

    public void OnSwitchMinus()
    {
        Debug.Log("switchminus for player " + GetComponent<PlayerInput>().playerIndex);

        RemovePlayer();
    }

    public void OnSwitchPlus()
    {
        Debug.Log("switchplus for player " + GetComponent<PlayerInput>().playerIndex);

        if (sceneName == "Battle")
        {
            Pause();
        }
    }

    #endregion

    #region webBindings

    public void OnWebEast()
    {
        Debug.Log("webeast for player " + GetComponent<PlayerInput>().playerIndex);

        if (menuControls.controllerType == MenuControls.ControllerType.SwitchPro)
        {
            OnSwitchA();
        }
        else if (menuControls.controllerType == MenuControls.ControllerType.Xbox)
        {
            OnXboxB();
        }
        else if (menuControls.controllerType == MenuControls.ControllerType.Playstation)
        {
            OnPlaystationCircle();
        }
        else if (menuControls.controllerType == MenuControls.ControllerType.Arcade)
        {
            if (sceneName == "Menu")
            {
                Confirm();
            }
            else if (sceneName == "Battle")
            {
                RightHand();
            }
        }
    }

    public void OnWebLShoulder()
    {
        Debug.Log("weblshoulder for player " + GetComponent<PlayerInput>().playerIndex);

        if (menuControls.controllerType == MenuControls.ControllerType.Arcade)
        {
            if (sceneName == "Menu")
            {
                Back();
            }
            else if (sceneName == "Battle")
            {
                Hat();
            }
        }
    }

    public void OnWebSouth()
    {
        Debug.Log("websouth for player " + GetComponent<PlayerInput>().playerIndex);

        if (menuControls.controllerType == MenuControls.ControllerType.SwitchPro)
        {
            OnSwitchB();
            
        }
        else if (menuControls.controllerType == MenuControls.ControllerType.Xbox)
        {
            OnXboxA();
        }
        else if (menuControls.controllerType == MenuControls.ControllerType.Playstation)
        {
            OnPlaystationCross();
        }
    }

    public void OnWebWest()
    {
        Debug.Log("webwest for player " + GetComponent<PlayerInput>().playerIndex);

        if (menuControls.controllerType == MenuControls.ControllerType.SwitchPro)
        {
            OnSwitchY();

        }
        else if (menuControls.controllerType == MenuControls.ControllerType.Xbox)
        {
            OnXboxX();
        }
        else if (menuControls.controllerType == MenuControls.ControllerType.Playstation)
        {
            OnPlaystationSquare();
        }
    }

    public void OnWebNorth()
    {
        Debug.Log("webnorth for player " + GetComponent<PlayerInput>().playerIndex);

        if (menuControls.controllerType == MenuControls.ControllerType.SwitchPro)
        {
            OnSwitchX();

        }
        else if (menuControls.controllerType == MenuControls.ControllerType.Xbox)
        {
            OnXboxY();
        }
        else if (menuControls.controllerType == MenuControls.ControllerType.Playstation)
        {
            OnPlaystationTriangle();
        }
        else if (menuControls.controllerType == MenuControls.ControllerType.Arcade)
        {
            if (sceneName == "Menu")
            {

            }
            else if (sceneName == "Battle")
            {
                LeftHand();
            }
        }
    }

    public void OnWebRStickPress()
    {
        Debug.Log("webrstickpress for player " + GetComponent<PlayerInput>().playerIndex);

        if (menuControls.controllerType == MenuControls.ControllerType.Arcade)
        {
            if (sceneName == "Menu")
            {
                RemovePlayer();
            }
            else if (sceneName == "Battle")
            {
                Pause();
            }
        }
    }

    public void OnWebLStickUpwards()
    {
        Debug.Log("weblstickupwards for player " + GetComponent<PlayerInput>().playerIndex);

        if (menuControls.controllerType == MenuControls.ControllerType.Arcade)
        {
            if (sceneName == "Battle")
            {
                Jump();
            }
        }
    }

    public void OnWebSelect()
    {
        Debug.Log("webselect for player " + GetComponent<PlayerInput>().playerIndex);

        if (menuControls.controllerType == MenuControls.ControllerType.SwitchPro
            || menuControls.controllerType == MenuControls.ControllerType.Xbox
            || menuControls.controllerType == MenuControls.ControllerType.Playstation)
        {
            RemovePlayer();
        }
    }

    #endregion

    #region playstationBindings

    public void OnPlaystationCross()
    {
        Debug.Log("playstationcross for player " + GetComponent<PlayerInput>().playerIndex);

        if (sceneName == "Menu")
        {
            Confirm();
        }
        else if (sceneName == "Battle")
        {
            Jump();
        }
    }

    public void OnPlaystationCircle()
    {
        Debug.Log("playstationcircle for player " + GetComponent<PlayerInput>().playerIndex);

        if (sceneName == "Menu")
        {
            Back();
        }
        else if (sceneName == "Battle")
        {
            RightHand();
        }
    }

    public void OnPlaystationTriangle()
    {
        Debug.Log("playstationtriangle for player " + GetComponent<PlayerInput>().playerIndex);

        if (sceneName == "Battle")
        {
            Hat();
        }
    }

    public void OnPlaystationSquare()
    {
        Debug.Log("playstationsquare for player " + GetComponent<PlayerInput>().playerIndex);

        if (sceneName == "Battle")
        {
            LeftHand();
        }
    }

    public void OnPlaystationShare()
    {
        Debug.Log("playstationsquare for player " + GetComponent<PlayerInput>().playerIndex);

        RemovePlayer();
    }

    public void OnPlaystationOptions()
    {
        Debug.Log("playstationoptions for player " + GetComponent<PlayerInput>().playerIndex);

        if (sceneName == "Battle")
        {
            Pause();
        }
    }

    #endregion

}
