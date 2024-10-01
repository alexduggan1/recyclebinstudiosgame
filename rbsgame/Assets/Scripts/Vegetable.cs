using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vegetable : MonoBehaviour
{
    public Player ownerException;

    public float bulletSpeed;

    public Rigidbody rb;

    public Vector3 dir;

    public Transform visual;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 7)
        {
            Debug.Log("VEGEBLE BECOME DIE");
            Destroy(gameObject);
        }
    }
}
