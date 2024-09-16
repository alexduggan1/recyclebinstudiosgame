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
    public UIElm starterUIElm;

    public UIElm.PanelType currentPanel;

    // Start is called before the first frame update
    void Awake()
    {
        currentUIElm = starterUIElm;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(menuControls.leftButton)) {
            if (currentUIElm.elmLeft != null) { currentUIElm = currentUIElm.elmLeft; }}
        if (Input.GetKeyDown(menuControls.rightButton)) {
            if (currentUIElm.elmRight != null) { currentUIElm = currentUIElm.elmRight; }}
        if (Input.GetKeyDown(menuControls.upButton)) {
            if (currentUIElm.elmAbove != null) { currentUIElm = currentUIElm.elmAbove; }}
        if (Input.GetKeyDown(menuControls.downButton)) {
            if (currentUIElm.elmBelow != null) { currentUIElm = currentUIElm.elmBelow; }}


    }
}
