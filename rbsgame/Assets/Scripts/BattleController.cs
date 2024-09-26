using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleController : MonoBehaviour
{
    public GameManager gameManager;


    public Camera cam;
    public List<Transform> objsToTrackCam = new List<Transform> { };
    public Vector3 camOffset;
    private Vector3 camVelocity;
    public float camMinZoom = 80f; 
    public float camMaxZoom = 40f;
    public float camZoomLimiter = 50f;

    public bool cameraMove;

    public List<GameObject> stages;
    public GameObject stage;
    public GameObject stageProto;

    public float stageWidth;
    
    public List<Player> players;
    public GameObject playerProto;


    public List<Character.CharacterNames> playerChosenCharacters;


    public Player.Controls leftPlayerControlsSignature = new Player.Controls("AD", KeyCode.W, KeyCode.Z, KeyCode.S, KeyCode.X, KeyCode.Q);
    public Player.Controls rightPlayerControlsSignature = new Player.Controls("Arrows", KeyCode.UpArrow, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Q);
    public Player.Controls middlePlayerControlsSignature = new Player.Controls("JL", KeyCode.I, KeyCode.M, KeyCode.K, KeyCode.Comma, KeyCode.Q);
    public Player.Controls middlePlayerControlsSignature2 = new Player.Controls("FH", KeyCode.T, KeyCode.V, KeyCode.G, KeyCode.B, KeyCode.Q);

    public float itemSpawnGap;
    public float itemSpawnTimer;
    public int itemsExisting;


    public GameObject itemPickupProto;

    [System.Serializable]
    public class ItemDropLoot
    {
        public Item loot;
        public float weight = 1;
    }
    public List<ItemDropLoot> itemDropLootTable;

    public float battleTimer;

    public LayerMask stageLayer;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        playerChosenCharacters.Clear();
        foreach (Character chara in gameManager.playerChosenChars)
        {
            playerChosenCharacters.Add(chara.characterName);
        }
        stageProto = gameManager.chosenStage;
        itemDropLootTable = gameManager.chosenItemDropLoots;

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

        

        if(players.Count > 0)
        {
            int alivePlayers = 0;
            foreach (Player player in players)
            {
                if (player.playerState.alive) { alivePlayers++; }
            }

            if(alivePlayers <= 1)
            {
                EndRound();
                StartRound();
            }
        }
    }

    void LateUpdate()
    {
        if (cameraMove)
        {
            Vector3 centerPoint = GetCenterPoint();

            Vector3 newPosition = centerPoint + camOffset;

            cam.transform.position = Vector3.SmoothDamp(cam.transform.position, newPosition, ref camVelocity, 0.5f);

            float newZoom = Mathf.Lerp(camMaxZoom, camMinZoom, GetGreatestDistance() / camZoomLimiter);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
        }
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(objsToTrackCam[0].position, Vector3.zero);
        for (int i = 0; i < objsToTrackCam.Count; i++)
        {
            bounds.Encapsulate(objsToTrackCam[i].position);
        }

        return bounds.size.x;
    }

    Vector3 GetCenterPoint()
    {
        objsToTrackCam.Clear();
        objsToTrackCam.Add(stage.transform);
        if (players.Count > 0)
        {
            foreach (Player player in players)
            {
                if (player.playerState.alive && player.transform.position.y > -5) { objsToTrackCam.Add(player.transform); }
            }
        }

        if (objsToTrackCam.Count == 1)
        {
            return objsToTrackCam[0].position;
        }

        var bounds = new Bounds(objsToTrackCam[0].position, Vector3.zero);
        for (int i = 0; i < objsToTrackCam.Count; i++)
        {
            bounds.Encapsulate(objsToTrackCam[i].position);
        }

        return bounds.center;
    }

    void SpawnItem()
    {
        //Debug.Log(stageWidth);


        // send raycasts down from sky within stage width and check if it hits something, if so drop an item there
        // make itempickup prefab to spawn which contains a reference to the item it should spawn? or perhaps just instantiate the item itself but with a 
        // flag that says its not been picked up yet? not super sure what would work best...
        // reference loot table for which item it is

        float newItemXPos = 0;

        int i = 0;
        while(i < 5) // we give this 5 tries
        {
            Vector3 pos = new Vector3(Random.Range(-stageWidth, stageWidth), stage.GetComponent<Stage>().ceiling.position.y - 1.2f, 0); 
            //Debug.Log(pos.x);
            
            RaycastHit rayHit;
            if(Physics.Raycast(new Ray(pos, Vector3.down), out rayHit, 100.0f, stageLayer.value))
            {
                newItemXPos = pos.x;
                i = 7; // break the while
            }
            i++;
        }

        if(i >= 7)
        {
            // actually spawn the item here at newItemXPos

            float totalWeight = 0;

            foreach (ItemDropLoot itemDrop in itemDropLootTable)
            {
                totalWeight += itemDrop.weight;
            }

            if (totalWeight > 0)
            {
                float randomWeightedSelection = Random.Range(0.0f, (float)totalWeight);
                //Debug.Log(randomWeightedSelection);

                float bypassedWeight = 0;
                Item selectedItem = null;

                foreach (ItemDropLoot itemDrop in itemDropLootTable)
                {
                    if (selectedItem == null)
                    {
                        if (randomWeightedSelection >= bypassedWeight && randomWeightedSelection <= (bypassedWeight + itemDrop.weight))
                        {
                            selectedItem = itemDrop.loot;
                        }

                        bypassedWeight += itemDrop.weight;
                    }
                }

                //Debug.Log("MADE ITEM!!!!!!!!!!!!!!!!!");

                GameObject itemPickup = Instantiate(itemPickupProto, position: new Vector3(newItemXPos, stage.GetComponent<Stage>().ceiling.position.y - 1.2f, 0), Quaternion.identity);
                Item madeItem = Instantiate(selectedItem.gameObject, itemPickup.transform).GetComponent<Item>();

                itemPickup.GetComponent<ItemPickup>().item = madeItem;
                madeItem.pickedUp = false;
            }
        }
    }

    void BeginBattle()
    {
        // load in the stage, then load in the players

        stage = Instantiate(stageProto);

        // find largest extents
        Vector3 largestExtents = Vector3.zero;
        foreach (MeshFilter mesh in stage.GetComponentsInChildren<MeshFilter>())
        {
            if(mesh.gameObject.layer != 10) // don't count the killboxes
            {
                if (mesh.mesh.bounds.extents.x * mesh.transform.lossyScale.x > largestExtents.x && mesh.gameObject.layer == 7)
                {
                    largestExtents = mesh.mesh.bounds.extents * mesh.transform.lossyScale.x;
                }
            }
        }
        //Debug.Log("largestExtents: " + largestExtents);
        stageWidth = largestExtents.x;



        // begin round?

        StartRound();
    }

    public void StartRound()
    {
        int playerCount = playerChosenCharacters.Count;
        players.Clear();
        int i = 0;
        foreach (Character.CharacterNames playerChosenCharacter in playerChosenCharacters)
        {
            Player newPlayer = Instantiate(playerProto).GetComponent<Player>();

            Character charaProto = gameManager.characters[0];
            foreach (Character chara in gameManager.characters)
            {
                if(chara.characterName == playerChosenCharacter) { charaProto = chara; }
            }

            GameObject newChar = Instantiate(charaProto.gameObject, newPlayer.transform);
            newPlayer.character = newChar.GetComponent<Character>();
            newPlayer.ID = i;

            newPlayer.transform.position = FigureOutPlayerStartPosition(newPlayer.ID, playerCount);

            if (newPlayer.ID == 0) { newPlayer.playerControls = leftPlayerControlsSignature; }
            if (newPlayer.ID == 1)
            {
                if (playerCount == 2) { newPlayer.playerControls = rightPlayerControlsSignature; }
                else if (playerCount == 3) { newPlayer.playerControls = rightPlayerControlsSignature; }
                else { newPlayer.playerControls = middlePlayerControlsSignature2; }
            }
            if (newPlayer.ID == 2)
            {
                if (playerCount == 3) { newPlayer.playerControls = middlePlayerControlsSignature; }
                else { newPlayer.playerControls = middlePlayerControlsSignature; }
            }
            if (newPlayer.ID == 3) { newPlayer.playerControls = rightPlayerControlsSignature; }

            //gameManager.listOfMenuPlayers[i].GetComponent<PlayerInput>().currentActionMap = gameManager.listOfMenuPlayers[i].GetComponent<PlayerInput>().actions.FindActionMap("Ingame");
            gameManager.listOfMenuPlayers[i].myPlayer = newPlayer;

            // do this bit start of every round

            newPlayer.playerState.alive = true;

            newPlayer.items = new Player.Items(null, null, null);
            newPlayer.playerState.activelyUsingItem = false;

            if (newPlayer.transform.position.x > 0) { newPlayer.playerState.facingDir = Player.State.Dir.Left; }
            else { newPlayer.playerState.facingDir = Player.State.Dir.Right; }

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

    public void EndRound()
    {
        // destroy old stuff


        foreach (Bullet bullet in FindObjectsByType<Bullet>(FindObjectsSortMode.None))
        {
            Destroy(bullet.gameObject);
        }
        foreach (Toast toast in FindObjectsByType<Toast>(FindObjectsSortMode.None))
        {
            Destroy(toast.gameObject);
        }
        foreach (Hitbox hitbox in FindObjectsByType<Hitbox>(FindObjectsSortMode.None))
        {
            Destroy(hitbox.gameObject);
        }
        foreach (Bananarang bananarang in FindObjectsByType<Bananarang>(FindObjectsSortMode.None))
        {
            Destroy(bananarang.gameObject);
        }
        foreach (TopHatPortal topHatPortal in FindObjectsByType<TopHatPortal>(FindObjectsSortMode.None))
        {
            Destroy(topHatPortal.gameObject);
        }



        foreach (Player player in FindObjectsByType<Player>(FindObjectsSortMode.None))
        {
            Destroy(player.gameObject);
        }


        foreach (ItemPickup itemPickup in FindObjectsByType<ItemPickup>(FindObjectsSortMode.None))
        {
            Destroy(itemPickup.gameObject);
        }

        foreach (Item item in FindObjectsByType<Item>(FindObjectsSortMode.None))
        {
            Destroy(item.gameObject);
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

        //Debug.Log(result);

        return (result);
    }
}
