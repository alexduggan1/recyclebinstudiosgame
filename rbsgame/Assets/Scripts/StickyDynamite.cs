using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyDynamite : MonoBehaviour
{
    public Player ownerException;

    public float bulletSpeed;

    public Rigidbody rb;

    public Vector3 dir;

    public Transform visual;
    public Transform visualRotatePoint;

    public float startingBulletSpeed;

    public bool exploding;


    public GameObject explosionKillbox;

    public Vector3 explodePos;

    Color origColor;

    float cd;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        exploding = false;

        origColor = visual.GetComponent<MeshRenderer>().materials[2].color;

        rb.velocity = dir * startingBulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (exploding)
        {
            transform.position = explodePos;

            cd -= Time.deltaTime;
            if(cd < 0)
            {
                if(visual.GetComponent<MeshRenderer>().materials[2].color == origColor)
                {
                    visual.GetComponent<MeshRenderer>().materials[2].color = Color.white;
                }
                else
                {
                    visual.GetComponent<MeshRenderer>().materials[2].color = origColor;
                }
                cd = 0.3f;
            }
        }
        else
        {
            if (dir.x >= 0)
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
    }

    public void Explode()
    {
        Hitbox e = Instantiate(explosionKillbox, transform.position, Quaternion.identity).GetComponent<Hitbox>();
        e.GoAway(0.4f);
    }

    public IEnumerator Fuse()
    {
        exploding = true;

        yield return new WaitForSeconds(3f);

        Explode();
        yield return null;

        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 7)
        {
            Debug.Log("banana hit killbox or stage");
            // stick
            rb.constraints = RigidbodyConstraints.FreezeAll;

            if (!exploding)
            {
                explodePos = transform.position;
                StartCoroutine(Fuse());
            }
        }
    }
}
