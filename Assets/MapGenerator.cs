using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("MapSetting")]
    [SerializeField]
    private float widthCell = 1;
    [SerializeField]
    private float heightCell = 1;
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
    private GameObject hunterTemplate;
    [SerializeField]
    private int hunterCount = 2;



    private void Start()
    {
        MapGeneration();
        SpawnPlayer_exit();
        SpawnHunters();
    }

    void SpawnHunters()
    {
        for( var i = 0; i < hunterCount; i++)
        {
            var x = Random.Range(0, cellXCount - 3) - ((cellXCount - 2) / 2 - 0.5f) * widthCell;
            var y = Random.Range(0, cellYCount - 3) - ((cellYCount - 2) / 2 - 0.5f) * heightCell;
            var hunter = Instantiate(hunterTemplate, transform);
            hunter.transform.position = new Vector3(x, y, 0);

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
                    var cell = Instantiate(wallTemplate, transform);
                    var xTemp = ((x * widthCell) - (cellXCount / 2)) +0.5f;
                    var yTemp = ((y * heightCell) - (cellYCount / 2)) +0.5f;
                    cell.transform.localPosition = new Vector3(xTemp,yTemp,0);

                }
                else if (Random.Range(0, randomCount) == 0)
                {
                    var cell = Instantiate(wallTemplate, transform);
                    var xTemp = ((x * widthCell) - (cellXCount / 2)) + 0.5f;
                    var yTemp = ((y * heightCell) - (cellYCount / 2)) + 0.5f;
                    cell.transform.localPosition = new Vector3(xTemp, yTemp, 0);
                }
            }
        }
    }
    void Generate()
    {
        for(var x=0; x < cellXCount; x++)
        {
            for (var y = 0; y < cellYCount; y++)
            {
                if (x == 0 || y == 0 || x == cellXCount - 1 || y == cellYCount - 1)
                {
                    var cell = Instantiate(wallTemplate,transform);
                    cell.transform.localPosition = new Vector3(x * widthCell, y * heightCell, 0);

                }
                else if (Random.Range(0,randomCount) == 0)
                {
                    var cell = Instantiate(wallTemplate, transform);
                    cell.transform.localPosition = new Vector3(x * widthCell, y * heightCell, 0);
                }
            }
        }
    }
    void GeneratePoints()
    {
        var countX = cellYCount -1;
        var countY = cellXCount -1;

        var startX = 1;
        var startY = 1;

        var startCell = Instantiate(playerTemplate, transform);
        var exitCell = Instantiate(exitTemplate, transform);

        var index = Random.Range(0, countX * 2 + (countY - 2) * 2);
        var xq = startX;
        var yq = startY;

        if (index < countX)
        {
            xq = index;
            yq = startY;
        }
        else if (index < countX * 2)
        {
            xq = index - countX;
            yq = countY - 1;
        }
        else if (index < countX * 2 + countY - 2)
        {
            xq = startX;
            yq = index - countX * 2;
        }
        else
        {
            xq = countX - 1;
            yq = index - (countX * 2 + (countY - 2));
        }

        startCell.transform.localPosition = new Vector3(xq * widthCell, yq * heightCell, -1);
        var xt = countX - xq - 1 + startX;
        var yt = countY - yq - 1 + startY;
        exitCell.transform.localPosition = new Vector3(xt * widthCell, yt * heightCell, -1);
    }
    void SpawnPlayer_exit()
    {
        var startCell = Instantiate(playerTemplate, transform);
        var exitCell = Instantiate(exitTemplate, transform);

        var x = Random.Range(0, cellXCount - 3);
        var y = 0;

        if (x > 0 && x < cellXCount - 3)
        {

        }
        else
        {
            y = Random.Range(0, cellYCount - 3);
        }

        Debug.Log(x + " Xcoord  " + y + "Ycoord");

        //var xTemp = ((x * widthCell) - (cellXCount / 2)) + 0.5f;
        //var yTemp = ((y * heightCell) - (cellYCount / 2)) + 0.5f;

        var xTemp = x - ((cellXCount - 2)/ 2 - 0.5f) * widthCell;
        var yTemp = y - ((cellYCount - 2) / 2 - 0.5f) * heightCell;
        startCell.transform.localPosition = new Vector3(xTemp, yTemp, -1);
        xTemp = -xTemp;
        yTemp = -yTemp;
        exitCell.transform.localPosition = new Vector3(xTemp, yTemp, -1);
    }


}
