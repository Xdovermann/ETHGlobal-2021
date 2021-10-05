using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Single_Arrow : Card
{
    public GameObject Arrow;

    // Start is called before the first frame update
    void Start()
    {
        StartFunction();
    }

    public override void CardEffect()
    {
        base.CardEffect();
        GameObject go = Instantiate(Arrow, PlayerController.playerController.transform.position, AttackHandler.attackHandler.WeaponAttackPoint.rotation);
        go.GetComponent<Projectile>().SetUp(AttackHandler.attackHandler.GetTargetDirection());
    }
}
