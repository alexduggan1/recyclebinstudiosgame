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

    public bool flat;

    public Animator animator;
    public SpriteRenderer sr;

    public Player.State playerState;

    public Transform anchorLH;
    public Transform anchorRH;
    public Transform anchorH;

    void Awake()
    {
        if (flat) 
        {
            Debug.Log("flat");

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).TryGetComponent<Animator>(out animator);
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
        //Debug.Log("EEEEEEEEEEEEEEEEEEEEEEEE");
        if (playerState.onGround)
        {
            if ((!animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")) && (playerState.jumpSquatCountdown <= 0))
            {
                if (playerState.activeDirectionalInput)
                {
                    // run
                    animator.Play("Run");
                    //Debug.Log("should be running");
                }
                else
                {
                    // idle
                    animator.Play("Idle");
                }
            }
        }
        else
        {
            if (playerState.yVel < 0)
            {
                animator.Play("Fall");
            }
        }
    }

    void Handle3DAnimation()
    {

    }

    public void Jump()
    {
        animator.Play("Jump");
    }
}
