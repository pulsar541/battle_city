using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupCamera : MonoBehaviour
{
    public float startupCameraSpeed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        if(transform.position.y > 0)
            startupCameraSpeed = -startupCameraSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y != 0) {
            transform.position += new Vector3(0, startupCameraSpeed * Time.deltaTime, 0);

            if(startupCameraSpeed > 0 && transform.position.y > 0) 
                transform.position  = new Vector3(0, 0, -1);

            if(startupCameraSpeed < 0 && transform.position.y < 0) 
                transform.position  = new Vector3(0, 0, -1);
        }
    }
}
