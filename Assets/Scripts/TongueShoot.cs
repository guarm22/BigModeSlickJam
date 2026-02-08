using UnityEngine.UI;
using UnityEngine;

public class TongueShoot : MonoBehaviour {

    [Header("References")]
    public Transform player;
    public Transform orientation;
    public Transform cam;
    public LayerMask mask;
    public Image crosshair;

    [Header("Tongue Settings")]
    public float cooldown = 2f;
    public float range = 50f;

    private float elapsedTime;
    private bool canShoot = true;
    [HideInInspector]
    public Vector3 hitPoint;
    [HideInInspector]
    public static TongueShoot Instance;

    [HideInInspector]
    public GrabbableObject grabbedObject;
    private Color green = Color.green;
    private Color red = Color.red;

    private Vector3 playerHandPos;

    private AudioSource audioSource;

    [Header("Sounds")]
    public AudioClip vacuumStart;
    public AudioClip vacuumHold;
    public AudioClip vacuumEnd;

    void Awake() {
        mask = LayerMask.GetMask("Ground"); 
        cam = Camera.main.transform;
        elapsedTime = 0f;
        hitPoint = Vector3.zero;
        Instance = this;
        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.volume = 0.05f;
    }

    void CrosshairControl() {
        if(!canShoot) {
            crosshair.color = red;
            return;
        }
        
        Physics.Raycast(cam.position, cam.forward, out RaycastHit crosshairHit, range, mask);
        GrabbableObject testObject = null;

        if(crosshairHit.collider == null) {
            crosshair.color = red;
            return;
        }

        if(crosshairHit.collider.tag == "Grabbable") {
            testObject = crosshairHit.collider.GetComponent<GrabbableObject>();
        }

        if(testObject != null || crosshairHit.collider != null) {
            crosshair.color = green;
        } else {
            crosshair.color = red;
        }
    }

    // Update is called once per frame
    void Update() {
        if(PlayerUI.instance.paused) {return;}
        CrosshairControl();

        if(Input.GetKeyUp(KeyCode.Mouse0) && grabbedObject != null) {
            grabbedObject.GetComponent<GrabbableObject>().LetGo();
            grabbedObject = null;
            playVacuumEnd();
            return;
        }

        if(Input.GetKey(KeyCode.Mouse0) && grabbedObject != null) {
            if(Input.GetKey(KeyCode.F)) {
                grabbedObject.GetComponent<GrabbableObject>().LetGo();
                grabbedObject.GetComponent<GrabbableObject>().ShootAwayFromPlayer(player.transform.position);
                grabbedObject = null;
                playVacuumEnd();
                return;
            }
            if(!audioSource.isPlaying) {
                playVacuumHold();
            }

            playerHandPos = player.gameObject.GetComponentInParent<PlayerMovement>().handPosition;
            grabbedObject.GetComponent<GrabbableObject>().PullTowardsPlayer(playerHandPos, grabbedObject.grabForce);
            return;
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) && canShoot) {
            canShoot = false;

            if(Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, range, mask)) {
                playVacuumStart();

                if(hit.collider.GetComponent<GrabbableObject>() != null) {
                    grabbedObject = hit.collider.GetComponent<GrabbableObject>();
                    return;
                }
                hitPoint = hit.point;
                Debug.DrawRay(hit.point, Vector3.up * 0.1f, Color.green, 5.0f);
                //draw a ray between player and hit point
                Debug.DrawLine(player.position, hit.point, Color.blue, 5.0f);
            } else {
                canShoot = true;
            }

        }
        if(Input.GetKeyUp(KeyCode.Mouse0)) {
            if(grabbedObject != null) {
                grabbedObject = null;
            }
            if(hitPoint != Vector3.zero) {playVacuumEnd();}

            hitPoint = Vector3.zero;
        }
        //if we're still holding the mouse down, don't start cooldown
        //this is so we don't reset the cooldown while pulling
        if(Input.GetKey(KeyCode.Mouse0) && canShoot == false) {
            if(!audioSource.isPlaying) {
                playVacuumHold();
            }
            return;
        }

        elapsedTime += Time.deltaTime;
        if(elapsedTime >= cooldown) {
            hitPoint = Vector3.zero;
            canShoot = true;
            elapsedTime = 0f;
        }
    }

    private void randomizePitch() {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
    }


    private void playVacuumHold(){
        audioSource.Stop();
        audioSource.clip = vacuumHold;
        audioSource.Play();
    }

    private void playVacuumStart(){
        randomizePitch();
        audioSource.Stop();
        audioSource.clip = vacuumStart;
        audioSource.Play();
    }

    private void playVacuumEnd(){
        randomizePitch();
        audioSource.Stop();
        audioSource.clip = vacuumEnd;
        audioSource.Play();
    }
}
