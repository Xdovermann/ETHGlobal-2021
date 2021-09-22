using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CameraController : MonoBehaviour
{
    public static CameraController cameraController;

    public Transform Player;

    public float FollowSpeed = 10f;

    public Vector3 CameraOffset;
    private Vector3 shakeOffset;
    float shakeMag, shakeTimeEnd;
    Vector3 shakeVector, refVel;
    bool shaking;

    Vector3 target;

    
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        cameraController = this;
    }

    private void LateUpdate()
    {
      
        shakeOffset = UpdateShake();

       

        target = Vector3.SmoothDamp(transform.position, Player.position + shakeOffset+ CameraOffset, ref refVel, 0.05f);
        transform.position = target;

    }

    private void Update()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            Camera.main.DOFieldOfView(50,0.1f);
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            Camera.main.DOFieldOfView(60, 0.1f);
        }

        //if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        //{

            //    Camera.main.fieldOfView -= Input.GetAxisRaw("Mouse ScrollWheel")*50000*Time.deltaTime;
            //    Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 50, 60);
            //}
    }

    private Vector3 UpdateShake()
    {
        if (!shaking || Time.time > shakeTimeEnd)
        {
            shaking = false; 
            return Vector3.zero; 
        } 

        Vector3 tempOffset = shakeVector;
        tempOffset *= shakeMag; 
        return tempOffset;
    }

    public void Shake(Vector3 direction, float magnitude, float length)
    { 
        shaking = true; 
        shakeVector = direction; 
                              
        shakeMag = magnitude; 
        shakeTimeEnd = Time.time + length; 
    }

 
}
