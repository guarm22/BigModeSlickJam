using System;
using UnityEngine;

public class SlickGooBall : MonoBehaviour {
    
    public GameObject SlickGoo;
    public bool activated = false;
    private Vector3 direction;
    private float speed;
    private void spawnGooPool(float angleOfSurface, float wallAngle) {
        Vector3 vec = Camera.main.transform.rotation.eulerAngles;
        double target = 90;
        double result = Math.Round(vec.y / target) * target;

        Instantiate(SlickGoo, this.transform.position - new Vector3(0, 0.08f, 0), Quaternion.Euler(-90 + angleOfSurface, wallAngle, 0));
        Destroy(this.gameObject);
    }


    void OnCollisionEnter(Collision collision) {
        if(LayerMask.LayerToName(collision.gameObject.layer) == "Ground") {
            Vector3 surfaceNormal = collision.contacts[0].normal;
            Vector3 incomingVelocity = this.GetComponent<Rigidbody>().linearVelocity;

            float angleOfSurfaceToWorldUp = Vector3.Angle(surfaceNormal, Vector3.up);

            Vector3 wallNormal = collision.contacts[0].normal;
            // Calculate the angle in degrees (0 to 360)
            // We use .x and .z because we want the horizontal orientation
            float wallAngle = Mathf.Atan2(wallNormal.x, wallNormal.z) * Mathf.Rad2Deg;

            if(collision.gameObject.tag != "StaticObjects") {return;}
            spawnGooPool(angleOfSurfaceToWorldUp, wallAngle);
        }
    }

    public void SetParams(Vector3 dir, float sp, bool start) {
        direction = dir;
        speed = sp;
        activated = start;
        this.GetComponent<Rigidbody>().AddForce(direction * speed * 100, ForceMode.Impulse);
    }


}
