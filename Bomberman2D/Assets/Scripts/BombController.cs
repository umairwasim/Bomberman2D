using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
    [Header("Bomb")]
    [SerializeField] private KeyCode bombKey = KeyCode.Space;
    public GameObject bombPrefab;
    public float bombFuseTime = 4f;
    public int bombAmount = 1;

    [Header("Explosion")]
    public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    [Header("Destructible")]
    public Tilemap destructibleTilemap;
    public Destructible destructiblePrefab;

    private int remainingBombsCounter;
    private const string BOMB = "Bomb";

    private void Start()
    {
        remainingBombsCounter = bombAmount;
    }

    private void Update()
    {
        if (Input.GetKeyDown(bombKey) && remainingBombsCounter > 0)
        {
            StartCoroutine(PlaceBomb());
        }
    }

    IEnumerator PlaceBomb()
    {
        Vector2 position = transform.position; //set bomb position equals to player position
        position.x = Mathf.Round(position.x); //Round x and y so that it always spawns at the center of the tile
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        remainingBombsCounter--;
        yield return new WaitForSeconds(bombFuseTime);

        //Explosion
        position = bomb.transform.position; // set explosion position equals to bomb's position
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        //Instantiate the center explosion prefab and sprites
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveSpriteRenderer(explosion.start);
        explosion.DestroyAfterSeconds(explosionDuration);


        //Call explode for each direction at that position
        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        //Explode in each direction

        Destroy(bomb);
        remainingBombsCounter++;
    }

    //It is a recursive function it will keep on calling for each direction unless the exit condition is met
    public void Explode(Vector2 position, Vector2 direction, int length)
    {
        //exit condition
        if (length <= 0)
            return;

        //incerement position to one unit direction (whichever it is that we called)
        position += direction;

        //check if it overlaps with a stage tile, then return and do not instantiate a sprite on that tile
        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            DestroyDestructible(position);
            return;
        }

        //Explosion middle/end logic for each tile
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveSpriteRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);
        explosion.DestroyAfterSeconds(explosionDuration);

        //call the recursive function with reduced length 
        Explode(position, direction, length - 1);
    }

    private void DestroyDestructible(Vector2 position)
    {
        //convert world pos to cell position
        Vector3Int cell = destructibleTilemap.WorldToCell(position);

        //get that tile at cell position
        TileBase tileBase = destructibleTilemap.GetTile(cell);

        if (tileBase)
        {
            Instantiate(destructiblePrefab, position, Quaternion.identity);

            //set the tile at cell position to null
            destructibleTilemap.SetTile(cell, null);
        }
    }

    /// <summary>
    /// Set Trigger to false whew player exits it so that it can be a collider
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(BOMB))
            collision.isTrigger = false;
    }
}
