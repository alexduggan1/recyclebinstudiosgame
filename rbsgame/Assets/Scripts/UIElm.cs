using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElm : MonoBehaviour
{
    public UIElm elmAbove = null;
    public UIElm elmBelow = null;
    public UIElm elmLeft = null;
    public UIElm elmRight = null;

    public enum PanelType
    {
        Character, Stage, ItemWeight, Confirm
    }

    public PanelType panelType;

    public Character attachedCharacter;
    public GameObject attachedStage;
    public BattleController.ItemDropLoot attachedItemDropLoot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
