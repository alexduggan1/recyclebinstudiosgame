using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UltEvents;
using TMPro.Examples;

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
            PropellerHat, ToasterHat, Fish, Bananarang, OrigamiDragon, SpikeHat, TopHat, PartyHat, CarHat, Bazooka
        };

        public enum AnimType
        {
            Shoot, OverheadSwing
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
    public float notPickedUpScale = 1;
    public Vector3 regularScale;

    public bool currentlyBeingUsed;

    // point to attach with
    public Transform anchorOffset;

    public Player myPlayer;

    public float hatAnimTime;
    public float handheldAnimSpeed = 1;

    public Sprite thumbnail;

    // Start is called before the first frame update
    void Awake()
    {
        currentUse = -1;
        actionTime = 0;
        regularScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pickedUp) { transform.localScale = regularScale * notPickedUpScale; }
        else { transform.localScale = regularScale; }

        if (currentlyBeingUsed)
        {
            actionTime += Time.deltaTime;
            if(itemUses.Count > 0)
            {
                if(itemUses.Count > currentUse)
                {
                    foreach (ItemAction action in itemUses[currentUse].actions)
                    {
                        if ((actionTime > action.timeStamp) && (!action.done))
                        {
                            if (action.function != null)
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
                else
                {
                    currentlyBeingUsed = false;
                    foreach (ItemUse itemUse in itemUses)
                    {
                        if (!itemUse.done) { itemUse.done = true; }
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
        if (myPlayer.playerState.facingDir == Player.State.Dir.Left) { dirToShoot = Vector3.right * -1; }
        GameObject bullet = Instantiate(objToShoot, transform.position, Quaternion.identity);
        myPlayer.myProjectiles.Add(bullet);
        if (bulletSpeed == 0) { bulletSpeed = 1; }
        bullet.GetComponent<Bullet>().bulletSpeed = bulletSpeed;
        bullet.GetComponent<Bullet>().dir = dirToShoot;
        bullet.GetComponent<Bullet>().ownerException = myPlayer;
        Physics.IgnoreCollision(bullet.GetComponent<Collider>(), myPlayer.GetComponent<Collider>(), true);
    }

    public void ShootBanana(GameObject objToShoot, float bulletSpeed)
    {
        Debug.Log("BANAAN!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        Vector3 dirToShoot = Vector3.right;
        if (myPlayer.playerState.facingDir == Player.State.Dir.Left) { dirToShoot = Vector3.right * -1; }
        GameObject bullet = Instantiate(objToShoot, transform.position, Quaternion.identity);
        myPlayer.myProjectiles.Add(bullet);
        if (bulletSpeed == 0) { bulletSpeed = 1; }
        Debug.Log(bulletSpeed);
        bullet.GetComponent<Bananarang>().bulletSpeed = bulletSpeed;
        bullet.GetComponent<Bananarang>().startingBulletSpeed = bulletSpeed;
        bullet.GetComponent<Bananarang>().dir = dirToShoot;
        if (dirToShoot == Vector3.right)
        {
            bullet.GetComponent<Bananarang>().visual.transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            bullet.GetComponent<Bananarang>().visual.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        bullet.GetComponent<Bananarang>().ownerException = myPlayer;

        Physics.IgnoreCollision(bullet.GetComponent<Collider>(), myPlayer.GetComponent<Collider>(), true);
    }

    public void DeleteBanana()
    {
        Destroy(gameObject);
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
            myPlayer.playerState.onGround = Physics.Raycast(new Ray(myPlayer.transform.position + (Vector3.down * 0.75f), Vector3.down), 1f, myPlayer.stageLayer.value);

            if (myPlayer.playerState.onGround) {
                Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
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
            myPlayer.playerState.onGround = Physics.Raycast(new Ray(myPlayer.transform.position + (Vector3.down * 0.75f), Vector3.down), 1f, myPlayer.stageLayer.value);

            if (myPlayer.playerState.onGround)
            {
                Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
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
        myPlayer.myProjectiles.Add(newToast);
        newToast.transform.Rotate(new Vector3(0, -45, 0));
        newToast.GetComponent<Toast>().rb.velocity = new Vector3(0, 14);
        newToast.GetComponent<Toast>().ownerException = myPlayer;
    }

    public void ShootVeggies(GameObject objToShoot, float bulletSpeed, int count, float angle)
    {
        Debug.Log("VEGEBTABLE!!!!!!!!!");
            Vector3 dirToShoot = new Vector3(bulletSpeed, angle);
            if (myPlayer.playerState.facingDir == Player.State.Dir.Left) { dirToShoot = new Vector3(dirToShoot.x * -1,dirToShoot.y); }
            GameObject veggie = Instantiate(objToShoot, transform.position, Quaternion.identity);
            myPlayer.myProjectiles.Add(veggie);
            veggie.GetComponent<Vegetable>().dir = dirToShoot;
            veggie.GetComponent<Vegetable>().ownerException = myPlayer;
            veggie.GetComponent<Vegetable>().rb.velocity = dirToShoot;

            Physics.IgnoreCollision(veggie.GetComponent<Collider>(), myPlayer.GetComponent<Collider>(), true);
    }
    public void FireSpikes(GameObject spikeToFire, GameObject spikeToFire2)
    {
        Debug.Log("FIRE SPIEKS?!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        GameObject newSpike1 = Instantiate(spikeToFire, transform.position, Quaternion.identity);
        myPlayer.myProjectiles.Add(newSpike1);
        newSpike1.transform.Rotate(new Vector3(0, 0, -45));
        newSpike1.GetComponent<Spike>().rb.velocity = new Vector3(10, 10);
        newSpike1.GetComponent<Spike>().ownerException = myPlayer;
        Physics.IgnoreCollision(newSpike1.GetComponent<Collider>(), myPlayer.GetComponent<Collider>(), true);

        GameObject newSpike2 = Instantiate(spikeToFire2, transform.position, Quaternion.identity);
        myPlayer.myProjectiles.Add(newSpike2);
        newSpike2.transform.Rotate(new Vector3(0, 0, 45));
        newSpike2.GetComponent<Spike>().rb.velocity = new Vector3(-10, 10);
        newSpike2.GetComponent<Spike>().ownerException = myPlayer;
        Physics.IgnoreCollision(newSpike2.GetComponent<Collider>(), myPlayer.GetComponent<Collider>(), true);
        Physics.IgnoreCollision(newSpike2.GetComponent<Collider>(), newSpike1.GetComponent<Collider>(), true);
    }

    public void SwingSword(GameObject hitbox, float hitboxTime, Vector3 offsetPos)
    {
        Debug.Log("swing sword!");
        if(myPlayer.playerState.facingDir == Player.State.Dir.Left) { offsetPos = new Vector3(offsetPos.x * -1, offsetPos.y, offsetPos.z); }
        IEnumerator makeHitbox = MakeHitbox(hitbox, hitboxTime, offsetPos, myPlayer);
        StartCoroutine(makeHitbox);
    }

    public void SwingFish(GameObject launchbox, float hitboxTime, Vector3 offsetPos, Vector3 launchDir, float launchPower)
    {
        Debug.Log("swing fish!");
        LaunchBox.LaunchData launchData = new LaunchBox.LaunchData(launchDir, launchPower);
        if (myPlayer.playerState.facingDir == Player.State.Dir.Left) 
        { 
            offsetPos = new Vector3(offsetPos.x * -1, offsetPos.y, offsetPos.z);
            launchData.launchDirection = new Vector3(launchData.launchDirection.x * -1, launchData.launchDirection.y, launchData.launchDirection.z);
        }
        IEnumerator makeLaunchbox = MakeLaunchbox(launchbox, hitboxTime, offsetPos, myPlayer, launchData);
        StartCoroutine(makeLaunchbox);
    }

    public IEnumerator MakeHitbox(GameObject hitbox, float hitboxTime, Vector3 offset, Player exception)
    {
        GameObject h = Instantiate(hitbox, myPlayer.transform.position + offset, Quaternion.identity);
        exception.myProjectiles.Add(h);
        h.GetComponent<Hitbox>().ownerException = exception;
        Physics.IgnoreCollision(h.GetComponent<Collider>(), myPlayer.GetComponent<Collider>(), true);
        yield return new WaitForSeconds(hitboxTime);
        Destroy(h);
    }

    public IEnumerator MakeLaunchbox(GameObject launchbox, float hitboxTime, Vector3 offset, Player exception, LaunchBox.LaunchData launchData)
    {
        GameObject l = Instantiate(launchbox, myPlayer.transform.position + offset, Quaternion.identity);
        exception.myProjectiles.Add(l);
        l.GetComponent<LaunchBox>().ownerException = exception;
        l.GetComponent<LaunchBox>().launchData = launchData;
        Physics.IgnoreCollision(l.GetComponent<Collider>(), myPlayer.GetComponent<Collider>(), true);
        yield return new WaitForSeconds(hitboxTime);
        Destroy(l);
    }

    public void OrigamiDragon(ParticleSystem fireParticles, ParticleSystem smokeParticles, GameObject hitbox)
    {
        IEnumerator dragonFire = DragonFire(fireParticles, smokeParticles, hitbox);
        StartCoroutine(dragonFire);
    }
    public void PlacePortal(GameObject portalObj)
    {
        Debug.Log("PLACING PORTAL??!!");
        GameObject newPortal = Instantiate(portalObj, transform.position, Quaternion.identity);
        newPortal.transform.Rotate(new Vector3(0, 90, 0));
        myPlayer.myPortals.Clear();
        myPlayer.myPortals.Add(newPortal); myPlayer.myProjectiles.Add(newPortal);
    }

    public void UsePortal()
    {
        Debug.Log("telperting??????");
        GameObject gotoPortal = myPlayer.myPortals[0];
        myPlayer.transform.position = gotoPortal.transform.position;
        myPlayer.myPortals.Remove(gotoPortal);
        Destroy(gotoPortal);
    }

    public void PartyImmune()
    {
        Debug.Log("PARTY TIME!!!!");
        foreach (Bullet bullet in FindObjectsByType<Bullet>(FindObjectsSortMode.None))
        {
            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), myPlayer.GetComponent<Collider>(), true);
        }
        foreach (Toast toast in FindObjectsByType<Toast>(FindObjectsSortMode.None))
        {
            Physics.IgnoreCollision(toast.GetComponent<Collider>(), myPlayer.GetComponent<Collider>(), true);
        }
        foreach (Bananarang bananarang in FindObjectsByType<Bananarang>(FindObjectsSortMode.None))
        {
            Physics.IgnoreCollision(bananarang.GetComponent<Collider>(), myPlayer.GetComponent<Collider>(), true);
        }
    }

    public IEnumerator DragonFire(ParticleSystem fireParticles, ParticleSystem smokeParticles, GameObject hitbox)
    {
        float realTime = 0;

        Physics.IgnoreCollision(hitbox.GetComponent<Collider>(), myPlayer.GetComponent<Collider>(), true);

        fireParticles.Stop();
        smokeParticles.Stop();
        hitbox.SetActive(false);

        while (realTime < (7f/60f))
        {
            myPlayer.character.animator.speed = 1;
            realTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        fireParticles.Play();
        smokeParticles.Play();
        hitbox.SetActive(true);

        while (realTime < ((7f + (15f * (1f / 0.2f))) / 60f))
        {
            myPlayer.character.animator.speed = 0.2f;
            realTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        fireParticles.Stop();
        smokeParticles.Stop();
        hitbox.SetActive(false);

        while (realTime < ((7f + (15f * (1f / 0.2f)) + 10f) / 60f))
        {
            myPlayer.character.animator.speed = 1f;
            realTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }


        yield return null;
    }

    public void DriveCar(Collider stayOnGround, LaunchBox launcher, float driveSpeed)
    {
        IEnumerator drivingCar = DrivingCar(stayOnGround, launcher, driveSpeed);
        StartCoroutine(drivingCar);

        Debug.Log(transform.eulerAngles);
        Debug.Log(myPlayer.transform.eulerAngles.z - transform.eulerAngles.z);
    }

    public IEnumerator DrivingCar(Collider stayOnGround, LaunchBox launcher, float driveSpeed)
    {
        float driveTime = 0;
        myPlayer.playerState.activelyUsingItem = true;
        myPlayer.playerState.itemAnimTime = hatAnimTime;

        Player.State.Dir driveDir = myPlayer.playerState.facingDir;

        myPlayer.transform.position += new Vector3(0, stayOnGround.transform.lossyScale.y, 0);
        stayOnGround.enabled = true;
        launcher.gameObject.SetActive(true);
        Physics.IgnoreCollision(myPlayer.GetComponent<Collider>(), launcher.GetComponent<Collider>(), true);

        Vector3 ld = launcher.launchData.launchDirection;

        while (driveTime < hatAnimTime)
        {
            driveTime += Time.deltaTime;

            myPlayer.playerState.rotLocked = true;
            myPlayer.playerState.freeMovement = true;

            if (myPlayer.playerInputs.hMoveAxis > 0.05f) { driveDir = Player.State.Dir.Right; }
            if (myPlayer.playerInputs.hMoveAxis < -0.05f) { driveDir = Player.State.Dir.Left; }

            if (driveDir == Player.State.Dir.Right)
            {
                myPlayer.transform.eulerAngles = new Vector3(0, 180, 180 + (myPlayer.transform.eulerAngles.z - transform.eulerAngles.z));
                myPlayer.rb.velocity = new Vector3(driveSpeed * 60f * Time.deltaTime, myPlayer.rb.velocity.y, 0);
                launcher.launchData.launchDirection = new Vector3(ld.x, ld.y, 0);
            }
            else
            {
                myPlayer.transform.eulerAngles = new Vector3(0, 0, 180 + (myPlayer.transform.eulerAngles.z - transform.eulerAngles.z));
                myPlayer.rb.velocity = new Vector3(-driveSpeed * 60f * Time.deltaTime, myPlayer.rb.velocity.y, 0);
                launcher.launchData.launchDirection = new Vector3(-ld.x, ld.y, 0);
            }

            yield return new WaitForEndOfFrame();
        }

        StopDriving(stayOnGround, launcher);
        yield return null;
    }

    public void StopDriving(Collider stayOnGround, LaunchBox launcher)
    {
        myPlayer.playerState.rotLocked = false;
        myPlayer.playerState.freeMovement = false;
    }
}
