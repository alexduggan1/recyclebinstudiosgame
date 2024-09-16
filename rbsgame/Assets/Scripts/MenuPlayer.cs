using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPlayer : MonoBehaviour
{
    // have a thing for the controls here
    public class MenuControls
    {
        public KeyCode leftButton;
        public KeyCode rightButton;
        public KeyCode upButton;
        public KeyCode downButton;
        public KeyCode confirmButton;
        public KeyCode backButton;

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

    // Start is called before the first frame update
    void Awake()
    {
        currentPanel = UIElm.PanelType.Character;
        hasChar = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(menuControls.leftButton)) {
            if (currentUIElm.elmLeft != null) {
                if(currentPanel != UIElm.PanelType.ItemWeight)
                {
                    currentUIElm = currentUIElm.elmLeft;
                }
                else
                {
                    // change the item weight
                }
            }
        }
        if (Input.GetKeyDown(menuControls.rightButton)) {
            if (currentUIElm.elmRight != null) {
                if (currentPanel != UIElm.PanelType.ItemWeight)
                {
                    currentUIElm = currentUIElm.elmRight;
                }
                else
                {
                    // change the item weight
                }
            }
        }
        
        
        if (Input.GetKeyDown(menuControls.upButton)) {
            if (currentUIElm.elmAbove != null) { currentUIElm = currentUIElm.elmAbove; }}
        if (Input.GetKeyDown(menuControls.downButton)) {
            if (currentUIElm.elmBelow != null) { currentUIElm = currentUIElm.elmBelow; }}

        if (Input.GetKeyDown(menuControls.confirmButton))
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
                    currentPanel = UIElm.PanelType.ItemWeight;
                    currentUIElm = menuManager.itemWeightElms[0];
                }
                else if (currentPanel == UIElm.PanelType.ItemWeight)
                {
                    currentPanel = UIElm.PanelType.Confirm;
                    currentUIElm = menuManager.confirmElm;
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
        if (Input.GetKeyDown(menuControls.backButton))
        {
            // back button stuff
            if (ID == 0)
            {

            }
            else
            {
                menuManager.playerChosenChars[ID] = null;
                hasChar = false;
            }
        }
    }
}
