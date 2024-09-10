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
        if (playerState.alive)
        {
            if (!playerState.activelyUsingItem)
            {
                if (playerState.onGround)
                {
                    Debug.Log("grounded");
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
                    else
                    {
                        animator.Play("AirRise");
                    }
                }
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

    public float ItemAnimation(Item.ItemType.AnimType animType, Player.State.Dir facingDir, string attachment)
    {
        float result = 0;

        string clipName = "";
        if(animType == Item.ItemType.AnimType.Shoot)
        {
            if (attachment == "LH") {
                if (facingDir == Player.State.Dir.Right)
                {
                    clipName = "ShootBack";
                }
                else
                {
                    clipName = "ShootFront";
                }
            }
            if (attachment == "RH")
            {
                if (facingDir == Player.State.Dir.Right)
                {
                    clipName = "ShootFront";
                }
                else
                {
                    clipName = "ShootBack";
                }
            }
        }
        if (animType == Item.ItemType.AnimType.RegSwing)
        {
            clipName = "ShootFront";
        }
        if (animType == Item.ItemType.AnimType.OverheadSwing)
        {
            clipName = "ShootFront";
        }
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == clipName)
            {

                result = ac.animationClips[i].length;
            }
        }


        animator.Play(clipName);
        Debug.Log("animation length: " + result);
        return (result);
    }
}
