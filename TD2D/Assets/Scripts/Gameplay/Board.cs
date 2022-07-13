using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public bool CreateLevelMode;
    private int xSize, ySize;
    public Tile tileGO;
    private int[] map; // 0 - ground, 1 - road, 2 - spawner, 3 - finish
    private const string groundTag = "Ground";
    private const string roadTag = "Road";
    private const string spawnerTag = "Spawner";
    private const string finishTag = "Finish";

    /*private GameObject prevRoadTile = null;
    private Vector2[] dirRay = new Vector2[] {Vector2.up, Vector2.down, Vector2.left, Vector2.right};
    public List<Vector3> wayPoints;*/

    public void SetValue(int xSize, int ySize, int[] map)
    {
        this.xSize = xSize;
        this.ySize = ySize;
        this.map = map;

        StartCoroutine(CreateBoard());
        //StartCoroutine(StartOrder());
    }

    /*IEnumerator StartOrder()
    {
        yield return StartCoroutine(CreateBoard());
        wayPoints = LoadWaypoints();
    }*/

    IEnumerator CreateBoard()
    {
        Tile[,] tileArr = new Tile[xSize, ySize];
        float xPos = transform.position.x;
        float yPos = transform.position.y;
        Vector2 tileSize = tileGO.spriteRenderer.bounds.size;

        int count = 0;
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                Tile newTile = Instantiate(tileGO, transform.position, Quaternion.identity);
                newTile.transform.position = new Vector3(xPos + (tileSize.x*x), yPos + (tileSize.y*y), 0);
                newTile.transform.parent = transform;
                newTile.name = count.ToString();
                if (!CreateLevelMode)
                    if (map[count] != 0)
                    {
                        newTile.spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
                        newTile.spriteRenderer.sortingLayerName = roadTag;
                        if (map[count] == 1)
                            newTile.tag = roadTag;
                        else
                            if (map[count] == 2)
                            {
                                newTile.tag = spawnerTag;
                                //prevRoadTile = newTile.gameObject;
                            }
                            else
                                newTile.tag = finishTag;
                    }
                    else
                        newTile.tag = groundTag;
                count++;

                tileArr[x, y] = newTile;
            }            
        }
        yield return null;
    }

    /*private List<Vector3> LoadWaypoints()
    { 
        List<Vector3> wayPointsVect = new List<Vector3>();
        List<GameObject> wayPoints = new List<GameObject>();
        if (!CreateLevelMode)
        {
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
        }
        gameObject.transform.Rotate(45.0f, 0.0f, 45.0f, Space.Self);
        foreach (GameObject go in wayPoints)
        {
            wayPointsVect.Add(go.transform.position);
        }

        return wayPointsVect;
    }

    public void CreateDecoration()
    {
        
        List<Sprite> tempSprite = new List<Sprite>();
        tempSprite.AddRange(tileSprite);
        
        tempSprite.Remove(cashSprite);
        if (x > 0)
        {
            tempSprite.Remove(tileArr[x-1, y].spriteRenderer.sprite); 
        }
        newTile.spriteRenderer.sprite = tempSprite[Random.Range(0, tempSprite.Count)];
        cashSprite = newTile.spriteRenderer.sprite;
    }*/
}
