using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterController : MonoBehaviour
{
    public Vector2Int[] WalkWay;
    private Vector2Int mapPosition;
    private int waySteep;
    private float lastTime;
    [SerializeField]
    private float speed = 1f;
    private NoiseController noiseBar;
    private MapManager map;
    private PlayerController player;
    private SpriteRenderer hunterColor;
    private GameManage manager;
    private bool detected;
    private Vector2Int velocity;
    private void Start()
    {
        manager = FindObjectOfType<GameManage>();
        noiseBar = FindObjectOfType<NoiseController>();
        map = FindObjectOfType<MapManager>();
        player = FindObjectOfType<PlayerController>();
        hunterColor = GetComponent<SpriteRenderer>();

    }
    private void Update()
    {
        if (Time.time - lastTime > speed)
        {
            lastTime = Time.time;
            if (detected || noiseBar.NoiseClip)
            {
                Attack();
            }
            else
            {
                Patrol();
            }

        }

        detected = PlayerDetect();
    }
    void Attack()
    {
        WalkWay = map.FindWay(mapPosition, player.Position);
        hunterColor.color = Color.red;
        if (WalkWay.Length > 1)
        {
            SetPosition(WalkWay[1]);
        }
        if ( mapPosition == player.Position)
        {
            manager.GameOver();
        }
    }
    public void SetPosition(Vector2Int Position)
    {
        velocity = Position - mapPosition;
        UpdateRotation();
        mapPosition = Position;
        transform.position = new Vector3(Position.x, Position.y, 0);
    }
    void Patrol()
    {
        if (WalkWay.Length == 0)
        {
            return;
        }

        var steep = waySteep;
        if (waySteep >= WalkWay.Length)
        {
            steep = WalkWay.Length + (WalkWay.Length - waySteep - 1);
        }

        var pos = WalkWay[steep];

        SetPosition(pos);
        waySteep++;

        if (waySteep >= WalkWay.Length * 2)
        {
            waySteep = 0;
        }
    }
    private bool PlayerDetect()
    {
        var pPos = player.Position;
        var hUp = mapPosition + velocity;
        var hRight = hUp + new Vector2Int(velocity.y, velocity.x);
        var hLeft =  hUp + new Vector2Int(velocity.y, velocity.x) * -1;
        if (pPos == hUp || pPos == hRight || pPos == hLeft)
        {
            return true;
        }
        else
        {
            return false;
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
    private void OnDrawGizmos()
    {
        if (WalkWay != null && WalkWay.Length > 0)
        {
            var old = WalkWay[0];
            for (var i = 1; i < WalkWay.Length; i++)
            {
                var current = WalkWay[i];
                var fP = new Vector3(old.x, old.y, 0);
                var sP = new Vector3(current.x, current.y, 0);
                Gizmos.DrawLine(fP, sP);
                old = current;
            }
        }

    }
}
