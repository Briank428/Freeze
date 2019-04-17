using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class TiledBackground : MonoBehaviour
{
    public GameObject tileObject;
    private BoxCollider2D canvasCollider;

    void Start()
    {
        canvasCollider = GetComponent<BoxCollider2D>();
        DrawTiledBackground();
    }

    void DrawTiledBackground()
    {
        Vector2 canvasSize = canvasCollider.bounds.size;

        // instantiate one tile to measure its size, then destroy it
        var templateTile = Instantiate(tileObject, Vector2.zero, Quaternion.identity) as GameObject;
        Vector2 tileSize = templateTile.GetComponent<Renderer>().bounds.size;

        float tilesX = canvasSize.x / tileSize.x;
        float tilesY = canvasSize.y / tileSize.y;
        Destroy(templateTile);

        // start placing tiles from the bottom left
        Vector2 bottomLeft = new Vector2(canvasCollider.transform.position.x - (canvasSize.x / 2), canvasCollider.transform.position.y - (canvasSize.y / 2));

        for (int i = 0; i < tilesX; i++)
        {
            for (int j = 0; j < tilesY; j++)
            {
                var newTilePos = new Vector3(bottomLeft.x + i * tileSize.x, bottomLeft.y + tileSize.y * j, 5f);
                var newTile = Instantiate(tileObject, newTilePos, Quaternion.identity) as GameObject;
                newTile.transform.parent = transform;
            }
        }

        // turn the template's renderer off
        tileObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}