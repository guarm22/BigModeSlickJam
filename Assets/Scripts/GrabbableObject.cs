using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class GrabbableObject : MonoBehaviour {
    Rigidbody rb;

    [Header("Grab Settings")]
    public float grabForce = 0.5f;
    public float objectMaxSpeed = 20f;

    [Header("Object Settings")]
    public float drag = 1f;
    private bool slippery = false;
    

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = drag;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void PullTowardsPlayer(Vector3 playerPosition, float force = 0.2f) {
        Vector3 toTarget = playerPosition - transform.position;
        float distance = toTarget.magnitude;
        // Tunables
        float slowRadius = 1f;
        float stopRadius = 0.05f;

        Vector3 desiredVelocity;
        if (distance < stopRadius){
            desiredVelocity = Vector3.zero;
        }
        else {
            float speed = objectMaxSpeed;
            if (distance < slowRadius) {
                speed = objectMaxSpeed * (distance / slowRadius);
            }
            desiredVelocity = toTarget.normalized * speed;
        }

        // Difference between current and desired velocity
        Vector3 steering = desiredVelocity - rb.linearVelocity;


        rb.AddForce(steering * force * 50f, ForceMode.Acceleration);

        //MAKE OBJECT MORE STABLE WHEN AT PLAYERS HAND
        if(Vector3.Distance(this.transform.position, playerPosition) < 1.5f) {
            rb.linearDamping = 8f;
        }
        else {
            rb.linearDamping = 0f;
        }
    }

    public void LetGo()
    {
        rb.linearDamping = 0f;
    }

    void FixedUpdate() {
        SpeedControl();
    }

    private void SpeedControl() {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if(flatVel.magnitude > objectMaxSpeed){
           Vector3 limitedVel = flatVel.normalized * objectMaxSpeed;
           rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    public void ActivateSlickGoo(Material gooMat) {
        if(slippery) {return;}

        Material[] currentMats = GetComponent<MeshRenderer>().materials;
        currentMats = currentMats.Append(gooMat).ToArray();
        GetComponent<MeshRenderer>().materials = currentMats;

        grabForce = grabForce + 0.1f;
        objectMaxSpeed = objectMaxSpeed + 8f;
        slippery = true;
    }
    public void ExitSlickGoo() {
        if(!slippery) {return; }

        Material[] currentMats = GetComponent<MeshRenderer>().materials;
        currentMats = currentMats.SkipLast(1).ToArray();
        GetComponent<MeshRenderer>().materials=currentMats;
        
        grabForce = grabForce - 0.1f;
        objectMaxSpeed = objectMaxSpeed - 8f;
        slippery = false;
    }
}
