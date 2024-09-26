using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchBox : MonoBehaviour
{
    public Player ownerException;

    public Rigidbody rb;

    public Vector3 dir;

    
    [System.Serializable]
    public class LaunchData
    {
        public Vector3 launchDirection;
        public float launchPower;

        public LaunchData(Vector3 _launchDirection, float _launchPower)
        {
            launchDirection = _launchDirection.normalized;
            launchPower = _launchPower;
        }
    }

    public LaunchData launchData;


    // Start is called before the first frame update
    void Awake()
    {
        launchData.launchDirection = launchData.launchDirection.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
