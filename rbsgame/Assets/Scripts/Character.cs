using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character : MonoBehaviour
{
    public enum CharacterNames
    {
        Gregory, Kneecaps, Wilson, Luna, BarryBones
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

    public Player.State playerState;

    public Transform anchorLH;
    public Transform anchorRH;
    public Transform anchorH;

    public Sprite render;
    void Awake()
    {
        if (flat) 
        {
            //Debug.Log("flat");

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).TryGetComponent<Animator>(out animator);
                
            }
        }
        else {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).TryGetComponent<Animator>(out animator);
            }
        } // do something for the 3d ones idk it'll be different somehow I'm sure
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
            if (playerState.hasControl)
            {
                if (!playerState.activelyUsingItem)
                {
                    if (playerState.hitstunTime <= 0)
                    {
                        animator.speed = 1;
                        if (playerState.onGround)
                        {
                            //Debug.Log("grounded");
                            if ((!animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")) && (playerState.jumpSquatCountdown <= 0))
                            {
                                if (playerState.activeDirectionalInput)
                                {
                                    // run
                                    animator.Play("Run");
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
                    else
                    {
                        animator.Play("Hurt");
                    }
                }
            }
            else
            {
                animator.Play("Idle");
            }
        }
        else
        {
            animator.Play("Dead");
        }
    }

    void Handle3DAnimation()
    {
        if (playerState.alive)
        {
            if (!playerState.activelyUsingItem)
            {
                if (playerState.hitstunTime <= 0)
                {
                    animator.speed = 1;
                    if (playerState.onGround)
                    {
                        //Debug.Log("grounded");
                        if ((!animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")) && (playerState.jumpSquatCountdown <= 0))
                        {
                            if (playerState.activeDirectionalInput)
                            {
                                // run
                                animator.Play("Run");
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
                else
                {
                    animator.Play("Hurt");
                }
            }
        }
        else
        {
            animator.Play("Dead");
        }
    }

    public void Jump()
    {
        animator.Play("Jump");
    }

    public float ItemAnimation(Item theItem, Player.State.Dir facingDir, string attachment)
    {
        float result = 0;

        Item.ItemType.AnimType animType = theItem.itemType.animType;

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
        if (animType == Item.ItemType.AnimType.OverheadSwing)
        {
            if (attachment == "LH")
            {
                if (facingDir == Player.State.Dir.Right)
                {
                    clipName = "OverheadBack";
                }
                else
                {
                    clipName = "OverheadFront";
                }
            }
            if (attachment == "RH")
            {
                if (facingDir == Player.State.Dir.Right)
                {
                    clipName = "OverheadFront";
                }
                else
                {
                    clipName = "OverheadBack";
                }
            }
        }

        //Debug.Log(animator.speed);
        //Debug.Log("should be:   " + theItem.handheldAnimSpeed.ToString());
        animator.speed = theItem.handheldAnimSpeed;

        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == clipName)
            {
                result = ac.animationClips[i].length / theItem.handheldAnimSpeed;
            }
        }


        animator.Play(clipName);
        //Debug.Log("animation length: " + result);
        return (result);
    }
}
