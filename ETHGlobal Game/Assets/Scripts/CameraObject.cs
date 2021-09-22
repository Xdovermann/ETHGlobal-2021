using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


// dit object handled de camera movement op npc talks en de camera movement van de muis
public class CameraObject : MonoBehaviour
{

	public static CameraObject cameraObject;
	public Transform NormalCamPoint; // de positie die de camera inneemt

	Vector3 target, mousePos, refVel;
	public float cameraDist = 1.75f;
	public float smoothTime = 0.010f;

	public float MoveSpeed = 25;

	private void Awake()
	{
		cameraObject = this;

	}


	void Start()
	{

		target = NormalCamPoint.position; //set default target
		
	}

	void Update()
	{
			
	    mousePos = CaptureMousePos(); //find out where the mouse is
	
		target = Vector3.Lerp(target, UpdateTargetPos(), MoveSpeed *Time.deltaTime);

		UpdateCameraPosition();
	}

	Vector3 CaptureMousePos()
	{
        if (!AttackHandler.attackHandler.hasTarget())
        {
			MoveSpeed = 25;
			Vector3 ret = Camera.main.ScreenToViewportPoint(Input.mousePosition); //raw mouse pos
			ret *= 2;
			ret -= Vector3.one; //set (0,0) of mouse to middle of screen

			ret.z = ret.y;
			ret.y = 0;


			float max = 0.9f;
			if (Mathf.Abs(ret.x) > max || Mathf.Abs(ret.z) > max)
			{
				ret = ret.normalized; //helps smooth near edges of screen
			}


			return ret;
        }
        else
        {
			MoveSpeed = 5;
			Vector3 ret = Camera.main.WorldToViewportPoint(AttackHandler.attackHandler.CurrentTarget.position);

			//raw mouse pos
			ret *= 4.25f;
			ret += AttackHandler.attackHandler.transform.position; //set (0,0) of mouse to middle of screen

			ret.z = ret.y;
			ret.y = 0;

			return ret;
		}
	
	}

	Vector3 UpdateTargetPos()
	{
		mousePos = NormalCamPoint.TransformVector(mousePos);
		mousePos.y = 0;
		Vector3 mouseOffset = mousePos * cameraDist;
		Vector3 offset = NormalCamPoint.position + mouseOffset;
		return offset;
	}


	void UpdateCameraPosition()
	{
		Vector3 tempPos;
		tempPos = Vector3.SmoothDamp(transform.position, target, ref refVel, smoothTime); //smoothly move towards the target

		transform.position = tempPos; //update the position


	}








}
