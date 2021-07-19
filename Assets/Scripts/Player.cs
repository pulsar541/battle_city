using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Player : Tank
{ 
    // Update is called once per frame
 
    [SerializeField] public TileBase tileBaseIce;
 
    public int selfIndex = -1;

    public Vector3 spawnPosition = new Vector3();

    public float _shootInterval = 0; 
    

    void Start() {
        gameObject.name = "Player" + GetInstanceID().ToString();
        spawnPosition = transform.position;
        Lives = 3; 
        LevelController.UpdateBattleInformation();
    }

    public void Init(int index) 
    {
        selfIndex = index;
        Global.score[index] = 0;
        for(int i = 0; i<(int)Tank.Type.MAX_TYPES; i++)
            Global.destroyedTankTypesCounter[selfIndex, i] = 0;
    }

    

    public override void BattleObjectUpdate()
    {   
 
        if(LevelController.isGameOver)
            return; 

        Vector3Int cellWorldPosition = tilemapCollider.WorldToCell(transform.position);
        

        bool onIce = false;
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
            onIce = true;
        } 
              
        if(GetComponent<InputsPlayer>() != null)
            GetComponent<InputsPlayer>().InputsUpdate(onIce);   

    }


    public override void OnZeroHealth() 
    {
        base.OnZeroHealth();  
        //transform.position = spawnPosition;
        Health = 100;
        _rigidbody.MovePosition(spawnPosition);      
    }
 
    public void UpdateScore(int deltaScore) {
        if(selfIndex >=0 && selfIndex < Global.MaxPlayersCount)
            Global.score[selfIndex] += deltaScore; 
    }
    public void UpdateDestroyedTanksInfo(Tank.Type type) {  
        if((int)type >=0 && (int)type < (int)Tank.Type.MAX_TYPES)
            Global.destroyedTankTypesCounter[selfIndex, (int)type] ++;
    } 
}
