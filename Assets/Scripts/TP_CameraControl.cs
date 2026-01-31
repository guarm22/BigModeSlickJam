using UnityEngine;

public class TP_CameraControl : MonoBehaviour {
    private const float YMin = -65.0f;
    private const float YMax = 30.0f;

    public Transform lookAt;

    public Transform Player;

    public Transform orientation;

    public float distance = 10.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    public float sensivity = 4.0f;

    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Camera.main.fieldOfView = 80;
    }
    void LateUpdate() {

        currentX += Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;
        currentY += Input.GetAxis("Mouse Y") * sensivity * Time.deltaTime;

        currentY = Mathf.Clamp(currentY, YMin, YMax);

        Vector3 Direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(-currentY, currentX, 0);
        transform.position = lookAt.position + rotation * Direction;

        transform.LookAt(lookAt.position);
        orientation.rotation = Quaternion.Euler(0, currentX, 0);
    }
}
