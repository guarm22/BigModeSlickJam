using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {
    [Header("Movement")]
    public float maxSpeed = 3f;
    public float groundDrag = 5f;
    public float speedLimit = 60f;
    [HideInInspector]
    private float defaultMaxSpeed = 4.5f;


    [Header("Ground Check")]
    public float playerHeight = 2f; //2 is default for capsule
    public LayerMask groundLayer;
    bool grounded;

    [Header("Jumping")]
    public float jumpForce = 6;
    public float jumpCd = 0.25f;
    public float airMultiplier = 0.4f;
    bool readyToJump = true; //must start as true
    
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Other")]
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    private bool BeingPulled = false;
    private Vector3 spawnPoint;
    private float defaultDrag;

    private bool pounding = false;

    public Vector3 handPosition;

    private List<string> activeGooZones = new List<string>();

    private float usualSpeedMult = 2.5f;


    private bool crouching = false;

    public List<AudioClip> jumpSounds = new List<AudioClip>();
    public AudioSource audioSource;

     void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        spawnPoint = transform.position;
        defaultDrag = groundDrag;
        if(audioSource == null)
        {
            audioSource = this.gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.clip = jumpSounds[0];
            audioSource.volume= 0.5f;
        }
    }

    //step 1. CHECK IF PLAYER IS GRAPPLED
    //STEP 2. IF YES, GET THE POINT THE PLAYER IS GRAPPLED TO
    //STEP 3. APPLY FORCE TO PLAYER FROM THAT POINT WHEN THEY JUMP
  
    private void GetInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && grounded) {
            RaycastHit hit;
            Vector3 halfExtents = new Vector3(0.5f, 0.5f, 0.5f);
            grounded = Physics.BoxCast(transform.position, halfExtents, Vector3.down, out hit, Quaternion.identity, 0.8f, groundLayer);
            if(hit.collider){
                if(hit.collider.gameObject.tag == "Grabbable" && TongueShoot.Instance.grabbedObject != null) {
                return;
                }
            }

            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCd); //TODO - if pausing is added, must change this
        }
        //RESET TO SPAWNPOINT
        if(Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //TONGUE SHOOT PULL --- While mouse0 is down and there is a hit point, pull player towards it
        if(Input.GetKey(KeyCode.Mouse0) && TongueShoot.Instance.hitPoint != Vector3.zero) {
            SetDrag(0f);
            maxSpeed = defaultMaxSpeed * 2f; //increase speed while being pulled
            BeingPulled = true;
        }
        else {
            BeingPulled = false;
        }

        if(Input.GetKey(KeyCode.Z) && !grounded) {
            pounding = true;
        }
        
        if(Input.GetKeyDown(KeyCode.F6)) {
            SceneManager.LoadScene(0);
        }
        if(Input.GetKeyDown(KeyCode.F7)) {
            SceneManager.LoadScene(1);
        }
    }

    private void Crouch() {
        Vector3 uncrouched = new Vector3(1, 1, 1);
        Vector3 crouched = new Vector3(0.5f, 0.5f, 0.5f);

        //no crouching in the air
        if(Input.GetKey(KeyCode.LeftControl) && !grounded && !crouching) {
            Debug.Log("In the air and not crouching");
            crouching = true;
            return;
        }

        if((Input.GetKey(KeyCode.LeftControl) && !crouching) || (grounded && Input.GetKey(KeyCode.LeftControl) && crouching && this.transform.localScale != crouched)) {
            this.transform.localScale = crouched;
            crouching = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftControl) && crouching) {
            this.transform.localScale = uncrouched;
            crouching = false;
        }
    }

    void Update() {
        if(PlayerUI.instance.paused) {return;}
        //ground check
        Vector3 halfExtents = new Vector3(0.5f, 0.5f, 0.5f);
        grounded = Physics.BoxCast(transform.position, halfExtents, Vector3.down, Quaternion.identity, 0.8f, groundLayer);
        GetInput();
        SpeedControl();
        SlickGooCheck();
        Crouch();
        if(grounded) {
            pounding = false;

            if(crouching) {
                if(defaultMaxSpeed > maxSpeed) {
                    maxSpeed = defaultMaxSpeed;
                }
            }
            else{
                maxSpeed = defaultMaxSpeed;
            }
        }
        else{ rb.linearDamping = 0;}

        if(maxSpeed > speedLimit) {
            maxSpeed = speedLimit;
        }
        handPosition = FP_CameraControl.pointInFrontOfCamera(1.8f);
    }

    private void FixedUpdate() {
        if(PlayerUI.instance.paused) {return;}
        MovePlayer();
        if (BeingPulled) {
            PullPlayerTowardsPoint(TongueShoot.Instance.hitPoint, 2f);
        }
        if(pounding){
            GroundPound(2f);
        }
        rb.AddForce(Physics.gravity * 1.6f, ForceMode.Acceleration);
    }

    private void MovePlayer() {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        float crouchedMult = crouching ? 0.08f : 1.0f;
        float trueDefaultMaxSpeed = crouching ? usualSpeedMult : maxSpeed;

        if(grounded) {
            rb.AddForce(moveDirection.normalized * crouchedMult *  trueDefaultMaxSpeed * 10f, ForceMode.Force);
        }
        else if(!grounded) {
            rb.AddForce(moveDirection.normalized * trueDefaultMaxSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl() {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if(flatVel.magnitude > maxSpeed){
           Vector3 limitedVel = flatVel.normalized * maxSpeed;
           rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump() {
        rb.linearVelocity = new (rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        
        audioSource.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Count)]);
    }

    private void ResetJump() {
        readyToJump = true;
    }

    public void ActivateSlickGoo(SlickGoo slickGoo) {
        activeGooZones.Add(slickGoo.name);
        defaultMaxSpeed = defaultMaxSpeed + usualSpeedMult;
    }
    public void ExitSlickGoo(SlickGoo slickGoo) {
        activeGooZones.Remove(slickGoo.name);
        if(!grounded || crouching) {
            StartCoroutine(DelayedSpeedReduction(usualSpeedMult));
            return;
        }
        defaultMaxSpeed = defaultMaxSpeed - usualSpeedMult;
    }

    private IEnumerator DelayedSpeedReduction(float amount) {
        //wait until grounnded
        while(!grounded || crouching) {
            yield return null;
        }
        defaultMaxSpeed = defaultMaxSpeed - amount;
    }
    
    private void SlickGooCheck() {
        if(activeGooZones.Count == 0) {
            SetDrag(defaultDrag);
        }
        else {
            SetDrag(0f);
        }
    }


    //
    public void PullPlayerTowardsPoint(Vector3 point, float force = 10f) {
        Vector3 direction = (point - transform.position).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    public void GroundPound(float force = 10f) {
        rb.AddForce(Vector3.down * force, ForceMode.Impulse);
    }

    public void SetDrag(float dragAmount) {
        rb.linearDamping = dragAmount;
    }

    //functions for testing
    public float GetCurrentPlayerSpeed() {
        return rb.linearVelocity.magnitude;
    }

    public float GetCurrentMaxSpeed() {
        return maxSpeed;
    }
}
