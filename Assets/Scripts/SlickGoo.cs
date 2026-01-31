using System.Collections;
using UnityEngine;

public class SlickGoo : MonoBehaviour {

    public Transform player;
    private bool activated = false;

    private float gooDuration;

    private float elapsedTime = 0f;
    private bool exitedGoo = false;

    void Awake() {
        if(player == null)
        {
            player = GameObject.Find("PlayerBody").transform;
        }
        gooDuration = 1.5f;   
    }

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            PlayerMovement playerMovement = other.GetComponentInParent<PlayerMovement>();
            if(playerMovement != null) {
                if(exitedGoo) {
                    activated = true;
                    exitedGoo = false;
                    elapsedTime = 0f;
                }
            }
        }

        if(other.tag == "Grabbable"){
            GrabbableObject grabbable = other.GetComponentInParent<GrabbableObject>();
            if(grabbable != null) {
                grabbable.ActivateSlickGoo();
            }
        }
    }

    void OnTriggerStay(Collider other) {
        if(other.tag == "Player"){
            PlayerMovement playerMovement = other.GetComponentInParent<PlayerMovement>();
            if(playerMovement != null) {
                if(activated) {return;}
                activated = true;
                playerMovement.ActivateSlickGoo(this);
            }
        }
    }

    void OnTriggerExit(Collider other){
        if(other.tag == "Player"){
            PlayerMovement playerMovement = other.GetComponentInParent<PlayerMovement>();
            if(playerMovement != null) {
                exitedGoo = true;
            }
        }
    }

    public void DestroyGooPool() {
        if(!activated) {
            Destroy(this.gameObject);
        }
        //turn off all renderers and colliders
        exitedGoo = true;
        this.GetComponent<BoxCollider>().enabled = false;
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(DestroyWait());
    }

    private IEnumerator DestroyWait(){
        while(this.activated) {
            yield return null;
        }
        Destroy(this.gameObject);
    }


    void Update() {
        if(!exitedGoo) {return;}
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= gooDuration) {
            PlayerMovement playerMovement = player.GetComponentInParent<PlayerMovement>();
            if(playerMovement != null) {
                activated = false;
                exitedGoo = false;
                playerMovement.ExitSlickGoo(this);
            }
            elapsedTime = 0f;
        }
    }
}
