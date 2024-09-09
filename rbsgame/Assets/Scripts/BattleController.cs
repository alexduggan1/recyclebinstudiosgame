using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public Camera cam;


    public List<GameObject> stages;
    public GameObject stage;
    public GameObject stageProto;

    public float stageWidth;
    
    public List<Player> players;
    public GameObject playerProto;


    public List<Character> playerChosenCharacters;


    public Player.Controls leftPlayerControlsSignature = new Player.Controls("AD", KeyCode.W);
    public Player.Controls rightPlayerControlsSignature = new Player.Controls("Arrows", KeyCode.UpArrow);
    public Player.Controls middlePlayerControlsSignature = new Player.Controls("JL", KeyCode.I);
    public Player.Controls middlePlayerControlsSignature2 = new Player.Controls("FH", KeyCode.T);

    public float itemSpawnGap;
    public float itemSpawnTimer;
    public int itemsExisting;

    public float battleTimer;

    // Start is called before the first frame update
    void Start()
    {
        BeginBattle();
    }

    // Update is called once per frame
    void Update()
    {
        itemsExisting = FindObjectsByType<Item>(FindObjectsSortMode.None).Length;

        battleTimer += Time.deltaTime;
        itemSpawnTimer += Time.deltaTime;

        
        if (battleTimer < 7.5f) { itemSpawnGap = 3; }
        else
        {
            if (itemsExisting < players.Count) { itemSpawnGap = 7.5f; }
            else if (itemsExisting < players.Count * 2) { itemSpawnGap = 13; }
            else { itemSpawnGap = 0; }
        }


        if(itemSpawnGap != 0)
        {
            if(itemSpawnTimer > itemSpawnGap)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    SpawnItem();
                }
                itemSpawnTimer = 0;
            }
        }
    }

    void SpawnItem()
    {
        Debug.Log(stageWidth);

        // TODO
        // send raycasts down from sky within stage width and check if it hits something, if so drop an item there
        // make itempickup prefab to spawn which contains a reference to the item it should spawn
        // reference loot table for which item it is
    }

    void BeginBattle()
    {
        // load in the stage, then load in the players

        stage = Instantiate(stageProto);
        stageWidth = Vector3.Scale(stage.GetComponent<Stage>().collision.GetComponent<MeshFilter>().mesh.bounds.extents, stage.GetComponent<Stage>().collision.transform.localScale * 2).x;

        int playerCount = playerChosenCharacters.Count;
        players.Clear();
        int i = 0;
        foreach (Character playerChosenCharacter in playerChosenCharacters)
        {
            Player newPlayer = Instantiate(playerProto).GetComponent<Player>();

            GameObject newChar = Instantiate(playerChosenCharacter.gameObject, newPlayer.transform);
            newPlayer.character = newChar.GetComponent<Character>();
            newPlayer.ID = i;

            newPlayer.transform.position = FigureOutPlayerStartPosition(newPlayer.ID, playerCount);

            if(newPlayer.ID == 0) { newPlayer.playerControls = leftPlayerControlsSignature; }
            if (newPlayer.ID == 1)
            {
                if(playerCount == 2) { newPlayer.playerControls = rightPlayerControlsSignature; }
                else if (playerCount == 3) { newPlayer.playerControls = rightPlayerControlsSignature; }
                else { newPlayer.playerControls = middlePlayerControlsSignature2; }
            }
            if (newPlayer.ID == 2)
            {
                if (playerCount == 3) { newPlayer.playerControls = middlePlayerControlsSignature; }
                else { newPlayer.playerControls = middlePlayerControlsSignature; }
            }
            if (newPlayer.ID == 3) { newPlayer.playerControls = rightPlayerControlsSignature; }

            players.Add(newPlayer);
            i += 1;
        }

        // init item system

        battleTimer = 0;
        itemSpawnTimer = 0;
        itemSpawnGap = 2;

        // spawn an item for each player
        for (int j = 0; j < playerChosenCharacters.Count; j++)
        {
            SpawnItem();
        }

    }

    Vector3 FigureOutPlayerStartPosition(int playerID, int playerCount)
    {
        Vector3 result = Vector3.zero;

        List<Stage.SpawnPoint> spawnPoints = stage.GetComponent<Stage>().spawnPoints;

        foreach (Stage.SpawnPoint spawnPoint in spawnPoints)
        {
            if(playerCount == 2)
            {
                if(spawnPoint.IDFor2Player == playerID) { result = spawnPoint.shortcut.transform.position; }
            }
            else if (playerCount == 3)
            {
                if (spawnPoint.IDFor3Player == playerID) { result = spawnPoint.shortcut.transform.position; }
            }
            else if (playerCount == 4)
            {
                if (spawnPoint.IDFor4Player == playerID) { result = spawnPoint.shortcut.transform.position; }
            }
        }

        Debug.Log(result);

        return (result);
    }
    
    void BeginRound()
    {
        // reset all the player's info
        // should it change the stage too? idk
        // items should be reset too
    }
}
