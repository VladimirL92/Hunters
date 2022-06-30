using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    [Header("MapSetting")]
    [SerializeField]
    private float sizeCell = 1;
    [SerializeField]
    private int cellXCount = 12;
    [SerializeField]
    private int cellYCount = 12;
    [SerializeField]
    private int randomCount = 5;

    [Header("Templates")]
    [SerializeField]
    private GameObject wallTemplate;
    [SerializeField]
    private GameObject playerTemplate;
    [SerializeField]
    private GameObject exitTemplate;
    [SerializeField]
    private HunterMover hunterTemplate;
    [SerializeField]
    private int hunterCount = 2;

    private GameObject[,] map;



    private void Start()
    {
        map = new GameObject[cellXCount, cellYCount];
        var x = cellXCount / 2 * sizeCell - (sizeCell / 2);
        var y = cellYCount / 2 * sizeCell - (sizeCell / 2);
        Camera.main.transform.localPosition = new Vector3(x, y, -10);

        MapGeneration();
        MainWayGeneration();
        SpawnHunters();
        MapView();
    }
    public bool IsWall(Vector2Int point)
    {
        return map[point.x, point.y] == wallTemplate;
    }
    public Vector2Int [] FindWay(Vector2Int start, Vector2Int  end)
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
                var count = 1;

                if (k.TryGetValue(p,out var v))
                {
                    count = v.count + 1;
                }

                var up = p + Vector2Int.up;
                var down = p + Vector2Int.down;
                var left = p + Vector2Int.left;
                var right = p + Vector2Int.right;

                if (k.TryGetValue(up,out var upV))
                {
                    if (count < upV.count)
                        k[up] = (count, p);
                }
                else
                {
                    k[up] = (count, p);
                    stack.Push(up);
                }

                if (k.TryGetValue(down, out var downV))
                {
                   if (count < downV.count)
                        k[down] = (count, p);
                }
                else
                {
                    k[down] = (count, p);
                    stack.Push(down);
                }

                if (k.TryGetValue(left, out var leftV))
                {
                    if (count < leftV.count)
                        k[left] = (count, p);
                }
                else
                {
                    k[left] = (count, p);
                    stack.Push(left);
                }

                if (k.TryGetValue(right, out var rightV))
                {
                    if (count < rightV.count)
                        k[right] = (count, p);
                }
                else
                {
                    k[right] = (count, p);
                    stack.Push(right);
                }
            }
        }

        var curent = start;
        var way = new List<Vector2Int>();

        while (k.ContainsKey(curent) && k[curent].count > 0)
        {
            way.Add(curent);
            curent = k[curent].point;
        }
        way.Add(end);
        return way.ToArray();
    } 
    public bool TryFindFreePoint(out Vector2Int point)
    {
        for (var t = 0; t < 10; t++)
        {
            var x = Random.Range(1, cellXCount - 2);
            var y = Random.Range(1, cellYCount - 2);
            if (map[x, y] == null)
            {
                point =  new Vector2Int(x, y);
                return true;
            }
        }

        for (var x = 0; x < cellXCount; x++)
        {
            for (var y = 0; y < cellYCount; y++)
            {
                if (map[x, y] == null)
                {
                    point = new Vector2Int(x, y);
                    return true;
                }
            }
        }

        point = new Vector2Int();
        return false;
    }
    void SpawnHunters()
    {
        for (var i = 0; i < hunterCount; i++)
        {
            if (TryFindFreePoint(out var point))
            {
                var hunter = Instantiate(hunterTemplate, transform);
                hunter.SetCellPosition(point);
                hunter.FindWay();
            }
        }


    }
    void MapGeneration()
    {
        for (var x = 0; x < cellXCount; x++)
        {
            for (var y = 0; y < cellYCount; y++)
            {
                if (x == 0 || y == 0 || x == cellXCount - 1 || y == cellYCount - 1)
                {
                    map[x, y] = wallTemplate;
                }
                else if (Random.Range(0, randomCount) == 0)
                {
                    map[x, y] = wallTemplate;
                }
            }
        }
    }
    void MainWayGeneration()
    {
        var startP = new Vector2Int(1, 1);
        var endP = new Vector2Int(cellXCount - 2, cellYCount - 2);

        var xSign = System.Math.Sign(endP.x - startP.x);
        var ySign = System.Math.Sign(endP.y - startP.y);

        var spy = startP.y;
        for (var xi = startP.x; xi <= endP.x ; xi += 1 * xSign)
        {
            if (Random.Range(0,4) == 0 || xi == endP.x)
            {
                for (var yi = spy; yi <= endP.y; yi += 1 * ySign)
                {
                    map[xi, yi] = null;
                    spy = yi;
                    if (Random.Range(0,4) == 0 && xi < endP.x)
                    {
                        break;
                    }
                }
            }
            map[xi, spy] = null;
        }

        map[startP.x, startP.y] = playerTemplate;
        map[endP.x, endP.y] = exitTemplate;
    }
    void MapView()
    {
        for (var x = 0; x < cellXCount; x++)
        {
            for (var y = 0; y < cellYCount; y++)
            {
                if (map[x,y] != null)
                {
                    var title = Instantiate(map[x, y], transform);
                    title.transform.localPosition = new Vector3(x, y, 0);
                }

            }
        }
    }


}
