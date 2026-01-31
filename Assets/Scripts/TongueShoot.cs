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
    private GrabbableObject grabbedObject;
    private Color green = Color.green;
    private Color red = Color.red;

    private Vector3 playerHandPos;

    void Awake()
    {
        mask = LayerMask.GetMask("Ground"); 
        cam = Camera.main.transform;
        elapsedTime = 0f;
        hitPoint = Vector3.zero;
        Instance = this;
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
        //FOR CROSSHAIR COLOR CHANGE
        CrosshairControl();

        if(Input.GetKey(KeyCode.Mouse0) && grabbedObject != null) {
            playerHandPos = player.gameObject.GetComponentInParent<PlayerMovement>().handPosition;
            grabbedObject.GetComponent<GrabbableObject>().PullTowardsPlayer(playerHandPos, grabbedObject.grabForce);

            if(Input.GetKeyUp(KeyCode.Mouse0)) {
                grabbedObject = null;
            }
            return;
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) && canShoot) {
            canShoot = false;
            if(Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, range, mask)) {
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
            hitPoint = Vector3.zero;
        }
        //if we're still holding the mouse down, don't start cooldown
        //this is so we don't reset the cooldown while pulling
        if(Input.GetKey(KeyCode.Mouse0) && canShoot == false) {
            return;
        }

        elapsedTime += Time.deltaTime;
        if(elapsedTime >= cooldown) {
            hitPoint = Vector3.zero;
            canShoot = true;
            elapsedTime = 0f;
        }
    }
}
