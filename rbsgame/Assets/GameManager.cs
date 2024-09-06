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
    public class Character
    {
        public enum CharacterNames
        {
            Gregory, GentlemanHumanoid
        }
        public CharacterNames characterName;

        public Character(CharacterNames _characterName)
        {
            characterName = _characterName;
        }
    }

    public List<Character> characters = new List<Character>
    {
        new Character(Character.CharacterNames.Gregory),
        new Character(Character.CharacterNames.GentlemanHumanoid)
    };


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Battle");
        }
    }
}
