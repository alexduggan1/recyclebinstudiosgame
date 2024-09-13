using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SemisolidPlat : MonoBehaviour
{
    [SerializeField] private Vector3 entryDirection = Vector3.up;
    [SerializeField] private bool localDirection = false;
    [SerializeField] private Vector3 triggerScale = new Vector3(1, 1.5f, 1);
    private new BoxCollider collider = null;

    private BoxCollider collisionCheckTrigger = null;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        collider.isTrigger = false;

        collisionCheckTrigger = gameObject.AddComponent<BoxCollider>();
        collisionCheckTrigger.size = Vector3.Scale(collider.size, triggerScale);
        collisionCheckTrigger.center = collider.center;
        collisionCheckTrigger.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Physics.ComputePenetration(
            collisionCheckTrigger, transform.position, transform.rotation,
            other, other.transform.position, other.transform.rotation,
            out Vector3 collisionDirection, out float penetrationDepth
            ))
        {
            float dot = Vector3.Dot(entryDirection, collisionDirection);

            if (dot < 0)
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
        if(Physics.ComputePenetration(
            collisionCheckTrigger, transform.position, transform.rotation,
            other, other.transform.position, other.transform.rotation,
            out Vector3 collisionDirection, out float penetrationDepth
            ))
        {
            float dot = Vector3.Dot(entryDirection, collisionDirection);
            
            if(dot < 0)
            {
                Physics.IgnoreCollision(collider, other, false);
            }
            else
            {
                Physics.IgnoreCollision(collider, other, true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
