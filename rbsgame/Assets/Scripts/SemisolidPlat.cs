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

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        collider.isTrigger = false;

        collisionCheckTrigger = gameObject.AddComponent<BoxCollider>();
        collisionCheckTrigger.size = Vector3.Scale(collider.size, triggerScale);
        collisionCheckTrigger.center = collider.center;
        collisionCheckTrigger.isTrigger = true;

        dropPlayers = new List<DropPlayer> { };
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
