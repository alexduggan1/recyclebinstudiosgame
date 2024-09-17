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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 7)
        {
            bool UpThruPlat = false;
            SemisolidPlat hope;
            if(collision.gameObject.TryGetComponent<SemisolidPlat>(out hope))
            {
                if(rb.velocity.y > 0.03f) { UpThruPlat = true; }
            }

            if(!UpThruPlat)
            {
                Destroy(gameObject);
            }
        }
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 7)
        {
            bool UpThruPlat = false;
            SemisolidPlat hope;
            if (collision.gameObject.TryGetComponent<SemisolidPlat>(out hope))
            {
                if (rb.velocity.y > 0.03f) { UpThruPlat = true; }
            }

            if (!UpThruPlat)
            {
                Destroy(gameObject);
            }
        }
    }
}
