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
        public float jumpSquatTime = 0.05f;
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
        public float yVel;
        public float jumpSquatCountdown;
        public bool activelyUsingItem;
        public float itemAnimTime;
        public float itemActionTime;
    }

    public State playerState;


    // player's inputs
    [System.Serializable]
    public class Inputs
    {
        public float hMoveAxis;
        public bool jumpPressed;

        public bool useLHand;
        public bool useRHand;
        public bool useHat;
    }

    public Inputs playerInputs;


    public class Controls
    {
        public string hMoveAxisName;
        public KeyCode jumpButton;

        public KeyCode useLHandButton;
        public KeyCode useHatButton;
        public KeyCode useRHandButton;

        public Controls(string _hMoveAxisName, KeyCode _jumpButton, KeyCode _useLHandButton, KeyCode _useHatButton, KeyCode _useRHandButton)
        {
            hMoveAxisName = _hMoveAxisName;
            jumpButton = _jumpButton;
            useLHandButton = _useLHandButton;
            useHatButton = _useHatButton;
            useRHandButton = _useRHandButton;
        }
    }

    public Controls playerControls;


    public int ID;
    
    public Character character;


    public List<GameObject> itemProtos;


    public LayerMask stageLayer;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // handle player inputs
        playerInputs.hMoveAxis = Input.GetAxis(playerControls.hMoveAxisName);
        playerInputs.jumpPressed = Input.GetKey(playerControls.jumpButton);
        playerInputs.useLHand = Input.GetKey(playerControls.useLHandButton);
        playerInputs.useRHand = Input.GetKey(playerControls.useRHandButton);
        playerInputs.useHat = Input.GetKey(playerControls.useHatButton);

        playerState.activeDirectionalInput = (Mathf.Abs(playerInputs.hMoveAxis) > 0);
        playerState.yVel = rb.velocity.y;

        playerState.itemActionTime += Time.deltaTime;

        // debug thing
        //character.playerState.activeDirectionalInput = true;

        
        character.playerState = playerState;



        if (playerState.facingDir == Player.State.Dir.Left) { transform.localEulerAngles = new Vector3(0, 180, 0); }
        else { transform.localEulerAngles = new Vector3(0, 0, 0); }

        #region item placement in anchors
        // put the equipment in the correct positions based on the anchors
        if (items.LeftHand != null) {
            if(playerState.facingDir == State.Dir.Right)
            {
                items.LeftHand.transform.eulerAngles = character.anchorLH.eulerAngles - items.LeftHand.anchorOffset.localEulerAngles;
                items.LeftHand.transform.localPosition = character.anchorLH.localPosition + new Vector3
                    (-1 * (items.LeftHand.anchorOffset.position - items.LeftHand.transform.position).x,
                    (-1 * (items.LeftHand.anchorOffset.position - items.LeftHand.transform.position)).y,
                    0);
            }
            else
            {
                items.LeftHand.transform.eulerAngles = character.anchorRH.eulerAngles - items.LeftHand.anchorOffset.localEulerAngles;
                items.LeftHand.transform.localPosition = character.anchorRH.localPosition + new Vector3
                    (1 * (items.LeftHand.anchorOffset.position - items.LeftHand.transform.position).x,
                    (-1 * (items.LeftHand.anchorOffset.position - items.LeftHand.transform.position)).y,
                    0);
            }
        }
        if (items.LeftHand != null)
        {
            if (playerState.facingDir == State.Dir.Right)
            {
                items.LeftHand.transform.eulerAngles = character.anchorLH.eulerAngles - items.LeftHand.anchorOffset.localEulerAngles;
                items.LeftHand.transform.localPosition = character.anchorLH.localPosition + new Vector3
                    (-1 * (items.LeftHand.anchorOffset.position - items.LeftHand.transform.position).x,
                    (-1 * (items.LeftHand.anchorOffset.position - items.LeftHand.transform.position)).y,
                    0);
            }
            else
            {
                items.LeftHand.transform.eulerAngles = character.anchorRH.eulerAngles - items.LeftHand.anchorOffset.localEulerAngles;
                items.LeftHand.transform.localPosition = character.anchorRH.localPosition + new Vector3
                    (1 * (items.LeftHand.anchorOffset.position - items.LeftHand.transform.position).x,
                    (-1 * (items.LeftHand.anchorOffset.position - items.LeftHand.transform.position)).y,
                    0);
            }
        }
        if (items.RightHand != null)
        {
            if (playerState.facingDir == State.Dir.Right)
            {
                items.RightHand.transform.eulerAngles = character.anchorRH.eulerAngles - items.RightHand.anchorOffset.localEulerAngles;
                items.RightHand.transform.localPosition = character.anchorRH.localPosition + new Vector3
                    (-1 * (items.RightHand.anchorOffset.position - items.RightHand.transform.position).x,
                    (-1 * (items.RightHand.anchorOffset.position - items.RightHand.transform.position)).y,
                    0);
            }
            else
            {
                items.RightHand.transform.eulerAngles = character.anchorLH.eulerAngles - items.RightHand.anchorOffset.localEulerAngles;
                items.RightHand.transform.localPosition = character.anchorLH.localPosition + new Vector3
                    (1 * (items.RightHand.anchorOffset.position - items.RightHand.transform.position).x,
                    (-1 * (items.RightHand.anchorOffset.position - items.RightHand.transform.position)).y,
                    0);
            }
        }
        if (items.Hat != null)
        {
            if (playerState.facingDir == State.Dir.Right)
            {
                items.Hat.transform.eulerAngles = character.anchorH.eulerAngles - items.Hat.anchorOffset.localEulerAngles;
                items.Hat.transform.localPosition = character.anchorH.localPosition + new Vector3
                    (-1 * (items.Hat.anchorOffset.position - items.Hat.transform.position).x,
                    (-1 * (items.Hat.anchorOffset.position - items.Hat.transform.position)).y,
                    0);
            }
            else
            {
                items.Hat.transform.eulerAngles = character.anchorH.eulerAngles - items.Hat.anchorOffset.localEulerAngles;
                items.Hat.transform.localPosition = character.anchorH.localPosition + new Vector3
                    (1 * (items.Hat.anchorOffset.position - items.Hat.transform.position).x,
                    (-1 * (items.Hat.anchorOffset.position - items.Hat.transform.position)).y,
                    0);
            }
        }
        #endregion

        if (!playerState.activelyUsingItem)
        {
            if (playerInputs.useLHand) {
                if (items.LeftHand != null) {
                    UseItem(items.LeftHand, "LH");
                }
            }
            if (playerInputs.useRHand)
            {
                if (items.RightHand != null)
                {
                    UseItem(items.RightHand, "RH");
                }
            }
            if (playerInputs.useHat)
            {
                if (items.Hat != null)
                {
                    UseItem(items.Hat, "H");
                }
            }
        }
        else
        {
            playerState.itemAnimTime -= Time.deltaTime;
            if (playerState.itemAnimTime <= 0)
            {
                playerState.activelyUsingItem = false;
            }
        }
    }

    public void UseItem(Item item, string attachment)
    {
        playerState.activelyUsingItem = true; // set this back at the end
        Debug.Log("USE ITEM: " + item.itemType.name);

        if(item.itemType.attachType == Item.ItemType.AttachTypes.Hat)
        {
            // hat items

        }
        else
        {
            // handheld items
            playerState.itemAnimTime = character.ItemAnimation(item.itemType.animType, playerState.facingDir, attachment);
            item.actionTime = 0.0f;
            item.currentlyBeingUsed = true;
            item.currentUse += 1;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // update player state
        // TODO still have to make this ray only work for things that should be able to be jumped from
        playerState.onGround = Physics.Raycast(new Ray(transform.position + (Vector3.down * 0.75f), Vector3.down), 0.5f, stageLayer.value);
        if (!playerState.activelyUsingItem)
        {
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
        if (playerState.jumpSquatCountdown > 0)
        {
            playerState.jumpSquatCountdown -= Time.fixedDeltaTime;
            playerInputs.jumpPressed = false;
            if (playerState.jumpSquatCountdown <= 0)
            {
                playerInputs.jumpPressed = false;
                rb.velocity = new Vector3(rb.velocity.x, physicsAttributes.jumpForce);
            }
        }
        if (playerInputs.jumpPressed && playerState.onGround)
        {
            playerInputs.jumpPressed = false;
            character.Jump();
            playerState.jumpSquatCountdown = physicsAttributes.jumpSquatTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Enter !!!!!!!!!!!!!!!!!!!!!!!!!!!");
        Debug.Log(collision.gameObject.layer);
        if (collision.gameObject.layer == 8)
        {
            // pickup item
            //Debug.Log("TOUCHED ITEM PICKUP!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

            Item itemToTry = collision.transform.GetChild(0).GetComponent<Item>();
            Debug.Log(itemToTry.itemType.name);

            bool pickupSuccessful = false;
            if(itemToTry.itemType.attachType == Item.ItemType.AttachTypes.Hat)
            {
                if (items.Hat != null) { } else
                {
                    Item newItem = Instantiate(FindCorrectItemProto(itemToTry.itemType.name), transform).GetComponent<Item>();
                    items.Hat = newItem;
                    newItem.pickedUp = true;
                    newItem.currentlyBeingUsed = false;
                    newItem.myPlayer = this;
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
                        newItem.pickedUp = true;
                        newItem.currentlyBeingUsed = false;
                        newItem.myPlayer = this;
                        pickupSuccessful = true;
                    }
                } 
                else
                {
                    Item newItem = Instantiate(FindCorrectItemProto(itemToTry.itemType.name), transform).GetComponent<Item>();
                    items.LeftHand = newItem;
                    newItem.pickedUp = true;
                    newItem.currentlyBeingUsed = false;
                    newItem.myPlayer = this;
                    pickupSuccessful = true;
                }
            }

            //Debug.Log(pickupSuccessful);
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
