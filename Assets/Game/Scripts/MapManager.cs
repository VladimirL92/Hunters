using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapManager : MonoBehaviour
{
    [Header("Map Setting")]
    [HideInInspector]
    public GameObject[,] Map;
    readonly int cellXCount = 12;
    readonly int cellYCount = 12;
    public float cellSize = 1;

    [Header("Templates")]
    public GameObject PlayerTemplate;
    public GameObject FinishTemplate;
    public HunterController HunterTemplate;
    public GameObject WallTemplate;

    public Vector2Int StartPoint;
    public Vector2Int FinishPoint;
    [SerializeField]
    private int HunterQuantity = 2;
    [SerializeField]
    private int wallFilling = 4;

    private void Start()
    {
        StartPoint = new Vector2Int(1, 1);
        FinishPoint = new Vector2Int(cellXCount - 2,cellYCount - 2);

        Map = new GameObject[cellXCount, cellYCount];
        CameraAlignMap();
        MapGeneration();
        PlayerWayGeneration(StartPoint,FinishPoint);
        CharacterSpawn();
        MapView();
    }
    void CameraAlignMap()
    {
        var x = cellXCount / 2 * cellSize - (cellSize / 2);
        var y = cellYCount / 2 * cellSize - (cellSize / 2);
        Camera.main.transform.localPosition = new Vector3(x, y, -10);
    }
    void MapGeneration()
    {
        for (var x = 0; x < cellXCount; x++)
        {
            for (var y = 0; y < cellYCount; y++)
            {
                if (x == 0 || y == 0 || x == cellXCount - 1 || y == cellYCount - 1)
                {
                    Map[x, y] = WallTemplate;
                }
                else if (Random.Range(0, wallFilling) == 0)
                {
                    Map[x, y] = WallTemplate;
                }
            }
        }
    }
    void PlayerWayGeneration(Vector2Int start, Vector2Int finish)
    {
        var xSign = System.Math.Sign(finish.x - start.x);
        var ySign = System.Math.Sign(finish.y - start.y);

        var spy = start.y;
        for (var xi = start.x; xi <= finish.x; xi += 1 * xSign)
        {
            if (Random.Range(0, 4) == 0 || xi == finish.x)
            {
                for (var yi = spy; yi <= finish.y; yi += 1 * ySign)
                {
                    Map[xi, yi] = null;
                    spy = yi;
                    if (Random.Range(0, 4) == 0 && xi < finish.x)
                    {
                        break;
                    }
                }
            }
            Map[xi, spy] = null;
        }
        Map[finish.x, finish.y] = FinishTemplate;
    }
    public Vector2Int[] FindWay(Vector2Int start, Vector2Int end)
    {
        var k = new Dictionary<Vector2Int, (int count, Vector2Int point)>();
        var stack = new Stack<Vector2Int>();

        k.Add(end, (0, end));
        stack.Push(end);

        while (stack.Count > 0)
        {
            var p = stack.Pop();
            if (!IsWall(p))
            {
                var count = 0;

                if (k.TryGetValue(p, out var v))
                {
                    count = v.count;
                }
                count++;
                var up = p + Vector2Int.up;
                var down = p + Vector2Int.down;
                var left = p + Vector2Int.left;
                var right = p + Vector2Int.right;

                if (k.TryGetValue(up, out var upV))
                {
                    if (count <= upV.count)
                    {
                        k[up] = (count, p);
                        stack.Push(up);
                    }
                }
                else if (!IsWall(up))
                {
                    k[up] = (count, p);
                    stack.Push(up);
                }

                if (k.TryGetValue(down, out var downV))
                {
                    if (count <= downV.count)
                    {
                        k[down] = (count, p);
                        stack.Push(down);
                    }
                }
                else if(!IsWall(down))
                {
                    k[down] = (count, p);
                    stack.Push(down);
                }

                if (k.TryGetValue(left, out var leftV))
                {
                    if (count <= leftV.count)
                    {
                        k[left] = (count, p);
                        stack.Push(left);
                    }
                }
                else if (!IsWall(left))
                {
                    k[left] = (count, p);
                    stack.Push(left);
                }

                if (k.TryGetValue(right, out var rightV))
                {
                    if (count <= rightV.count)
                    {
                        k[right] = (count, p);
                        stack.Push(right);
                    }
                }
                else if (!IsWall(right))
                {
                    k[right] = (count, p);
                    stack.Push(right);
                }
            }
            else
            {
                k.Remove(p);
            }
        }

        var curent = start;
        var way = new List<Vector2Int>();
        debugWay = k;
        while (k.ContainsKey(curent) && k[curent].count > 0)
        {
            way.Add(curent);
            curent = k[curent].point;
        }
        way.Add(end);
        return way.ToArray();
    }
    public void CharacterSpawn()
    {
     Instantiate(PlayerTemplate, transform);

        for (var i = 0; i < HunterQuantity; i++)
        {
            if (TryFindFreePoint(out var point))
            {
                var hunter = Instantiate(HunterTemplate, transform);
                hunter.SetPosition(point);
                TryFindFreePoint(out var endPoint);
                hunter.PatrolWay = FindWay(point, endPoint);
            }
        }

    }
    public bool IsWall(Vector2Int point)
    {
        return Map[point.x, point.y] == WallTemplate;
    }
    public bool TryFindFreePoint(out Vector2Int point)
    {
        for (var t = 0; t < 10; t++)
        {
            var x = Random.Range(1, cellXCount - 2);
            var y = Random.Range(1, cellYCount - 2);
            if (Map[x, y] == null)
            {
                point = new Vector2Int(x, y);
                return true;
            }
        }

        for (var x = 0; x < cellXCount; x++)
        {
            for (var y = 0; y < cellYCount; y++)
            {
                if (Map[x, y] == null)
                {
                    point = new Vector2Int(x, y);
                    return true;
                }
            }
        }

        point = new Vector2Int();
        return false;
    }
    void MapView()
    {
        for (var x = 0; x < cellXCount; x++)
        {
            for (var y = 0; y < cellYCount; y++)
            {
                if (Map[x, y] != null)
                {
                    var title = Instantiate(Map[x, y], transform);
                    title.transform.localPosition = new Vector3(x, y, 0);
                }

            }
        }
    }

    private Dictionary<Vector2Int, (int count, Vector2Int point)> debugWay;

    private void OnDrawGizmos()
    {
        if ( debugWay != null)
        {
            foreach (var current in debugWay)
            {
                var count = current.Value.count;
                var point = current.Value.point;
                var p = current.Key;
                var position = new Vector3(p.x,p.y,0);
                var style = new GUIStyle { alignment = TextAnchor.MiddleCenter };
                Handles.Label(position, "" + count, style);
            }
        }

    }
}
