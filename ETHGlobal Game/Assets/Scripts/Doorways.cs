using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorways : MonoBehaviour
{
    DungeonRoom BelongsToRoom;

    public Transform PlayerSpawnPoint;
    public int DoorIndex;
    public GameObject DoorwayBlocker;
    public GameObject PathWay;
    public void ConnectDoorToRoom(DungeonRoom room)
    {
        BelongsToRoom = room;
        // means that theres no room connecting to that side so disable the door
        if(room.Connections[DoorIndex] == false)
        {
            gameObject.SetActive(false);
            DoorwayBlocker.SetActive(true);
            PathWay.SetActive(false);
            DoorwayBlocker.transform.SetParent(BelongsToRoom.transform);
        }
        else
        {
          
            DoorwayBlocker.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartTravel();
        }
    }

    public void StartTravel()
    {
        AttackHandler.attackHandler.ClearTarget(AttackHandler.attackHandler.target);
        BelongsToRoom.TravelToNextRoom(DoorIndex);
    }

    public void SetPlayerPos()
    {
        PlayerController.playerController.transform.position = PlayerSpawnPoint.position;
        PlayerController.playerController.CancelMovement();
       
    }
   
}
