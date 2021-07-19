using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsPlayerTwo : InputsPlayer
{
     Tank tank;
    float _shootInterval;
    // Start is called before the first frame update
    void Start()
    {
        tank = GetComponent<Tank>();
    }

    // Update is called once per frame
    public override void InputsUpdate(bool onIce = false)
    {
        if(!onIce) 
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                tank.GoLeft();

            if (Input.GetKey(KeyCode.RightArrow))
                tank.GoRight();

            if (Input.GetKey(KeyCode.UpArrow))
                tank.GoUp();

            if (Input.GetKey(KeyCode.DownArrow))
                tank.GoDown(); 

        }

        if (Input.GetKey(KeyCode.RightControl))
        {
            if (_shootInterval >= 0.5f || _shootInterval == 0)
            {
                tank.Shoot();
                if (_shootInterval > 0)
                    _shootInterval = 0;
            }
            _shootInterval += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.RightControl))
        {
            _shootInterval = 0;
        }       
    }
}
