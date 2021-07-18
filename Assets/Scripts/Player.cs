using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Player : Tank
{ 
    // Update is called once per frame
 
    [SerializeField] public TileBase tileBaseIce;

  
    public int selfNum = -1;

    public Vector3 spawnPosition = new Vector3();

    private float _shootInterval = 0;

    public int score = 0;

    void Start() {
        gameObject.name = "Player" + GetInstanceID().ToString();
        spawnPosition = transform.position;
        Lives = 3;
        score = 0;
        SceneController.UpdateBattleInformation();
    }

    public override void BattleObjectUpdate()
    {   

        if(SceneController.isGameOver)
            return; 

        Vector3Int cellWorldPosition = tilemapCollider.WorldToCell(transform.position);
       
        if ( _rigidbody.velocity.magnitude > 0.1f && tilemapCollider.HasTile(cellWorldPosition) && tilemapCollider.GetTile(cellWorldPosition) == tileBaseIce)
        {
            switch((Direction)lastDirection) {
                case Direction.DIR_LEFT: 
                    GoLeft(); 
                    break;
                case Direction.DIR_RIGHT: 
                    GoRight(); 
                    break;
                case Direction.DIR_UP: 
                    GoUp(); 
                    break;
                case Direction.DIR_DOWN: 
                    GoDown(); 
                    break;                                                
            } 
     
        }
        else  {
            if (Input.GetButton("Horizontal"))
            {   
                if(Input.GetAxis("Horizontal") < 0) 
                    GoLeft(); 
                else 
                    GoRight(); 
            }
            
            if (Input.GetButton("Vertical"))
            {              
                if(Input.GetAxis("Vertical") < 0) 
                    GoUp();
                else 
                    GoDown(); 
            }
        }
         
        if (Input.GetButton("Fire1")) { 
            if(_shootInterval >= 0.5f  || _shootInterval == 0) 
            {
                 Shoot();
                 if(_shootInterval > 0)
                    _shootInterval = 0;
            }
            _shootInterval += Time.deltaTime;
        }  
        if (Input.GetButtonUp("Fire1")) {
            _shootInterval = 0;
        } 
    }


    public override void OnZeroHealth() 
    {
        base.OnZeroHealth();  
        //transform.position = spawnPosition;
        Health = 100;
        _rigidbody.MovePosition(spawnPosition);      
    }
 
    public void PlusScore(int deltaScore) {
        score += deltaScore; 
    }
 
}
