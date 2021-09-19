using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Transform PointToLook;

    private void Start()
    {
        PointToLook = CameraController.cameraController.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(PointToLook);
    }
}
