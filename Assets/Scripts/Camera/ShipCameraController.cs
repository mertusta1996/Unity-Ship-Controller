using UnityEngine;

public class ShipCameraController : MonoBehaviour
{
    public ShipController shipController;
    public ShipCameraSettings shipCameraSettings;
    
    // Private Fields
    private float currentX;
    private float currentY;
    private float rotVelocityX;
    private float timeSinceMouseMoved;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (shipController != null)
        {
            currentX = shipController.transform.eulerAngles.y;
        }
    }

    void FixedUpdate()
    {
        if (!shipController) return;

        float mouseX = Input.GetAxis("Mouse X") * shipCameraSettings.mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * shipCameraSettings.mouseSensitivity;

        if (Mathf.Abs(mouseX) > 0.01f || Mathf.Abs(mouseY) > 0.01f)
        {
            currentX += mouseX;
            currentY -= mouseY;
            timeSinceMouseMoved = 0f;
        }
        else
        {
            timeSinceMouseMoved += Time.fixedDeltaTime;

            if (timeSinceMouseMoved >= shipCameraSettings.returnDelay)
            {
                float targetX = shipController.transform.eulerAngles.y;
                currentX = Mathf.SmoothDampAngle(currentX, targetX, ref rotVelocityX, 1.0f / shipCameraSettings.rotationSmooth);
            }
        }

        currentY = Mathf.Clamp(currentY, shipCameraSettings.minYAngle, shipCameraSettings.maxYAngle);

        Quaternion targetRotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 targetPosition = shipController.transform.position + targetRotation * shipController.cameraOffset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, shipCameraSettings.positionSmooth * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, shipCameraSettings.rotationSmooth * Time.fixedDeltaTime);
    }
}