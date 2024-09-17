using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Player ownerException;

    public float bulletSpeed;

    public Rigidbody rb;

    public Vector3 dir;

    public Transform visual;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Physics.IgnoreCollision(GetComponent<Collider>(), ownerException.GetComponent<Collider>(), true);
    }


    void FixedUpdate()
    {
        rb.MovePosition(transform.position + (bulletSpeed * Time.fixedDeltaTime * dir));

        if(dir == Vector3.right)
        {
            visual.transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            visual.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 7)
        {
            Debug.Log("bullet hit killbox or stage");
            Destroy(gameObject);
        }
    }
}
