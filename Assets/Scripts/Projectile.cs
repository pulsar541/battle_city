using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Projectile : BattleObject
{
    private Vector3 _direction;
    public float speed = 15.0f; 

    public int creatorInstanseID;
    private Tilemap tilemapWalls;
    [SerializeField] public TileBase tileBaseSteelWall;
    [SerializeField] public TileBase tileBaseBrickWall;
    [SerializeField] public TileBase tileBaseMapLimit;  
    [SerializeField] public GameObject canvasTempInfoPrefab; 

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
 
    public override void BattleObjectUpdate()
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

                    if (tileBase == tileBaseSteelWall || tileBase == tileBaseBrickWall || tileBase == tileBaseMapLimit)
                    {
                        wasCollide = true;
                    }
                } 
            }
        }
 
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 0.2f);
        foreach (Collider2D hitCollider in hitColliders)
        { 
            if(hitCollider.gameObject.GetInstanceID() != creatorInstanseID) 
            {
                if (hitCollider.name.IndexOf("Projectile") > -1 && hitCollider.gameObject.GetInstanceID() != this.gameObject.GetInstanceID()) {
                    hitCollider.gameObject.GetComponent<BattleObject>().Health = 0;
                    wasCollide = true;
                }
                else if (hitCollider.name.IndexOf("Enemy") > -1)
                {
                    if(!_isEnemy) 
                    {   Enemy enemy = hitCollider.gameObject.GetComponent<Enemy>();
                        enemy.Health -= 100; 
                        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                        foreach(GameObject player in players) {
                            if(player.GetInstanceID() == creatorInstanseID) {
                                player.GetComponent<Player>().UpdateScore(enemy.price);   
                                player.GetComponent<Player>().UpdateDestroyedTanksInfo(Tank.Type.TYPE_SIMPLE); 
                            }
                        } 
                        GameObject go = Instantiate(canvasTempInfoPrefab, transform.position, Quaternion.identity);
                    }
                    wasCollide = true;
                }  
                else if (hitCollider.name.IndexOf("Player") > -1)
                {
                    hitCollider.gameObject.GetComponent<BattleObject>().Health -= 100; 
                    wasCollide = true;
                } 
                else if (hitCollider.name.IndexOf("Base") > -1)
                {
                    hitCollider.gameObject.GetComponent<BattleObject>().Health -= 100; 
                    wasCollide = true;
                } 
            }
        }  

        if (wasCollide)
        {
            Health = 0;
        }
 
    }

    public override void OnZeroHealth()
    {
        base.OnZeroHealth();
        Destroy(this.gameObject);
    }
}
