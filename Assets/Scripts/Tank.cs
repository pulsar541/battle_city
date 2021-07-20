using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tank : BattleObject
{
    public float baseSpeed = 6;
    protected Rigidbody2D _rigidbody; 

    protected Tilemap tilemapCollider;

    int spriteChildEnableNum = 0;
 
    private Quaternion _childLocalRotation = new Quaternion();
    private Vector2 _movement = new Vector3();

    public int lastDirection = (int)Direction.STOP;


    public enum Type
    {
        TYPE_SIMPLE = 0,
        TYPE_BTR, 
        MAX_TYPES
    }

    protected enum Direction
    {
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

    public virtual void GoLeft()
    {
        _movement = new Vector3(-baseSpeed, 0);
        _childLocalRotation = Quaternion.Euler(0, 0, 0);
        lastDirection = (int)Direction.DIR_LEFT;
    }
    public virtual void GoRight()
    {
        _movement = new Vector3(baseSpeed, 0);
        _childLocalRotation = Quaternion.Euler(0, 0, 180);
        lastDirection = (int)Direction.DIR_RIGHT;
    }
    public virtual void GoUp()
    {
        _movement = new Vector3(0, baseSpeed);
        _childLocalRotation = Quaternion.Euler(0, 0, 270);
        lastDirection = (int)Direction.DIR_UP;
    }
    public virtual void GoDown()
    {
        _movement = new Vector3(0, -baseSpeed);
        _childLocalRotation = Quaternion.Euler(0, 0, 90);
        lastDirection = (int)Direction.DIR_DOWN;
    }

    // Update is called once per frame
    public void FixedUpdate()
    { 
        foreach (Transform child in transform)
        {
            child.localRotation = _childLocalRotation;
        }

        _movement *= Time.fixedDeltaTime;
        _rigidbody.AddForce(_movement, ForceMode2D.Impulse);
        _rigidbody.AddRelativeForce(_movement - _rigidbody.velocity);

        if (_rigidbody.velocity.magnitude > 0.1f)
            spriteChildEnableNum = 1 - spriteChildEnableNum;

        int i = 0;
        foreach (Transform child in transform)
        {
            if(i > 1) 
                break;
            child.GetComponent<Renderer>().enabled = spriteChildEnableNum == i; 
            i++;
        }
    }

    public virtual void Shoot(bool isEnemyProjectile = false)
    {
        Vector3 projectilePosition = transform.position;
        GameObject projectile = Instantiate(projectilePrefab, projectilePosition, _childLocalRotation * Quaternion.Euler(0, 0, 180.0f));
        projectile.GetComponent<Projectile>().IsEnemy = isEnemyProjectile;
        projectile.GetComponent<Projectile>().creatorInstanseID = this.gameObject.GetInstanceID();
    }
 
    public override void OnZeroHealth()
    {  
        
        LevelController.UpdateBattleInformation();
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        //Destroy(this.gameObject); 
    }

    public override void OnZeroLives()
    {
        Destroy(this.gameObject); 
    }    

}
