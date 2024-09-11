using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Rigidbody rb;
    public BoxCollider col;

    public Item item;

    public bool hitGround;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();

        rb.useGravity = true;
        hitGround = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hitGround)
        {
            item.transform.Rotate(Vector3.up, 320 * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 7)
        {
            // hit stage
            rb.constraints = RigidbodyConstraints.FreezeAll;
            col.isTrigger = true;
            rb.useGravity = false;
            hitGround = true;
        }
    }
}
