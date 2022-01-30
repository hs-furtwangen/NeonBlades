using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum color{
    BLUE,
    RED
}
public class Target : MonoBehaviour
{   
    public bool registered = false;
    public bool destroyed = false;
    public color color;
    void Start() {
    }
    void Update() {
        if(!registered) {
            GameMaster._instance.RegisterTarget(this);
            registered = true;
        }
        if(gameObject.activeInHierarchy == false) {
            DestroyTarget();
        }
    }
    public void DestroyTarget() {
        if(!destroyed) {
            GameMaster._instance.DestroyTarget(this);
            destroyed = true;
        }

    }
}
