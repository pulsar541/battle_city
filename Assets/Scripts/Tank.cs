using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tank : MonoBehaviour
{
    public float health = 100;
    public float baseSpeed = 6; 
    protected Rigidbody2D _rigidbody;

 
    protected Tilemap tilemapCollider;

    int spriteChildEnableNum = 0;
 
    [SerializeField] GameObject projectilePrefab;
    [SerializeField]  GameObject explosionPrefab;

    private Quaternion _childLocalRotation = new Quaternion();
    private Vector2 _movement = new Vector3();   
    
    public int lastDirection = (int)Direction.STOP; 

    
    protected enum Direction {
        STOP = -1,
        DIR_LEFT = 0,
        DIR_RIGHT = 1,   
        DIR_UP = 2,
        DIR_DOWN = 3,  
    }

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();  
        tilemapCollider = GameObject.Find("Tilemap").GetComponent<Tilemap>();
    }

  


    public void GoLeft() {
        _movement = new Vector3(-baseSpeed, 0);
        _childLocalRotation =  Quaternion.Euler (0, 0, 0); 
        lastDirection = (int)Direction.DIR_LEFT;          
    }
    public void GoRight() {
        _movement = new Vector3(baseSpeed, 0);
        _childLocalRotation =  Quaternion.Euler (0, 0, 180);
        lastDirection = (int)Direction.DIR_RIGHT;           
    }
    public void GoUp() {
        _movement = new Vector3(0, -baseSpeed); 
        _childLocalRotation =  Quaternion.Euler (0, 0, 90);
        lastDirection = (int)Direction.DIR_UP;            
    }
    public void GoDown() {
        _movement = new Vector3(0, baseSpeed); 
        _childLocalRotation =  Quaternion.Euler (0, 0, 270); 
        lastDirection = (int)Direction.DIR_DOWN;            
    }   

    // Update is called once per frame
    public void FixedUpdate()
    {  
        foreach (Transform child in transform) 
        { 
            child.localRotation =  _childLocalRotation;
        }

        _movement *= Time.fixedDeltaTime;  
        _rigidbody.AddForce(_movement, ForceMode2D.Impulse); 
        _rigidbody.AddRelativeForce(_movement - _rigidbody.velocity);

        if(_rigidbody.velocity.magnitude > 0.1f)
            spriteChildEnableNum = 1 - spriteChildEnableNum;
 
        int i = 0;
        foreach (Transform child in transform) 
        {  
            child.GetComponent<Renderer>().enabled = spriteChildEnableNum == i;
            i++; 
        } 

        if(health <=0) {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public void Shoot(bool isEnemyProjectile = false) { 
        Vector3 projectilePosition = transform.position; 
        GameObject projectile = Instantiate(projectilePrefab, projectilePosition, _childLocalRotation * Quaternion.Euler(0,0,180.0f)); 
        projectile.GetComponent<Projectile>().IsEnemy = isEnemyProjectile;
    }

}
