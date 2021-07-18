using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : BattleObject
{

    int aliveStatus;
    void Start()
    {
        aliveStatus = 1;
        UpdateSkin();

    }
    void Update()
    {
        if (aliveStatus == 1)
        {
            if (Health <= 0)
            {
                aliveStatus = 0; 
                UpdateSkin();
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }
        }  
    }
 
    void UpdateSkin()
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            child.GetComponent<Renderer>().enabled =  (i == 1 - aliveStatus);
            i++;
        }   
    }
 
}
