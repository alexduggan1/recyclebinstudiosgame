using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Player ownerException;

    public float bulletSpeed;

    public Rigidbody rb;

    public Vector3 dir;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Physics.IgnoreCollision(GetComponent<Collider>(), ownerException.GetComponent<Collider>(), true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
