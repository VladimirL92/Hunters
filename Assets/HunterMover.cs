using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterMover : MonoBehaviour
{
    [SerializeField]
    private Vector3Int[] trail = new[]
    {
        new Vector3Int(0,0,0),
        new Vector3Int(0,1,0),
        new Vector3Int(1,1,0),
        new Vector3Int(1,2,0),
        new Vector3Int(1,3,0),
        new Vector3Int(2,3,0)
    };

    private float lastMoveTime;
    private int trailIndex;
    private void Update()
    {
        if (Time.time - lastMoveTime > 1)
        {
            lastMoveTime = Time.time;
            Move();

        }
        
    }
    private void Move()
    {
        var index = trailIndex;

        if (trailIndex >= trail.Length)
        {
            index = trail.Length + (trail.Length - trailIndex -1);
        }

        var pos = trail[index];
        transform.position = new Vector3(pos.x, pos.y, -2);
        trailIndex++;

        if (trailIndex >= trail.Length * 2)
        {
            trailIndex = 0;
        }
    }
}
