using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDPlayer : MonoBehaviour
{
    public int score;

    public Character character;

    public BattleController bc;

    public int ID;

    public Image hpImage;

    public List<Sprite> hpSprites;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(bc.players[ID].playerState.health > 0)
        {
            hpImage.enabled = true;
            hpImage.sprite = hpSprites[bc.players[ID].playerState.health - 1];
        }
        else
        {
            hpImage.enabled = false;
        }
    }
}
