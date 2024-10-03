using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Player ownerException;

    public Rigidbody rb;

    public Vector3 dir;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoAway(float time)
    {
        StartCoroutine(GoAwayInTime(time));
    }

    public IEnumerator GoAwayInTime(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }
}
