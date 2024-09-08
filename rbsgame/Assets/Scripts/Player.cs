using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public class Items
    {
        public Item LeftHand;
        public Item RightHand;
        public Item Hat;

        public Items(Item _LeftHand, Item _RightHand, Item _Hat)
        {
            LeftHand = _LeftHand;
            RightHand = _RightHand;
            Hat = _Hat;
        }
    }

    public Items items;

    public Rigidbody rb;

    // physics attributes to be set in inspector
    [System.Serializable]
    public class PhysicsAttributes
    {
        public float moveAcceleration = 20;
        public float maxMoveSpeed = 5;
        public float autoDeceleration = 0.9f;
        public float jumpForce = 10;
    }

    public PhysicsAttributes physicsAttributes;


    // current player state
    [System.Serializable]
    public class State
    {
        public bool alive;
        public enum Dir
        {
            Left, Right
        }
        public Dir facingDir;
        public bool onGround;
        public bool activeDirectionalInput;
    }

    public State playerState;


    // player's inputs
    [System.Serializable]
    public class Inputs
    {
        public float hMoveAxis;
        public bool jumpPressed;
    }

    public Inputs playerInputs;


    public class Controls
    {
        public string hMoveAxisName;
        public KeyCode jumpButton;

        public Controls(string _hMoveAxisName, KeyCode _jumpButton)
        {
            hMoveAxisName = _hMoveAxisName;
            jumpButton = _jumpButton;
        }
    }

    public Controls playerControls;


    public int ID;
    
    public Character character;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // handle player inputs
        playerInputs.hMoveAxis = Input.GetAxis(playerControls.hMoveAxisName);
        playerInputs.jumpPressed = Input.GetKey(playerControls.jumpButton);
        
        playerState.activeDirectionalInput = (Mathf.Abs(playerInputs.hMoveAxis) > 0);

        character.playerState = playerState;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // update player state
        // TODO still have to make this ray only work for things that should be able to be jumped from
        playerState.onGround = Physics.Raycast(new Ray(transform.position + (Vector3.down * 0.75f), Vector3.down), 0.5f);
        if (playerState.onGround)
        {
            if (playerInputs.hMoveAxis > 0.05f)
            {
                playerState.facingDir = State.Dir.Right;
            }
            if (playerInputs.hMoveAxis < -0.05f)
            {
                playerState.facingDir = State.Dir.Left;
            }
        }

        // handle movement

        // move side to side
        rb.velocity += new Vector3 (playerInputs.hMoveAxis * physicsAttributes.moveAcceleration * Time.fixedDeltaTime, 0);
        
        // decelerate automatically
        rb.velocity = new Vector3(rb.velocity.x * physicsAttributes.autoDeceleration, rb.velocity.y);

        // maximum horizontal speed
        //Debug.Log(rb.velocity.x);
        if ((rb.velocity.x) > physicsAttributes.maxMoveSpeed) {
            rb.velocity = new Vector3((physicsAttributes.maxMoveSpeed), rb.velocity.y);
        }
        if ((rb.velocity.x) < physicsAttributes.maxMoveSpeed * -1) {
            rb.velocity = new Vector3((physicsAttributes.maxMoveSpeed * -1), rb.velocity.y);
        }

        // jumping
        if (playerInputs.jumpPressed && playerState.onGround)
        {
            rb.velocity = new Vector3(rb.velocity.x, physicsAttributes.jumpForce);
            playerInputs.jumpPressed = false;
        }
    }
}
