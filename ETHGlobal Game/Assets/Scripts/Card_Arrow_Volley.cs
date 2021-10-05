using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Arrow_Volley : Card
{
    public GameObject Arrow;

    // Start is called before the first frame update
    void Start()
    {
        StartFunction();
    }

    public override void CardEffect()
    {
        StartCoroutine(ArrowVolley());
        IEnumerator ArrowVolley()
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject go = Instantiate(Arrow, PlayerController.playerController.transform.position, AttackHandler.attackHandler.WeaponAttackPoint.rotation);
                go.GetComponent<Projectile>().SetUp(AttackHandler.attackHandler.GetTargetDirection());
                yield return new WaitForSeconds(0.1f);
            }

            base.CardEffect();

        }
    }
}
