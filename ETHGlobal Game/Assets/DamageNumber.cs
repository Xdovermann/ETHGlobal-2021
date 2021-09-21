using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public TextMeshPro text;
    public float LifeTime = 1f;
    public Rigidbody rb;
    public void SetNumber(int damage)
    {
        rb.velocity = new Vector3(0, 0, 0);

        text.SetText(damage.ToString());

        StartAnimation();

        StartCoroutine(TimeOnScreen());
    }

    private void StartAnimation()
    {


        Vector3 dir = Random.onUnitSphere;
        float randPower = Random.Range(1.5f, 2.25f);
        dir.y = 1 * randPower;
        rb.velocity += dir * 6.5f;
    }

    private IEnumerator TimeOnScreen()
    {
        yield return new WaitForSeconds(LifeTime);

        gameObject.SetActive(false);
    }
}
