using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [System.Serializable]
    public class ItemType
    {
        public enum AttachTypes
        {
            Handheld, Hat
        };

        public enum Names
        {
            Handgun
        };

        public AttachTypes attachType;
        public Names name;

        public ItemType(AttachTypes _attachType, Names _name)
        {
            attachType = _attachType;
            name = _name;
        }
    }

    // point to attach with
    // 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
