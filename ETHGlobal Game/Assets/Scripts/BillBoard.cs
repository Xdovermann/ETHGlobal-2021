using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public bool KeepUpdating = false;
    private void Awake()
    {
        transform.localEulerAngles = new Vector3(-35, -50, 0);
    }

    private void Update()
    {
        if (KeepUpdating)
        {
            transform.LookAt(CameraController.cameraController.transform.position);
        }
    }


}
