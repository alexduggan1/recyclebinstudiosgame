using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SemisolidPlat : MonoBehaviour
{
    [SerializeField] private Vector3 entryDirection = Vector3.up;
    [SerializeField] private bool localDirection = false;
    [SerializeField, Range(1.0f, 2.0f)] private float triggerScale = 1.25f;
    private new BoxCollider collider = null;

    private BoxCollider collisionCheckTrigger = null;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        collider.isTrigger = false;

        collisionCheckTrigger = gameObject.AddComponent<BoxCollider>();
        collisionCheckTrigger.size = collisionCheckTrigger.size * triggerScale;
        collisionCheckTrigger.center = collider.center;
        collisionCheckTrigger.isTrigger = true;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
