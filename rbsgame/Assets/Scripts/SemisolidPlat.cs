using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SemisolidPlat : MonoBehaviour
{
    [SerializeField] private Vector3 entryDirection = Vector3.up;
    //[SerializeField] private bool localDirection = false;
    [SerializeField] private Vector3 triggerScale = new Vector3(1, 1.5f, 1);
    private new BoxCollider collider = null;

    private BoxCollider collisionCheckTrigger = null;

    public class DropPlayer
    {
        public Player player;
        public bool dropping;

        public DropPlayer(Player _player, bool _dropping)
        {
            player = _player;
            dropping = _dropping;
        }
    }

    public List<DropPlayer> dropPlayers;

    public List<Vector3> movePoints;
    int currentMovePoint;
    public float smoothTime = 3.0f;
    float xVel = 0.0f;
    float yVel = 0.0f;

    float currentSmoothTime;

    public List<GameObject> objectsOnMe = new List<GameObject> { };

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        collider.isTrigger = false;

        collisionCheckTrigger = gameObject.AddComponent<BoxCollider>();
        collisionCheckTrigger.size = Vector3.Scale(collider.size, triggerScale);
        collisionCheckTrigger.center = collider.center;
        collisionCheckTrigger.isTrigger = true;

        dropPlayers = new List<DropPlayer> { };

        currentMovePoint = 0;
        currentSmoothTime = 0.0f;

        objectsOnMe.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        bool playerPressingDrop = false;
        Player playerInQuestion;
        if (other.TryGetComponent<Player>(out playerInQuestion))
        {
            DropPlayer foundDropPlayer = null;
            foreach (DropPlayer dp in dropPlayers)
            {
                if(dp.player == playerInQuestion)
                {
                    foundDropPlayer = dp;
                }
            }
            if(foundDropPlayer != null)
            {
                if (!foundDropPlayer.dropping)
                {
                    foundDropPlayer.dropping = playerInQuestion.playerInputs.dropPressed;
                }
            }
            else
            {
                foundDropPlayer = new DropPlayer(playerInQuestion, playerInQuestion.playerInputs.dropPressed);
                dropPlayers.Add(foundDropPlayer);
            }
            playerPressingDrop = foundDropPlayer.dropping;
        }

        if (Physics.ComputePenetration(
            collisionCheckTrigger, transform.position, transform.rotation,
            other, other.transform.position, other.transform.rotation,
            out Vector3 collisionDirection, out float penetrationDepth
            ))
        {
            float dot = Vector3.Dot(entryDirection, collisionDirection);

            if (dot < 0 && (!playerPressingDrop))
            {
                Physics.IgnoreCollision(collider, other, false);
            }
            else
            {
                Physics.IgnoreCollision(collider, other, true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        bool playerPressingDrop = false;
        Player playerInQuestion;
        if (other.TryGetComponent<Player>(out playerInQuestion))
        {
            DropPlayer foundDropPlayer = null;
            foreach (DropPlayer dp in dropPlayers)
            {
                if (dp.player == playerInQuestion)
                {
                    foundDropPlayer = dp;
                }
            }
            if (foundDropPlayer != null)
            {
                if (!foundDropPlayer.dropping)
                {
                    foundDropPlayer.dropping = playerInQuestion.playerInputs.dropPressed;
                }
            }
            else
            {
                foundDropPlayer = new DropPlayer(playerInQuestion, playerInQuestion.playerInputs.dropPressed);
                dropPlayers.Add(foundDropPlayer);
            }
            playerPressingDrop = foundDropPlayer.dropping;
        }

        if (Physics.ComputePenetration(
            collisionCheckTrigger, transform.position, transform.rotation,
            other, other.transform.position, other.transform.rotation,
            out Vector3 collisionDirection, out float penetrationDepth
            ))
        {
            float dot = Vector3.Dot(entryDirection, collisionDirection);

            if (dot < 0 && (!playerPressingDrop))
            {
                Physics.IgnoreCollision(collider, other, false);
            }
            else
            {
                Physics.IgnoreCollision(collider, other, true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player playerInQuestion;
        if (other.TryGetComponent<Player>(out playerInQuestion))
        {
            DropPlayer foundDropPlayer = null;
            foreach (DropPlayer dp in dropPlayers)
            {
                if (dp.player == playerInQuestion)
                {
                    foundDropPlayer = dp;
                }
            }
            if (foundDropPlayer != null)
            {
                dropPlayers.Remove(foundDropPlayer);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("coll enter");
        if (!objectsOnMe.Contains(collision.gameObject))
        {
            objectsOnMe.Add(collision.gameObject);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Log("coll stay");
        if (!objectsOnMe.Contains(collision.gameObject))
        {
            objectsOnMe.Add(collision.gameObject);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log("coll exit");
        if (objectsOnMe.Contains(collision.gameObject))
        {
            if(! collision.gameObject.TryGetComponent<ItemPickup>(out ItemPickup ip))
            {
                objectsOnMe.Remove(collision.gameObject);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(movePoints.Count > 0)
        {
            if (currentSmoothTime > smoothTime)
            {
                currentMovePoint++;
                if(currentMovePoint >= movePoints.Count) { currentMovePoint = 0; }
                currentSmoothTime = 0.0f;
            }
            else
            {
                currentSmoothTime += Time.fixedDeltaTime;
                float newPosX = Mathf.SmoothDamp(transform.position.x, movePoints[currentMovePoint].x, ref xVel, smoothTime);
                float newPosY = Mathf.SmoothDamp(transform.position.y, movePoints[currentMovePoint].y, ref yVel, smoothTime);

                Vector3 diff = new Vector3(newPosX, newPosY, transform.position.z) - transform.position;
                diff = new Vector3(diff.x, diff.y, 0);

                transform.position = new Vector3(newPosX, newPosY, transform.position.z);

                List<GameObject> objectsOnMeBetter = new List<GameObject> { };
                foreach (GameObject obj in objectsOnMe)
                {
                    if (obj != null)
                    {
                        if (!objectsOnMeBetter.Contains(obj))
                        {
                            objectsOnMeBetter.Add(obj);
                        }
                    }
                }
                foreach (GameObject obj in objectsOnMeBetter)
                {
                    if (obj != null)
                    {
                        if(obj.TryGetComponent<Rigidbody>(out Rigidbody rb) && (! obj.TryGetComponent<ItemPickup>(out ItemPickup ip)))
                        {
                            rb.position += diff;
                        }
                        else
                        {
                            obj.transform.position += diff;
                        }
                    }
                }
                objectsOnMe.Clear();
                foreach (GameObject obj in objectsOnMeBetter)
                {
                    objectsOnMe.Add(obj);
                }
            }
        }
    }
}
