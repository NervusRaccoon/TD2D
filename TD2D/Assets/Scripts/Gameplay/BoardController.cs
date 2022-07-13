using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public ShopController shopController;
    private int xSize, ySize;
    private List<Sprite> tileSprite = new List<Sprite>();
    private Tile[,] tileArr;
    private Tile oldSelectionTile = null;
    private Vector2[] dirRay = new Vector2[] {Vector2.up, Vector2.down, Vector2.left, Vector2.right};
    private const string groundTag = "Ground";
    public GameObject towerPref;
    public Transform towerParent;
    public bool openShop = false;
    
    public void SetValue(Tile[,] tileArr, int xSize, int ySize, List<Sprite> tileSprite)
    {
        this.tileArr = tileArr;
        this.xSize = xSize;
        this.ySize = ySize;
        this.tileSprite = tileSprite;
    }

    private void Select(Tile tile)
    {
        if (oldSelectionTile != tile)
        {
            //tile.isSelected = true;
            tile.spriteRenderer.color = new Color(0.8f, 0.8f, 0.8f);
            if (oldSelectionTile != null)
                Deselect(oldSelectionTile);
            oldSelectionTile = tile;
        }
    }

    private void Deselect(Tile tile)
    {
        tile.spriteRenderer.color = new Color(1f, 1f, 1f);
        oldSelectionTile = null;
    }

    void Update()
    {
        RaycastHit2D ray = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
        if (ray != false)
        {
            if (ray.collider.gameObject.tag == groundTag)
            {
                if (!openShop)
                    Select(ray.collider.GetComponent<Tile>());
                if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    Vector2 pos = new Vector2(ray.collider.gameObject.transform.position.x, ray.collider.gameObject.transform.position.y+0.6f);
                    openShop = true;
                    shopController.OpenCloseShop(pos);
                }
            }
        }
    }
}
