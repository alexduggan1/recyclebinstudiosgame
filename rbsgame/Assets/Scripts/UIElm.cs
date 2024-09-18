using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Slider weightSlider;
    public GameObject visualItem;

    public GameObject resetVisual;

    // Start is called before the first frame update
    void Start()
    {
        if(panelType == PanelType.ItemWeight)
        {
            if(attachedItemDropLoot != null)
            {
                if(attachedItemDropLoot.loot != null)
                {
                    weightSlider.value = 1;
                }
                else
                {
                    resetVisual.SetActive(true);
                }
            }
            else
            {
                resetVisual.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (panelType == PanelType.ItemWeight)
        {
            if (attachedItemDropLoot != null)
            {
                if (attachedItemDropLoot.loot != null)
                {
                    attachedItemDropLoot.weight = weightSlider.value;
                }
            }
        }
    }
}
