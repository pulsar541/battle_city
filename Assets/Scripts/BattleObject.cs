using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleObject : MonoBehaviour
{ 
    
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] public GameObject explosionPrefab;

    public int Health = 100;

    public int Lives = 1;

    public virtual void BattleObjectUpdate() {}  
    public virtual void OnZeroHealth() {}    
    public virtual void OnZeroLives() {} 
    void Update() 
    {
        BattleObjectUpdate();

        if(Health <= 0 && Lives > 0)
        {   
            Lives --;
            OnZeroHealth();
        } 

        if(Lives <= 0)
        {  
            OnZeroLives();
        } 
    }
}
