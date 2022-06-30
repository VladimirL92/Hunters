using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterController : MonoBehaviour
{
    public Vector2Int[] PatrolWay;
    private Vector2Int[] attackWay;
    private Vector2Int mapPosition;
    private int waySteep;
    private float lastTime;
    private bool attack;
    [SerializeField]
    private float speed = 1f;
    private NoiseController noiseBar;
    private MapManager map;
    private PlayerController player;
    private SpriteRenderer hunterColor;
    private GameManage manager;
    private bool alarm;
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
        PatrolWay = map.FindWay(mapPosition, player.position);
        hunterColor.color = Color.red;
        if (PatrolWay.Length > 1)
        {
            SetPosition(PatrolWay[1]);
        }
        if ( mapPosition == player.position)
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
        if (PatrolWay.Length == 0)
        {
            return;
        }

        var steep = waySteep;
        if (waySteep >= PatrolWay.Length)
        {
            steep = PatrolWay.Length + (PatrolWay.Length - waySteep - 1);
        }

        var pos = PatrolWay[steep];

        SetPosition(pos);
        waySteep++;

        if (waySteep >= PatrolWay.Length * 2)
        {
            waySteep = 0;
        }
    }
    private bool PlayerDetect()
    {
        var pPos = player.position;
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
        if (PatrolWay != null && PatrolWay.Length > 0)
        {
            var old = PatrolWay[0];
            for (var i = 1; i < PatrolWay.Length; i++)
            {
                var current = PatrolWay[i];
                var fP = new Vector3(old.x, old.y, 0);
                var sP = new Vector3(current.x, current.y, 0);
                Gizmos.DrawLine(fP, sP);
                old = current;
            }
        }

    }
}
