using UnityEngine;

public class ShipCameraSettings : MonoBehaviour
{
    [Header("Speed & Damping")]
    public float mouseSensitivity;
    public float positionSmooth;
    public float rotationSmooth;
    public float returnDelay;

    [Header("Limits")]
    public float minYAngle;
    public float maxYAngle;
}
