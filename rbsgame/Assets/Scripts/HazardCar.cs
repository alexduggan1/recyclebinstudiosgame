using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardCar : MonoBehaviour
{
    public bool moving;
    public string dir;

    public Rigidbody rb;

    public float waitTime;
    public float waitUp;

    public LaunchBox launchBox;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        launchBox = GetComponent<LaunchBox>();

        dir = "r";
        waitTime = 8.5f;
        moving = false;
        waitUp = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (moving)
        {
            if (dir == "l")
            {
                rb.velocity = new Vector3(-650 * Time.fixedDeltaTime, 0, 0);
                transform.eulerAngles = new Vector3(0, 180, 0);

                launchBox.launchData.launchDirection = new Vector3(-1.8f, 1, 0).normalized;

                if (rb.position.x <= -50)
                {
                    moving = false;
                    dir = "r";

                    waitTime = Random.Range(1.2f, 7.8f);
                    waitUp = 0;
                }
            }
            else
            {
                rb.velocity = new Vector3(650 * Time.fixedDeltaTime, 0, 0);
                transform.eulerAngles = new Vector3(0, 0, 0);

                launchBox.launchData.launchDirection = new Vector3(1.8f, 1, 0).normalized;

                if (rb.position.x >= 50)
                {
                    moving = false;
                    dir = "l";

                    waitTime = Random.Range(1.2f, 7.8f);
                    waitUp = 0;
                }
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
            if (waitUp > waitTime)
            {
                waitUp = 0;
                moving = true;
            }
            else
            {
                waitUp += Time.fixedDeltaTime;
            }
        }
    }
}
