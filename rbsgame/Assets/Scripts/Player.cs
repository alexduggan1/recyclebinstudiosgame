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
        public float jumpPressed;
        public bool dropPressed;

        public float useLHand;
        public float useRHand;
        public float useHat;
    }

    public Inputs playerInputs;


    public class Controls
    {
        public string hMoveAxisName;
        public KeyCode jumpButton;

        public KeyCode useLHandButton;
        public KeyCode useHatButton;
        public KeyCode useRHandButton;
        public KeyCode dropButton;

        public Controls(string _hMoveAxisName, KeyCode _jumpButton, KeyCode _useLHandButton, KeyCode _useHatButton, KeyCode _useRHandButton, KeyCode _dropButton)
        {
            hMoveAxisName = _hMoveAxisName;
            jumpButton = _jumpButton;
            useLHandButton = _useLHandButton;
            useHatButton = _useHatButton;
            useRHandButton = _useRHandButton;
            dropButton = _dropButton;
        }
    }

    public Controls playerControls;


    public int ID;
    
    public Character character;


    public LayerMask stageLayer;


    public List<GameObject> myProjectiles;

    public BattleController battleController;

    public float pickupDelay;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        myProjectiles.Clear();

        battleController = FindAnyObjectByType<BattleController>();
    }

    void Update()
    {
        if (playerState.alive)
        {
            // handle player inputs
            //playerInputs.hMoveAxis = Input.GetAxis(playerControls.hMoveAxisName);
            //playerInputs.jumpPressed = Input.GetKey(playerControls.jumpButton);
            //playerInputs.useLHand = Input.GetKeyDown(playerControls.useLHandButton);
            //playerInputs.useRHand = Input.GetKeyDown(playerControls.useRHandButton);
            //playerInputs.useHat = Input.GetKeyDown(playerControls.useHatButton);
            //playerInputs.dropPressed = Input.GetKey(playerControls.dropButton);

            playerInputs.jumpPressed -= Time.deltaTime;
            playerInputs.useLHand -= Time.deltaTime;
            playerInputs.useRHand -= Time.deltaTime;
            playerInputs.useHat -= Time.deltaTime;


            playerState.activeDirectionalInput = (Mathf.Abs(playerInputs.hMoveAxis) > 0);
            playerState.yVel = rb.velocity.y;

            playerState.itemActionTime += Time.deltaTime;
            pickupDelay -= Time.deltaTime;


            character.playerState = playerState;



            if (playerState.facingDir == State.Dir.Left) { transform.localEulerAngles = new Vector3(0, 180, 0); }
            else { transform.localEulerAngles = new Vector3(0, 0, 0); }

            #region item placement in anchors
            // put the equipment in the correct positions based on the anchors
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
                    if (playerState.activelyUsingItem)
                    {
                        items.LeftHand.transform.eulerAngles = character.anchorRH.eulerAngles - items.LeftHand.anchorOffset.localEulerAngles;
                        items.LeftHand.transform.localPosition = character.anchorRH.localPosition + new Vector3
                            (1 * (items.LeftHand.anchorOffset.position - items.LeftHand.transform.position).x,
                            (-1 * (items.LeftHand.anchorOffset.position - items.LeftHand.transform.position)).y,
                            0);
                        items.LeftHand.transform.localPosition = new Vector3(items.LeftHand.transform.localPosition.x,
                            items.LeftHand.transform.localPosition.y,
                            -1 * items.LeftHand.transform.localPosition.z);
                    }
                    else
                    {
                        items.LeftHand.transform.eulerAngles = character.anchorRH.eulerAngles - items.LeftHand.anchorOffset.localEulerAngles;
                        items.LeftHand.transform.localPosition = character.anchorRH.localPosition + new Vector3
                            (1 * (items.LeftHand.anchorOffset.position - items.LeftHand.transform.position).x,
                            (-1 * (items.LeftHand.anchorOffset.position - items.LeftHand.transform.position)).y,
                            0);
                        items.LeftHand.transform.localPosition = new Vector3(items.LeftHand.transform.localPosition.x,
                            items.LeftHand.transform.localPosition.y,
                            -1 * items.LeftHand.transform.localPosition.z);
                    }
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
                    if (playerState.activelyUsingItem)
                    {
                        items.RightHand.transform.eulerAngles = character.anchorLH.eulerAngles - items.RightHand.anchorOffset.localEulerAngles;
                        items.RightHand.transform.localPosition = character.anchorLH.localPosition + new Vector3
                            (1 * (items.RightHand.anchorOffset.position - items.RightHand.transform.position).x,
                            (-1 * (items.RightHand.anchorOffset.position - items.RightHand.transform.position)).y,
                            0);
                        items.RightHand.transform.localPosition = new Vector3(items.RightHand.transform.localPosition.x,
                            items.RightHand.transform.localPosition.y,
                            -1 * items.RightHand.transform.localPosition.z);
                    }
                    else
                    {
                        items.RightHand.transform.eulerAngles = character.anchorLH.eulerAngles - items.RightHand.anchorOffset.localEulerAngles;
                        items.RightHand.transform.localPosition = character.anchorLH.localPosition + new Vector3
                            (1 * (items.RightHand.anchorOffset.position - items.RightHand.transform.position).x,
                            (-1 * (items.RightHand.anchorOffset.position - items.RightHand.transform.position)).y,
                            0);
                        items.RightHand.transform.localPosition = new Vector3(items.RightHand.transform.localPosition.x,
                            items.RightHand.transform.localPosition.y,
                            -1 * items.RightHand.transform.localPosition.z);
                    }
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
                if (playerState.jumpSquatCountdown <= 0)
                {
                    if (playerInputs.useLHand > 0)
                    {
                        if (items.LeftHand != null)
                        {
                            UseItem(items.LeftHand, "LH");
                            playerInputs.useLHand = 0;
                        }
                    }
                    if (playerInputs.useRHand > 0)
                    {
                        if (items.RightHand != null)
                        {
                            UseItem(items.RightHand, "RH");
                            playerInputs.useRHand = 0;
                        }
                    }
                    if (playerInputs.useHat > 0)
                    {
                        if (items.Hat != null)
                        {
                            UseItem(items.Hat, "H");
                            playerInputs.useHat = 0;
                        }
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
    }

    public void UseItem(Item item, string attachment)
    {
        playerState.activelyUsingItem = true; // set this back at the end
        Debug.Log("USE ITEM: " + item.itemType.name);

        if(item.itemType.attachType == Item.ItemType.AttachTypes.Hat)
        {
            // hat items
            playerState.itemAnimTime = item.hatAnimTime;
            item.actionTime = 0.0f;
            item.currentlyBeingUsed = true;
            item.currentUse += 1;
        }
        else
        {
            // handheld items
            playerState.itemAnimTime = character.ItemAnimation(item, playerState.facingDir, attachment);
            item.actionTime = 0.0f;
            item.currentlyBeingUsed = true;
            item.currentUse += 1;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerState.alive)
        {
            // update player state

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
                playerInputs.jumpPressed = 0;
                if (playerState.jumpSquatCountdown <= 0)
                {
                    playerInputs.jumpPressed = 0;
                    rb.velocity = new Vector3(rb.velocity.x, physicsAttributes.jumpForce);
                }
            }
            if (playerInputs.jumpPressed > 0 && playerState.onGround)
            {
                playerInputs.jumpPressed = 0;
                character.Jump();
                playerState.jumpSquatCountdown = physicsAttributes.jumpSquatTime;
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            playerState.facingDir = State.Dir.Right; transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    public void Die()
    {
        playerState.alive = false;

        if (items.LeftHand != null) { Destroy(items.LeftHand.gameObject); }
        if (items.RightHand != null) { Destroy(items.RightHand.gameObject); }
        if (items.Hat != null) { Destroy(items.Hat.gameObject); }

        foreach (GameObject projectile in myProjectiles)
        {
            if(projectile != null)
            {
                Destroy(projectile);
            }
        }
        myProjectiles.Clear();

        GetComponent<Collider>().isTrigger = true;
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision Enter !!!!!!!!!!!!!!!!!!!!!!!!!!!");
        //Debug.Log(collision.gameObject.layer);
        if (collision.gameObject.layer == 8)
        {
            /*
            if (playerState.alive)
            {
                Item itemToTry = collision.transform.GetChild(0).GetComponent<Item>();
                //Debug.Log(itemToTry.itemType.name);

                bool pickupSuccessful = false;
                if (itemToTry.itemType.attachType == Item.ItemType.AttachTypes.Hat)
                {
                    if (items.Hat != null) { }
                    else
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
                    if (items.LeftHand != null)
                    {
                        if (items.RightHand != null) { }
                        else
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

                if (pickupSuccessful)
                {
                    Destroy(collision.gameObject);
                }
            }
            */
        }
        else if (collision.gameObject.layer == 10)
        {
            Debug.Log("player hit killbox");
            Die();
        }
        else if (collision.gameObject.layer == 11)
        {
            // is a hitbox
            bool iAmException = false;
            Bullet bullet;
            Toast toast;
            Hitbox hitbox;
            if(collision.gameObject.TryGetComponent<Bullet>(out bullet)) { if (bullet.ownerException == this) { iAmException = true; } }
            if(collision.gameObject.TryGetComponent<Toast>(out toast)) { if (toast.ownerException == this) { iAmException = true; } }
            if(collision.gameObject.TryGetComponent<Hitbox>(out hitbox)) { if (hitbox.ownerException == this) { iAmException = true; } }

            Debug.Log("hit with hitbox");

            if(!iAmException)
            {
                Die();
            }
        }
    }
    

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            if (playerState.alive)
            {
                if (pickupDelay <= 0)
                {
                    Item itemToTry = other.transform.GetChild(0).GetComponent<Item>();
                    //Debug.Log(itemToTry.itemType.name);

                    bool pickupSuccessful = false;
                    if (itemToTry.itemType.attachType == Item.ItemType.AttachTypes.Hat)
                    {
                        if (items.Hat != null) { }
                        else
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
                        if (items.LeftHand != null)
                        {
                            if (items.RightHand != null) { }
                            else
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

                    if (pickupSuccessful)
                    {
                        Destroy(other.gameObject);
                        pickupDelay = 0f;
                    }
                }
            }
        }
    }
    */

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            if (playerState.alive)
            {
                if (pickupDelay <= 0 && other.GetComponent<ItemPickup>().alreadyPickedup == false)
                {
                    Item itemToTry = other.transform.GetChild(0).GetComponent<Item>();
                    //Debug.Log(itemToTry.itemType.name);

                    bool pickupSuccessful = false;
                    if (itemToTry.itemType.attachType == Item.ItemType.AttachTypes.Hat)
                    {
                        if (items.Hat != null) { }
                        else
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
                        if (items.LeftHand != null)
                        {
                            if (items.RightHand != null) { }
                            else
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

                    if (pickupSuccessful)
                    {
                        other.GetComponent<ItemPickup>().alreadyPickedup = true;
                        Destroy(other.gameObject);
                        pickupDelay = 0f;
                    }
                }
            }
        }
    }

    public GameObject FindCorrectItemProto(Item.ItemType.Names itemName)
    {
        GameObject result = battleController.itemDropLootTable[0].loot.gameObject;

        foreach (BattleController.ItemDropLoot itemDropLoot in battleController.itemDropLootTable)
        {
            if(itemDropLoot.loot.itemType.name == itemName)
            {
                result = itemDropLoot.loot.gameObject;
            }
        }

        return result;
    }

    public void Jump()
    {
        playerInputs.jumpPressed = 0.1f;
    }

    public void LeftHand()
    {
        playerInputs.useLHand = 0.1f;
    }

    public void RightHand()
    {
        playerInputs.useRHand = 0.1f;
    }

    public void Hat()
    {
        playerInputs.useHat = 0.1f;
    }
}
