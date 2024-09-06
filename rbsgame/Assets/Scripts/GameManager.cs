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


    // important stuff to access
    [System.Serializable]
    public class Character
    {
        public enum CharacterNames
        {
            Gregory, GentlemanHumanoid
        }
        public CharacterNames characterName;

        public GameObject characterProto;

        public Character(CharacterNames _characterName, GameObject _characterProto)
        {
            characterName = _characterName;
            characterProto = _characterProto;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(SceneManager.GetActiveScene().name == "Startup")
            {
                SceneManager.LoadScene("Battle");
            }
        }
    }
}
