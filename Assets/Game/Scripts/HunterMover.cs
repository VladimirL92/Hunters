using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterMover : MonoBehaviour
{
    private float lastMoveTime;
    private int trailIndex;
    private Vector2Int[] trail;
    private Vector2Int cellPosition;


    public void FindWay(bool follow = false)
    {
        var map = FindObjectOfType<MapGenerator>();

        if (follow)
        {
            trailIndex = 1;
            var player = FindObjectOfType<PlayerMover>();
            var p = player.transform.position;
            var cp = new Vector2Int((int)p.x,(int)p.y);
            trail = map.FindWay(cellPosition, cp);
        }
        else if (map.TryFindFreePoint(out var point))
        {
                trail = map.FindWay(cellPosition, point);
        }

    }
    private void Update()
    {
        if (Time.time - lastMoveTime > 1)
        {
            lastMoveTime = Time.time;
            Move();
        }
    }

    public void SetCellPosition(Vector2Int position)
    {
        cellPosition = position;
        transform.position = new Vector3(cellPosition.x, cellPosition.y, -2);

    }

    private void Move()
    {
        FindWay(true);
        if (trail.Length == 0)
        {
            return;
        }        
        var index = trailIndex;

        if (trailIndex >= trail.Length)
        {
            index = trail.Length + (trail.Length - trailIndex -1);
        }

        var pos = trail[index];

        SetCellPosition(pos);
        trailIndex++;

        if (trailIndex >= trail.Length * 2)
        {
            trailIndex = 0;
        }
    }
}
