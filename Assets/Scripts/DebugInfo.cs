using TMPro;
using UnityEngine;

public class DebugInfo : MonoBehaviour
{
    public GameObject player;
    
    private PlayerMovement moveScript;
    private FP_CameraControl camScript;

    [Header("UI Elements")]
    public TMP_Text currentPlayerSpeed;
    public TMP_Text currentMaxSpeed;
    public TMP_Text currentSens;
    void Start()
    {
        moveScript = player.GetComponent<PlayerMovement>();
        camScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FP_CameraControl>();
    }

    // Update is called once per frame
    void Update()
    {
        currentPlayerSpeed.text = "Current Speed: " + moveScript.GetCurrentPlayerSpeed().ToString("F2");
        currentMaxSpeed.text = "Current Max Speed: "  + moveScript.GetCurrentMaxSpeed().ToString("F2");
        currentSens.text = "Mouse Sens: " + camScript.sensX.ToString("F2");
    }
}
