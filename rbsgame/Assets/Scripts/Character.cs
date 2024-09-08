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
        public AnimationClip idle;
        public AnimationClip run;

        public AnimationClip jump; // might split up into jump and jumpsquat?
        public AnimationClip fall;
        public AnimationClip land;

        public AnimationClip hitstop;
        public AnimationClip stunned;
        public AnimationClip fatalHit;

        public AnimationClip dead;
    }

    public AnimationSet animationSet;

    public bool flat;

    public Animator animator;
    public Animation anim;
    public SpriteRenderer sr;

    public Player.State playerState;

    public AnimationClip currentAnimation;

    // Start is called before the first frame update
    void Start()
    {
        if (flat) 
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).TryGetComponent<Animator>(out animator);
                transform.GetChild(i).TryGetComponent<Animation>(out anim);
                transform.GetChild(i).TryGetComponent<SpriteRenderer>(out sr);
            }
        }
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
        if (playerState.activeDirectionalInput)
        {
            // run
            anim.clip = animationSet.run;
        }
        else
        {
            // idle
            anim.clip = animationSet.idle;
        }

        currentAnimation = anim.clip;
    }

    void Handle3DAnimation()
    {

    }
}
