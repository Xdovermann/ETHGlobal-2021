using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{  
    public enum TargetType
    {
        Enemy,
        Lootable,
        NPC,

    }

    // gets called when a target gets killed or destroyed 
    // if this is the same target that we are targeting we wanna return the ui and clear the current target
    public void RemoveTarget()
    {
        AttackHandler.attackHandler.ClearTarget(this);
    }

}
