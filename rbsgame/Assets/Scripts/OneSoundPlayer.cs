using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.clip != null)
        {
            if (audioSource.time > audioSource.clip.length)
            {
                Destroy(gameObject);
            }
        }
    }
}
