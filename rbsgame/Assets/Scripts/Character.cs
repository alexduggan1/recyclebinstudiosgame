using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character : MonoBehaviour
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

    [System.Serializable]
    public class AnimationSet
    {
        public Animation idle;
        public Animation walk;

        public AnimationSet(Animation _idle, Animation _walk)
        {
            idle = _idle;
            walk = _walk;
        }
    }

    public AnimationSet animationSet;

    public bool flat;

    public Animator animator;

    public Player.State playerState;

    // Start is called before the first frame update
    void Start()
    {
        if (flat) { animator = transform.GetChild(0).GetComponent<Animator>(); }
        else { } // do something for the 3d ones idk it'll be different somehow I'm sure
    }

    // Update is called once per frame
    void Update()
    {
        if (flat) { Handle2DAnimation(); }
        else { Handle3DAnimation(); }
    }

    void Handle2DAnimation()
    {

    }

    void Handle3DAnimation()
    {

    }
}
