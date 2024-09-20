using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPoint
    {
        public GameObject shortcut;

        public int IDFor2Player = -1;
        public int IDFor3Player = -1;
        public int IDFor4Player = -1;


        public SpawnPoint(GameObject _shortcut, int _ID2P = -1, int _ID3P = -1, int _ID4P = -1)
        {
            shortcut = _shortcut;
            IDFor2Player = _ID2P;
            IDFor3Player = _ID3P;
            IDFor4Player = _ID4P;
        }
    }

    [SerializeField]
    public List<SpawnPoint> spawnPoints;

    public GameObject collision;

    public Transform ceiling;

    public Sprite thumbnail;
}
