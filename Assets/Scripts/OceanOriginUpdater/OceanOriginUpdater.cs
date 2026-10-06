using UnityEngine;

public class OceanOriginUpdater : MonoBehaviour
{
    public ShipCameraController shipCameraController;
    public Transform ocean;
    public float shiftThreshold = 400f;

    void Update()
    {
        FixAllAxis();
    }

    public void FixAllAxis()
    {
        // X-axis
        if (Mathf.Abs(shipCameraController.shipController.transform.position.x - ocean.position.x) > shiftThreshold)
        {
            ocean.transform.position = new Vector3(shipCameraController.shipController.transform.position.x, ocean.transform.position.y, ocean.transform.position.z);
        }

        // Y-axis
        if (Mathf.Abs(shipCameraController.shipController.transform.position.y - ocean.position.y) > shiftThreshold)
        {
            ocean.transform.position = new Vector3(ocean.transform.position.x, shipCameraController.shipController.transform.position.y, ocean.transform.position.z);
        }

        // Z-axis
        if (Mathf.Abs(shipCameraController.shipController.transform.position.z - ocean.position.z) > shiftThreshold)
        {
            ocean.transform.position = new Vector3(ocean.transform.position.x, ocean.transform.position.y, shipCameraController.shipController.transform.position.z);
        }
    }
}
