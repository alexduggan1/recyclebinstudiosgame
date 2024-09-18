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
    public Image visualItem;

    public GameObject resetVisual;

    public Sprite stageQuestionMarkThumb;

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
                    if(attachedItemDropLoot.loot.thumbnail != null)
                    {
                        visualItem.sprite = attachedItemDropLoot.loot.thumbnail;
                    }
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
        else if(panelType == PanelType.Stage)
        {
            if(attachedStage != null)
            {
                Stage stageObj;
                if (attachedStage.TryGetComponent<Stage>(out stageObj))
                {
                    if (stageObj.thumbnail != null)
                    {
                        visualItem.sprite = stageObj.thumbnail;
                    }
                }
            }
            else
            {
                visualItem.sprite = stageQuestionMarkThumb;
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
