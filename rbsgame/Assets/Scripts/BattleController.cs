using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public Camera cam;


    public List<GameObject> stages;
    public GameObject stage;
    public GameObject stageProto;
    
    public List<Player> players;
    public GameObject playerProto;


    public List<GameManager.Character> playerChosenCharacters;


    public Player.Controls leftPlayerControlsSignature = new Player.Controls("AD", KeyCode.W);
    public Player.Controls rightPlayerControlsSignature = new Player.Controls("Arrows", KeyCode.UpArrow);
    public Player.Controls middlePlayerControlsSignature = new Player.Controls("JL", KeyCode.I);
    public Player.Controls middlePlayerControlsSignature2 = new Player.Controls("FH", KeyCode.T);

    // Start is called before the first frame update
    void Start()
    {
        

        BeginBattle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BeginBattle()
    {
        // load in the stage, then load in the players

        stage = Instantiate(stageProto);


        int playerCount = playerChosenCharacters.Count;
        players.Clear();
        int i = 0;
        foreach (GameManager.Character playerChosenCharacter in playerChosenCharacters)
        {
            Player newPlayer = Instantiate(playerProto).GetComponent<Player>();

            newPlayer.character = playerChosenCharacter;
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
