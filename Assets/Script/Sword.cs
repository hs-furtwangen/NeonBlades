using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    
    public bool activeState = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionExit(Collision collision) {
        if(activeState) {
            if(collision.gameObject.GetComponent<Slice>() != null)
            {
                    if(collision.gameObject.GetComponent<Slice>() != null)
    {
        var hitbox = this.GetComponent<Collider>();
        var center = hitbox.bounds.center;
        var extents = hitbox.bounds.extents;

        extents = new Vector3(extents.x * this.transform.localScale.x,
                            extents.y * this.transform.localScale.y,
                            extents.z * this.transform.localScale.z);
                            
        // Cast a ray and find the nearest object
        RaycastHit[] hits = Physics.BoxCastAll(this.transform.position, extents, this.transform.forward, this.transform.rotation, extents.z);
        
        foreach(RaycastHit hit in hits)
        {
            var obj = hit.collider.gameObject;
            var sliceObj = obj.GetComponent<Slice>();

            if (sliceObj != null)
            {
                sliceObj.GetComponent<MeshRenderer>()?.material.SetVector("CutPlaneOrigin", Vector3.positiveInfinity);
                sliceObj.ComputeSlice(this.transform.up, this.transform.position);
            }
        }
    }
            }
        }
    }

    public void SetActiveState (bool state) {
        this.activeState = state;
    }
    public void KatanaSlice() {

    }
}
