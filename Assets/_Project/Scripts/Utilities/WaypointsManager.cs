using System.Collections.Generic;
using UnityEngine;

public class WaypointsManager : MonoBehaviour
{
    [SerializeField] private Transform[] _path0 = new Transform[4];

    public Transform[] GetPath(int agentId)
    {
        switch (agentId)
        {
            case 0:
                return _path0;
            default:
                return null;
        }
    }
}
