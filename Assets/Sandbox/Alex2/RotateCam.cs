using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCam : MonoBehaviour
{
    public float Speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Fire2") > 0)
        {
            Debug.Log(Input.GetAxis("Fire2") + " " + Input.GetAxis("Mouse X"));
            this.transform.Rotate(0, Input.GetAxis("Mouse X") * Speed, 0);
        }
    }
}
