using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UltEvents;

public class Item : MonoBehaviour
{
    [System.Serializable]
    public class ItemType
    {
        public enum AttachTypes
        {
            Handheld, Hat
        };

        public enum Names
        {
            Handgun, BlusterBlade,
            PropellerHat, ToasterHat
        };

        public enum AnimType
        {
            Shoot, RegSwing, OverheadSwing
        }

        public AttachTypes attachType;
        public Names name;
        public AnimType animType;

        public ItemType(AttachTypes _attachType, Names _name)
        {
            attachType = _attachType;
            name = _name;
        }
    }

    public ItemType itemType;


    [System.Serializable]
    public class ItemAction
    {
        public float timeStamp;

        public UltEvent function;

        public bool done;
    }


    [System.Serializable]
    public class ItemUse
    {
        public List<ItemAction> actions;

        public bool done;
    }

    public List<ItemUse> itemUses;

    public int currentUse;


    public float actionTime;

    public bool pickedUp;

    public bool currentlyBeingUsed;

    // point to attach with
    public Transform anchorOffset;

    public Player myPlayer;

    public float hatAnimTime;

    // Start is called before the first frame update
    void Awake()
    {
        currentUse = -1;
        actionTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentlyBeingUsed)
        {
            actionTime += Time.deltaTime;
            if(itemUses.Count > 0)
            {
                foreach (ItemAction action in itemUses[currentUse].actions)
                {
                    if ((actionTime > action.timeStamp) && (!action.done))
                    {
                        if(action.function != null)
                        {
                            action.function.Invoke();
                        }
                        action.done = true;

                        if (action == itemUses[currentUse].actions[^1])
                        {
                            // if it's the last one turn off currentlybeingused
                            currentlyBeingUsed = false;
                            itemUses[currentUse].done = true;
                        }
                    }
                }
            }
        }
        else
        {
            bool outOfUses = true;
            if (itemUses.Count > 0)
            {
                foreach (ItemUse itemUse in itemUses)
                {
                    if (!itemUse.done) { outOfUses = false; }
                }
            }
            else { outOfUses = false; }
            if(outOfUses)
            {
                Debug.Log("out of uses of " + itemType.name.ToString() + "!!!!!!@!!!");
                Destroy(gameObject);
            }
        }
    }


    public void Shoot(GameObject objToShoot, float bulletSpeed)
    {
        Debug.Log("shoot!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        Vector3 dirToShoot = Vector3.right;
        if(myPlayer.playerState.facingDir == Player.State.Dir.Left) { dirToShoot = Vector3.right * -1; }
        GameObject bullet = Instantiate(objToShoot, transform.position, Quaternion.identity);
        if(bulletSpeed == 0) { bulletSpeed = 1; }
        bullet.GetComponent<Bullet>().bulletSpeed = bulletSpeed;
        bullet.GetComponent<Bullet>().dir = dirToShoot;
        bullet.GetComponent<Bullet>().ownerException = myPlayer;
    }

    public void PropellerJump(GameObject propellerToSpin, float jumpPower)
    {
        Debug.Log("PROPELLER JUMP");
        IEnumerator spinPropeller = SpinPropeller(propellerToSpin);
        StartCoroutine(spinPropeller);

        myPlayer.character.animator.Play("AirRise");
        myPlayer.rb.velocity = new Vector3(myPlayer.rb.velocity.x, jumpPower);
    }

    IEnumerator SpinPropeller(GameObject propellerToSpin)
    {
        float spinTime = 0;
        while (spinTime < hatAnimTime - 0.6f)
        {
            spinTime += Time.deltaTime;
            Vector3 propellerCenterPosition = propellerToSpin.GetComponent<Renderer>().bounds.center;
            propellerToSpin.transform.RotateAround(propellerCenterPosition, Vector3.up, (1200 - ((spinTime / (hatAnimTime - 0.6f)) * 900)) * Time.deltaTime);
            if (myPlayer.playerInputs.hMoveAxis > 0.05f)
            {
                myPlayer.playerState.facingDir = Player.State.Dir.Right;
            }
            if (myPlayer.playerInputs.hMoveAxis < -0.05f)
            {
                myPlayer.playerState.facingDir = Player.State.Dir.Left;
            }
            if (myPlayer.playerState.onGround) {
                if (myPlayer.playerState.activeDirectionalInput) {
                    // run
                    myPlayer.character.animator.Play("Run");
                    //Debug.Log("should be running");
                }
                else {
                    // idle
                    myPlayer.character.animator.Play("Idle");
                }
            }
            else {
                if (myPlayer.playerState.yVel < 0) {
                    myPlayer.character.animator.Play("Fall");
                }
                else {
                    myPlayer.character.animator.Play("AirRise");
                }
            }
            yield return new WaitForEndOfFrame();
        }
        while (spinTime < hatAnimTime)
        {
            spinTime += Time.deltaTime;
            Vector3 propellerCenterPosition = propellerToSpin.GetComponent<Renderer>().bounds.center;
            propellerToSpin.transform.RotateAround(propellerCenterPosition, Vector3.up, (400 - ((spinTime / (hatAnimTime - 0.6f)) * 200)) * Time.deltaTime);
            if (myPlayer.playerInputs.hMoveAxis > 0.05f)
            {
                myPlayer.playerState.facingDir = Player.State.Dir.Right;
            }
            if (myPlayer.playerInputs.hMoveAxis < -0.05f)
            {
                myPlayer.playerState.facingDir = Player.State.Dir.Left;
            }
            if (myPlayer.playerState.onGround)
            {
                if (myPlayer.playerState.activeDirectionalInput)
                {
                    // run
                    myPlayer.character.animator.Play("Run");
                    //Debug.Log("should be running");
                }
                else
                {
                    // idle
                    myPlayer.character.animator.Play("Idle");
                }
            }
            else
            {
                if (myPlayer.playerState.yVel < 0)
                {
                    myPlayer.character.animator.Play("Fall");
                }
                else
                {
                    myPlayer.character.animator.Play("AirRise");
                }
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    public void FireToast(GameObject toastToFire)
    {
        Debug.Log("FIRE TOSAST?!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        Vector3 dirToShoot = Vector3.up;
        GameObject newToast = Instantiate(toastToFire, transform.position, Quaternion.identity);
        newToast.transform.Rotate(new Vector3(0, -45, 0));
        newToast.GetComponent<Toast>().rb.velocity = new Vector3(0, 14);
        newToast.GetComponent<Toast>().ownerException = myPlayer;
    }

    public void SwingSword(GameObject hitbox, float hitboxTime, Vector3 offsetPos)
    {
        Debug.Log("swing sword!");
        if(myPlayer.playerState.facingDir == Player.State.Dir.Left) { offsetPos = new Vector3(offsetPos.x * -1, offsetPos.y, offsetPos.z); }
        IEnumerator makeHitbox = MakeHitbox(hitbox, hitboxTime, offsetPos);
        StartCoroutine(makeHitbox);
    }

    public IEnumerator MakeHitbox(GameObject hitbox, float hitboxTime, Vector3 offset)
    {
        GameObject h = Instantiate(hitbox, transform.position + offset, Quaternion.identity);
        yield return new WaitForSeconds(hitboxTime);
        Destroy(h);
    }
}
