using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toast : MonoBehaviour
{
    public Player ownerException;

    public float bulletSpeed;

    public Rigidbody rb;

    public Transform visual;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Physics.IgnoreCollision(GetComponent<Collider>(), ownerException.GetComponent<Collider>(), true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 7)
        {
            Debug.Log("TOAST BECOME DIE");
            Destroy(gameObject);
        }
    }
}
