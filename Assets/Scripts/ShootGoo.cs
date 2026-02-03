using UnityEngine;

public class ShootGoo : MonoBehaviour {

    [Header("ShootObject")]
    public GameObject gooBullet;

    public GameObject waterBullet;

    [Header("Player Info")]
    public Transform playerBody;

    private float waterCd = 0.05f;
    private float elapsedTime = 0;
    
    void Start() {
        
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Q)) {
            Shoot();
        }
        if(Input.GetKey(KeyCode.Mouse1) && elapsedTime > waterCd){ ShootWater(); elapsedTime=0;}
        elapsedTime += Time.deltaTime;
    }

    private void ShootWater(){
        Vector3 spawnPos = FP_CameraControl.pointInFrontOfCamera(1.2f);
        Vector3 currentDirection = FP_CameraControl.cameraFacingDirection();
        Vector3 moveDirection = currentDirection;

        WaterBall ball = Instantiate(waterBullet, spawnPos, playerBody.rotation).GetComponent<WaterBall>();
        ball.SetParams(moveDirection, 0.3f, true);
    }


    private void Shoot() {
        //from player, shoot a rigidbody object forward then record where it lands when it hits the ground
        Vector3 spawnPos = FP_CameraControl.pointInFrontOfCamera(1f);
        Vector3 currentDirection = FP_CameraControl.cameraFacingDirection();
        Vector3 moveDirection = currentDirection;

        SlickGooBall ball = Instantiate(gooBullet, spawnPos, playerBody.rotation).GetComponent<SlickGooBall>();
        float distance = 0.3f;
        ball.SetParams(moveDirection, distance, true);

    }
}
