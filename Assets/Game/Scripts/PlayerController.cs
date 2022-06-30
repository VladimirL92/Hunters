using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int speedCellPerSecond = 1;

    private Vector2Int mapPosition;
    private MapManager map;
    private Vector2Int velocity;
    private float lastMoveTime;
    private NoiseController noiseBar;
    private GameManage manager;

    public Vector2Int position
    {
        get { return this.mapPosition; }
    }

    private void Start()
    {
        manager = FindObjectOfType<GameManage>();
        noiseBar = FindObjectOfType<NoiseController>();
        map = FindObjectOfType<MapManager>();
        mapPosition = map.StartPoint;
        transform.position = new Vector3(mapPosition.x, mapPosition.y, 0);
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            velocity=Vector2Int.up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            velocity=Vector2Int.down;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            velocity=Vector2Int.right;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            velocity=Vector2Int.left;
        }
        else
        {
            velocity = Vector2Int.zero;
        }

        Move(velocity);
        UpdateRotation();
        CheckFinish();
    }
    void CheckFinish()
    {
        if (map.FinishPoint == mapPosition)
        {
            manager.GameWin();
        }
    }
    void UpdateRotation()
    {
        if (velocity == Vector2Int.up)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (velocity == Vector2Int.right)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        if (velocity == Vector2Int.left)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        if (velocity == Vector2Int.down)
        {
            transform.rotation = Quaternion.Euler(0, 0, -180);
        }
    }
    private void Move(Vector2Int target)
    {
        if (Time.time - lastMoveTime > 1 && velocity != Vector2Int.zero)
        {
            if (!map.IsWall(mapPosition + target))
            {
                noiseBar.PlayerSteep();
                mapPosition += target;
                var xWpos = mapPosition.x;
                var yWpos = mapPosition.y;
                transform.position = new Vector3(xWpos, yWpos, 0);
            }
            lastMoveTime = Time.time;
        }


    }
}


