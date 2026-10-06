using System.Collections.Generic;
using UnityEngine;

public class ShipSelector : MonoBehaviour
{
    public List<ShipController> shipList = new List<ShipController>();
    public ShipCameraController shipCameraController;

    private int shipIndex = 0;

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            for (var i=0; i< shipList.Count; i++)
            {
                shipList[i].gameObject.SetActive(false);
            }

            shipIndex++;

            if (shipList.Count <= shipIndex)
            {
                shipIndex = 0;
            }
            
            shipList[shipIndex].gameObject.SetActive(true);
            shipCameraController.shipController = shipList[shipIndex];
        }
    }
}
