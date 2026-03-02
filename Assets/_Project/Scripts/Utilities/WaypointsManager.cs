using System.Collections.Generic;
using UnityEngine;

public class WaypointsManager : MonoBehaviour
{
    [SerializeField] private Transform[] _path0 = new Transform[4];
    [SerializeField] private Transform[] _path1 = new Transform[4];
    [SerializeField] private Transform[] _path2 = new Transform[4];

    public Transform[] GetPath(int agentId)
    {
        switch (agentId)
        {
            case 0:
                return _path0;
            case 1:
                return _path1;
            case 2:
                return _path2;
            default:
                Debug.LogWarning($"No path found for agentId {agentId}");
                return null;
        }
    }
}
