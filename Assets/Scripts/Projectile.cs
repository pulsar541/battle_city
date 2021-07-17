using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Projectile : MonoBehaviour
{
    private Vector3 _direction;
    public float speed = 15.0f; 
    private Tilemap tilemapWalls;
    [SerializeField] public TileBase tileBaseSteelWall;
    [SerializeField] public TileBase tileBaseBrickWall;

    private bool _isEnemy = false;
    public bool IsEnemy {
        set {_isEnemy = value;}
        get {return _isEnemy;}
    }

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

    // Update is called once per frame
    void Update()
    {
        Vector3 movemont = transform.TransformDirection(speed * Time.deltaTime, 0, 0);     
        transform.position += movemont;

        bool wasCollide = false;
        float damageRadiusX = Mathf.Abs(movemont.x) < Mathf.Abs(movemont.y) ? damageRadius : 0;
        float damageRadiusY = Mathf.Abs(movemont.y) < Mathf.Abs(movemont.x) ? damageRadius : 0;

        for (float i = -damageRadiusX; i <= damageRadiusX; i += damageRadius * 0.5f)
        {
            for (float j = -damageRadiusY; j <= damageRadiusY; j += damageRadius * 0.5f)
            {
                Vector3Int cellWorldPosition = tilemapWalls.WorldToCell(transform.position + new Vector3(i, j, 0)); 

                if (tilemapWalls.HasTile(cellWorldPosition))
                {
                    TileBase tileBase = tilemapWalls.GetTile(cellWorldPosition);

                    if (tileBase == tileBaseBrickWall)
                    {
                        tilemapWalls.SetTile(cellWorldPosition, null);
                    }

                    if (tileBase == tileBaseSteelWall || tileBase == tileBaseBrickWall)
                    {
                        wasCollide = true;
                    }
                } 
            }
        }
 
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 0.2f);
        foreach (Collider2D hitCollider in hitColliders)
        { 
            if (hitCollider.name.IndexOf("Enemy") > -1 && !_isEnemy)
            {
                hitCollider.gameObject.GetComponent<Tank>().health -= 100; 
                wasCollide = true;
            }  
            else if (hitCollider.name.IndexOf("Player") > -1 && _isEnemy)
            {
                hitCollider.gameObject.GetComponent<Tank>().health -= 100; 
                wasCollide = true;
            } 
        }  

        if (wasCollide)
        {
            Destroy(this.gameObject);
        }
    }
}
