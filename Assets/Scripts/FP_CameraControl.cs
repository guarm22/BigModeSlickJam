using System.Collections;
using UnityEngine;

public class FP_CameraControl : MonoBehaviour
{
    public float sens;

    public Transform orientation;

    float xRotation;
    float yRotation;

    public float FOV = 90;

    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Camera.main.fieldOfView = FOV;
        sens = PlayerPrefs.GetFloat("Sens", 400);
    }

    void Update() {
        if(PlayerUI.instance.paused) {return;}
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        if(Input.GetKeyDown(KeyCode.X)) {
            StopAllCoroutines();
            StartCoroutine(SnapCamera180());
        }

        if(Input.GetKeyDown(KeyCode.PageUp)) {
           sens += 50;
           PlayerPrefs.SetFloat("Sens", sens);
        }
        if(Input.GetKeyDown(KeyCode.PageDown)) {
           sens -= 50;
           PlayerPrefs.SetFloat("Sens", sens);
        }
        
    }

    private IEnumerator SnapCamera180(float duration = 0.2f) {
        float elapsedTime = 0f;
        while(elapsedTime < duration) {
            float step = (180f / duration) * Time.deltaTime;
            yRotation += step;
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public static Vector3 pointInFrontOfCamera(float distance) {
        Camera cam = Camera.main;
        return cam.transform.position + cam.transform.forward * distance;
    }

    public static Vector3 cameraFacingDirection() {
        return Camera.main.transform.forward;
    }
}
