using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [System.Serializable]
    public class Items
    {
        public Item LeftHand = null;
        public Item RightHand = null;
        public Item Hat = null;

        public Items(Item _LeftHand, Item _RightHand, Item _Hat)
        {
            LeftHand = _LeftHand;
            RightHand = _RightHand;
            Hat = _Hat;
        }
    }

    public Items items = new Items(null, null, null);

    public Rigidbody rb;

    // physics attributes to be set in inspector
    [System.Serializable]
    public class PhysicsAttributes
    {
        public float moveAccelerationGround = 20;
        public float moveAccelerationAir = 20;
        public float maxMoveSpeedGround = 5;
        public float maxMoveSpeedAir = 5;
        public float autoDecelerationGround = 0.9f;
        public float autoDecelerationAir = 0.9f;
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


    public List<GameObject> itemProtos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        items = new Items(null, null, null);
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
        // decelerate automatically
        // maximum horizontal speed
        if (playerState.onGround)
        {
            rb.velocity += new Vector3(playerInputs.hMoveAxis * physicsAttributes.moveAccelerationGround * Time.fixedDeltaTime, 0);
            rb.velocity = new Vector3(rb.velocity.x * physicsAttributes.autoDecelerationGround, rb.velocity.y);


            if ((rb.velocity.x) > physicsAttributes.maxMoveSpeedGround)
            {
                rb.velocity = new Vector3((physicsAttributes.maxMoveSpeedGround), rb.velocity.y);
            }
            if ((rb.velocity.x) < physicsAttributes.maxMoveSpeedGround * -1)
            {
                rb.velocity = new Vector3((physicsAttributes.maxMoveSpeedGround * -1), rb.velocity.y);
            }
        }
        else
        {
            rb.velocity += new Vector3(playerInputs.hMoveAxis * physicsAttributes.moveAccelerationAir * Time.fixedDeltaTime, 0);
            rb.velocity = new Vector3(rb.velocity.x * physicsAttributes.autoDecelerationAir, rb.velocity.y);


            if ((rb.velocity.x) > physicsAttributes.maxMoveSpeedAir)
            {
                rb.velocity = new Vector3((physicsAttributes.maxMoveSpeedAir), rb.velocity.y);
            }
            if ((rb.velocity.x) < physicsAttributes.maxMoveSpeedAir * -1)
            {
                rb.velocity = new Vector3((physicsAttributes.maxMoveSpeedAir * -1), rb.velocity.y);
            }
        }
        


        // jumping
        if (playerInputs.jumpPressed && playerState.onGround)
        {
            rb.velocity = new Vector3(rb.velocity.x, physicsAttributes.jumpForce);
            playerInputs.jumpPressed = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Enter !!!!!!!!!!!!!!!!!!!!!!!!!!!");
        Debug.Log(collision.gameObject.layer);
        if (collision.gameObject.layer == 8)
        {
            // pickup item
            Debug.Log("TOUCHED ITEM PICKUP!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

            Item itemToTry = collision.transform.GetChild(0).GetComponent<Item>();
            Debug.Log(itemToTry.itemType.name);

            bool pickupSuccessful = false;
            if(itemToTry.itemType.attachType == Item.ItemType.AttachTypes.Hat)
            {
                if (items.Hat != null) { } else
                {
                    Item newItem = Instantiate(FindCorrectItemProto(itemToTry.itemType.name), transform).GetComponent<Item>();
                    items.Hat = newItem;
                    pickupSuccessful = true;
                }
            }
            else if (itemToTry.itemType.attachType == Item.ItemType.AttachTypes.Handheld)
            {
                // for now fill lefthand first then right, can change behavior later
                if (items.LeftHand != null) {
                    if (items.RightHand != null) { } else
                    {
                        Item newItem = Instantiate(FindCorrectItemProto(itemToTry.itemType.name), transform).GetComponent<Item>();
                        items.RightHand = newItem;
                        pickupSuccessful = true;
                    }
                } 
                else
                {
                    Item newItem = Instantiate(FindCorrectItemProto(itemToTry.itemType.name), transform).GetComponent<Item>();
                    items.LeftHand = newItem;
                    pickupSuccessful = true;
                }
            }

            Debug.Log(pickupSuccessful);
            // decide on behavior later, for now destroy the object on touch
            Destroy(collision.gameObject);
        }
    }

    public GameObject FindCorrectItemProto(Item.ItemType.Names itemName)
    {
        GameObject result = itemProtos[0];

        foreach (GameObject itemProto in itemProtos)
        {
            if(itemProto.GetComponent<Item>().itemType.name == itemName)
            {
                result = itemProto;
            }
        }

        return result;
    }
}
