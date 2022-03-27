using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<SpawnPoint> Points;

    private int _currentIndex;

    public SpawnPoint GetNext()
    {
        var index = _currentIndex;
        
        _currentIndex++;
        if (_currentIndex >= Points.Count)
            _currentIndex = 0;
        
        return Points[index];
    }

    [ContextMenu("Find Spawns")]
    public void FindSpawns()
    {
        Points = new List<SpawnPoint>(transform.GetComponentsInChildren<SpawnPoint>());
    }
}
