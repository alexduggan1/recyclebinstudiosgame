using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bananarang : MonoBehaviour
{
    public Player ownerException;

    public float bulletSpeed;

    public Rigidbody rb;

    public Vector3 dir;

    public Transform visual;
    public Transform visualRotatePoint;

    public float startingBulletSpeed;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        IEnumerator bananaReturn = BananaReturn();
        StartCoroutine(bananaReturn);
        Physics.IgnoreCollision(GetComponent<Collider>(), ownerException.GetComponent<Collider>(), true);
    }


    void FixedUpdate()
    {
        Debug.Log(bulletSpeed);
        rb.MovePosition(transform.position + (bulletSpeed * Time.fixedDeltaTime * dir));

        if (dir == Vector3.right)
        {
            visual.transform.localEulerAngles = new Vector3(0, 180, visual.transform.localEulerAngles.z);
            visual.transform.RotateAround(visualRotatePoint.position, Vector3.forward, -1440 * Time.fixedDeltaTime);
        }
        else
        {
            visual.transform.localEulerAngles = new Vector3(0, 0, visual.transform.localEulerAngles.z);
            visual.transform.RotateAround(visualRotatePoint.position, Vector3.forward, 1440 * Time.fixedDeltaTime);
        }
    }

    IEnumerator BananaReturn()
    {
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime;
            bulletSpeed = startingBulletSpeed;
            yield return new WaitForEndOfFrame();
        }
        while (timer < 2)
        {
            timer += Time.deltaTime;
            bulletSpeed -= Time.deltaTime * startingBulletSpeed * 2;
            yield return new WaitForEndOfFrame();

        }
        bulletSpeed = -startingBulletSpeed;
        yield return null;

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 7)
        {
            Debug.Log("banana hit killbox or stage");
            Destroy(gameObject);
        }
    }
}
