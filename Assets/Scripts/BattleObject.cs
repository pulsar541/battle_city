using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleObject : MonoBehaviour
{ 
    
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] public GameObject explosionPrefab;

    private int _health = 100;

    public int Health {
        get{return _health;}
    }

    public int Lives = 1;

    public virtual void BattleObjectUpdate() {}  
    public virtual void OnZeroHealth()  {}    
    public virtual void OnZeroLives() {} 
  
    public virtual void ChangeHealth(int deltaHealth) 
    {
        SetHealth(_health + deltaHealth);
    }
    public virtual void SetHealth(int health) 
    {
        _health = health;
        if(_health <= 0 && Lives > 0)
        {    
            OnZeroHealth();
            Lives --;
            if(Lives <= 0)
            {  
                OnZeroLives();
            }  
        } 
    }    
  




    void Update() 
    {
        if(LevelController.isPause)
            return;
            
        BattleObjectUpdate();  
    }
}
