using UnityEngine;

public class GrabbableObject : MonoBehaviour {
    Rigidbody rb;

    [Header("Grab Settings")]
    public float grabForce = 0.5f;
    public float objectMaxSpeed = 20f;

    [Header("Object Settings")]
    public float drag = 1f;
    public float objectWeight = 1f;

    private bool slippery = false;
    

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = drag;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void PullTowardsPlayer(Vector3 playerPosition, float force = 0.2f) {
        Vector3 direction = (playerPosition - transform.position).normalized;
        rb.AddForce(direction * force * (1/objectWeight) * Time.deltaTime * 350f, ForceMode.Impulse);

        //if the object is close enough to the point, do not apply force and "snap" it to the point
        if(Vector3.Distance(transform.position, playerPosition) < 0.2f) {
            rb.linearVelocity = Vector3.zero;
        }
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

    public void ActivateSlickGoo() {
        if(slippery) {return;}

        grabForce = grabForce + 1f;
        objectMaxSpeed = objectMaxSpeed + 10f;
        slippery = true;
    }
    public void ExitSlickGoo() {
        grabForce = grabForce - 1f;
        objectMaxSpeed = objectMaxSpeed - 10f;
    }
}
