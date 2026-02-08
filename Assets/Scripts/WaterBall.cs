using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBall : MonoBehaviour {
    public bool activated = false;
    private Vector3 direction;
    private float speed;

    public List<AudioClip> waterHits = new List<AudioClip>();

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Ball") {
            return;
        }
        if(collision.gameObject.tag == "Grabbable") {
            collision.gameObject.GetComponent<GrabbableObject>().ExitSlickGoo();
        }
        
        AudioSource.PlayClipAtPoint(waterHits[Random.Range(0, waterHits.Count)], this.transform.position);
        StartCoroutine(DestroyAfterDelay(0.05f));
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Goo") {
            other.gameObject.GetComponent<SlickGoo>().DestroyGooPool();
            StartCoroutine(DestroyAfterDelay(0.05f));
        }
    }

    private IEnumerator DestroyAfterDelay(float delay) {
        this.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }

    public void SetParams(Vector3 dir, float sp, bool start) {
        direction = dir;
        speed = sp;
        activated = start;
        this.GetComponent<Rigidbody>().AddForce(direction * speed * 100, ForceMode.Impulse);
    }
}
