using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }


    [SerializeField]
    public List<Character> characters = new List<Character> { };

    public List<GameObject> stages = new List<GameObject> { };

    public List<Character> playerChosenChars = new List<Character> { };

    public GameObject chosenStage;

    public List<BattleController.ItemDropLoot> chosenItemDropLoots = new List<BattleController.ItemDropLoot> { };
    public List<BattleController.ItemDropLoot> defaultItemDropLoots = new List<BattleController.ItemDropLoot> { };

    public List<MenuPlayer> listOfMenuPlayers;

    public float titleTimer;

    public MenuManager menuManager;

    public int winnerID;

    public EndManager endManager;


    public List<Sprite> controllerDiagrams;

    public GameObject oneSoundPlayer;

    // Start is called before the first frame update
    void Start()
    {
        defaultItemDropLoots.Clear();
        foreach (BattleController.ItemDropLoot idl in chosenItemDropLoots)
        {
            BattleController.ItemDropLoot nidl = new BattleController.ItemDropLoot();
            nidl.loot = idl.loot;
            nidl.weight = idl.weight;
            defaultItemDropLoots.Add(nidl);
        }

        titleTimer = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if(titleTimer > 0)
        {
            titleTimer -= Time.deltaTime;
            if (GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Stop();
            }
        }
        if (SceneManager.GetActiveScene().name == "Startup")
        {
            if (Input.anyKeyDown && titleTimer <= 0) { PressGoButton(); }
        }
        else if (SceneManager.GetActiveScene().name == "Menu")
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }
        }
        else if (SceneManager.GetActiveScene().name == "Battle")
        {
            if (GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Stop();
            }
        }
        else if (SceneManager.GetActiveScene().name == "End")
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }
            if (endManager != null)
            {
                endManager.gameManager = this;
                endManager.winnerID = winnerID;
            }
            else
            {
                endManager = FindObjectOfType<EndManager>();
            }
        }
    }

    public void PressGoButton()
    {
        if (SceneManager.GetActiveScene().name == "Startup")
        {
            SceneManager.LoadScene("Menu");
        }
    }

    public void SendLeftOption(int idx)
    {
        if (SceneManager.GetActiveScene().name == "End")
        {
            if (endManager != null)
            {
                endManager.ReceiveLeftOption(idx);
            }
        }
    }

    public void SendRightOption(int idx)
    {
        if (SceneManager.GetActiveScene().name == "End")
        {
            if (endManager != null)
            {
                endManager.ReceiveRightOption(idx);
            }
        }
    }
}
