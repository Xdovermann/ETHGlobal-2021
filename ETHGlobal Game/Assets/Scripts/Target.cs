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

    public TargetType targetType;

    private Enemy enemy;
    private void Start()
    {
        switch (targetType)
        {
            case TargetType.Enemy:
                enemy = GetComponent<Enemy>();
                break;
            case TargetType.Lootable:
                break;
            case TargetType.NPC:
                break;
            default:
                break;
        }
    }

    public void SettedTarget()
    {
        switch (targetType)
        {
            case TargetType.Enemy:
                enemy.ToggleHealthBar(true);
                break;
            case TargetType.Lootable:
                break;
            case TargetType.NPC:
                break;
            default:
                break;
        }
    }


    // gets called when a target gets killed or destroyed 
    // if this is the same target that we are targeting we wanna return the ui and clear the current target
    public void RemoveTarget()
    {
        AttackHandler.attackHandler.ClearTarget(this);
        switch (targetType)
        {
            case TargetType.Enemy:
                enemy.ToggleHealthBar(false);
                break;
            case TargetType.Lootable:
                break;
            case TargetType.NPC:
                break;
            default:
                break;
        }
    }

}
