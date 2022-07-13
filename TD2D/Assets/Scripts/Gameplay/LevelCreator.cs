using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour /*позволяет создать вручную уровень. Как использовать: в Board поставить флажок в пункте "CreateLevelMode"
                                            Запустить, нарисовать дорогу, выделяя tile. Первый выделенный tile будет выделен как спавнер мобов
                                            После того, как дорога нарисована нужно нажать второй раз на последний выделенный tile, это будет считаться
                                            финишем. После чего в логи будет выписан массив, который нужно подставить в map в скрипте Board */
{
    private Tile firstSelectionTile = null;
    private Tile lastSelectionTile = null;
    private List<Vector3> wayPoints;
    private GameObject prevRoadTile = null;
    private Vector2[] dirRay = new Vector2[] {Vector2.up, Vector2.down, Vector2.left, Vector2.right};
    private const string groundTag = "Ground";
    private const string roadTag = "Road";
    private const string spawnerTag = "Spawner";
    private const string finishTag = "Finish";

    private void Select(Tile tile)
    {
        if (firstSelectionTile == null)
        {
            firstSelectionTile = tile;
            prevRoadTile = tile.gameObject;
        }
        if (tile.isSelected)
        {
            lastSelectionTile = tile;
            BuildRoad();
            wayPoints = LoadWaypoints();
        }
        else
        {
            tile.isSelected = true;
            tile.spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    private void BuildRoad()
    {
        string str = "[";
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Tile>().isSelected)
            {
                if (child.gameObject.GetComponent<Tile>() == firstSelectionTile)
                    str = str + ", 2";
                else 
                    if (child.gameObject.GetComponent<Tile>() == lastSelectionTile)
                        str = str + ", 3";
                    else
                        str = str + ", 1";
            }
            else
                str = str + ", 0"; 
        }
        str = str + "]";
        Debug.Log(str); 
    }

    private List<Vector3> LoadWaypoints()
    { 
        List<Vector3> wayPointsVect = new List<Vector3>();
        List<GameObject> wayPoints = new List<GameObject>();
        wayPoints.Add(prevRoadTile);
        bool finish = false;
        while (!finish)
            for (int i = 0; i < dirRay.Length; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(prevRoadTile.transform.position.x, prevRoadTile.transform.position.y)
                                                    + prevRoadTile.GetComponent<BoxCollider2D>().bounds.size.magnitude*dirRay[i], dirRay[i]); 
                if (hit.collider != null)
                {        
                    GameObject tile = hit.collider.gameObject;
                    if (tile != prevRoadTile)
                    {
                        if (tile.tag == roadTag)
                        {
                            bool usedPoint = false;
                            foreach (GameObject go in wayPoints)
                                if (tile == go)
                                    usedPoint = true;
                            if (!usedPoint)
                            {
                                wayPoints.Add(tile);
                                prevRoadTile = tile;
                            }
                        }
                        else
                            if (tile.tag == finishTag)
                            {
                                wayPoints.Add(tile);
                                finish = true;
                            }
                    }
                }
            }

        foreach (GameObject go in wayPoints)
        {
            wayPointsVect.Add(go.transform.position);
        }

        return wayPointsVect;
    }


    void Update()
    {
        if (this.GetComponent<Board>().CreateLevelMode)
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D ray = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
                if (ray != false)
                {
                    Select(ray.collider.GetComponent<Tile>());
                }
            }
    }
}
