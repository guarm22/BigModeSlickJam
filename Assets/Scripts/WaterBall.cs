using UnityEngine;

public class WaterBall : MonoBehaviour {
    public bool activated = false;
    private Vector3 direction;
    private float speed;

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Goo") {
            collision.gameObject.GetComponent<SlickGoo>().DestroyGooPool();
        }
        if (collision.gameObject.tag == "Player")
        {
            return;
        }
        Destroy(this.gameObject);
    }

    public void SetParams(Vector3 dir, float sp, bool start) {
        direction = dir;
        speed = sp;
        activated = start;
        this.GetComponent<Rigidbody>().AddForce(direction * speed * 100, ForceMode.Impulse);
    }
}
