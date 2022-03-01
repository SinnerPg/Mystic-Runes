using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    bool isPicking  ;
    GameObject pickedObject;
    Rigidbody objectRb;
    FixedJoint joint;
 
    public void Start() {
        isPicking = false;
        joint = GetComponent<FixedJoint>();
    }
 
    public void Update() {
        RaycastHit hitObject;
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitObject, 2, 1 << 9))
        {
            if(Input.GetMouseButtonDown(0))
            {
                isPicking = true;
                hitObject.transform.parent = null;
                pickedObject = hitObject.transform.gameObject;
                objectRb = pickedObject.GetComponent<Rigidbody>();
                hitObject.rigidbody.useGravity = false;
                hitObject.rigidbody.velocity = Vector3.zero;
                hitObject.rigidbody.angularVelocity = Vector3.zero;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isPicking = false;
            if (pickedObject != null)
            {
                objectRb.useGravity = true;
                pickedObject = null;
            }
        }
 
        if (isPicking)
        {
            if (pickedObject != null)
            {
                if (joint == null)
                {
                    joint = this.gameObject.AddComponent<FixedJoint>();
                    joint.connectedBody = objectRb;
                    joint.enableCollision = true;
                    joint.breakForce = 5000;
                    joint.connectedMassScale = joint.connectedBody.mass;
                }
            }
        }
        else
        {
            if (joint != null)
            {
                objectRb.velocity = Vector3.zero;
                Object.Destroy(joint);
            }
        }
    }

    private void OnJointBreak(float breakForce) {
        objectRb.velocity = Vector3.zero;
        isPicking = false;
        if (pickedObject != null)
        {
            objectRb.useGravity = true;
            pickedObject = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Pickup
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 2);
    }
}
