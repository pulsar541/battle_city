using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Projectile : MonoBehaviour
{
    private Vector3 _direction;
    public float speed = 15.0f;
    private Camera mainCamera;
    private Tilemap tilemapWalls;
    [SerializeField] public TileBase tileBaseSteelWall;
    [SerializeField] public TileBase tileBaseBrickWall;


    public float damageRadius = 0.3f;

    public Vector3 Direction
    {
        set
        {
            _direction = value;
            // transform.rotation
        }
    }
    // Start is called before the first frame update

    void Awake()
    {
        tilemapWalls = GameObject.Find("Tilemap").GetComponent<Tilemap>();
    }
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.TransformDirection(speed * Time.deltaTime, 0, 0);

        bool wasCollide = false;

        for (float i = -damageRadius; i <= damageRadius; i += damageRadius * 0.5f)
        {
            for (float j = -damageRadius; j <= damageRadius; j += damageRadius * 0.5f)
            {
                Vector3 cellWorldPosition = tilemapWalls.WorldToCell(transform.position + new Vector3(i, j, 0));
                Vector3Int cellRoundWorldPosition = Vector3Int.RoundToInt(cellWorldPosition);

                if (tilemapWalls.HasTile(cellRoundWorldPosition))
                {
                    TileBase tileBase = tilemapWalls.GetTile(cellRoundWorldPosition);

                    if (tileBase == tileBaseBrickWall)
                    {
                        tilemapWalls.SetTile(cellRoundWorldPosition, null);
                    }

                    if (tileBase == tileBaseSteelWall || tileBase == tileBaseBrickWall)
                    {
                        wasCollide = true;
                    }
                }

            }
        }

        if (wasCollide)
        {
            Destroy(this.gameObject);
        }

    }
}
