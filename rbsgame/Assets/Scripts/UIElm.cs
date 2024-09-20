using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public GameObject otherVisual;
    public enum IWButtonTypes
    {
        Confirm, Reset
    }
    public IWButtonTypes iWButtonType;

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
                    otherVisual.SetActive(true);
                    if(iWButtonType == IWButtonTypes.Confirm)
                    {
                        otherVisual.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Confirm";
                    }
                    else
                    {
                        otherVisual.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Reset";
                    }
                }
            }
            else
            {
                otherVisual.SetActive(true); 
                if (iWButtonType == IWButtonTypes.Confirm)
                {
                    otherVisual.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Confirm";
                }
                else
                {
                    otherVisual.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Reset";
                }
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
