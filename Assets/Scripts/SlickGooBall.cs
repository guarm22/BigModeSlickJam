using UnityEngine;

public class SlickGooBall : MonoBehaviour {
    
    public GameObject SlickGoo;
    public bool activated = false;
    private Vector3 direction;
    private float speed;
    private void spawnGooPool() {
        Vector3 vec = Camera.main.transform.rotation.eulerAngles;
        Instantiate(SlickGoo, this.transform.position - new Vector3(0, 0.08f, 0), Quaternion.Euler(0, vec.y, 0));
        Destroy(this.gameObject);
    }


    void OnCollisionEnter(Collision collision) {
        if(LayerMask.LayerToName(collision.gameObject.layer) == "Ground") {
            if(collision.gameObject.tag != "StaticObjects") {return;}
            spawnGooPool();
        }
    }

    public void SetParams(Vector3 dir, float sp, bool start) {
        direction = dir;
        speed = sp;
        activated = start;
        this.GetComponent<Rigidbody>().AddForce(direction * speed * 100, ForceMode.Impulse);
    }


}
