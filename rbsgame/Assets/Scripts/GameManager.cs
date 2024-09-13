using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PressGoButton()
    {
        if (SceneManager.GetActiveScene().name == "Startup")
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
