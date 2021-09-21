using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int currentHealth;
    public int maxHealth;
    [Space(10)]
    private Target target;
    [Header("Enemy Render")]
    public Transform EnemyRenderer;
    [Space(10)]
    private bool hasDied = false;
    [Header("Enemy HealthBar")]
    public Transform HealthBarCanvas;
    public Image HealthBarFiller;

    private void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        ToggleHealthBar(false);

        target = GetComponent<Target>();
    }

    public void TakeDamage(int damage)
    {
        Vector3 damagePos = transform.position;
        damagePos.y += 0.5f;
        GameObject damageNumber =  ObjectPooler.DamageNumber.SetObject(damagePos);

        damageNumber.GetComponent<DamageNumber>().SetNumber(damage);


        ToggleHealthBar(true);

        currentHealth -= damage;

       

        EnemyRenderer.DOShakeScale(0.1f);

        if (currentHealth <= 0 && !hasDied)
        {
            EnemyDied();
        }
        else
        {
            UpdateHealthBar();
        }
    }

    public void EnemyDied()
    {
        hasDied = true;
        target.RemoveTarget();


        Destroy(gameObject);
    }

    private void UpdateHealthBar()
    {
        float curM = currentHealth / 100f;
        float maxM = maxHealth / 100f;

        float holder = curM / maxM;

        HealthBarFiller.DOFillAmount(holder,0.1f);
    }

    public void ToggleHealthBar(bool toggle)
    {
        HealthBarCanvas.gameObject.SetActive(toggle);
    }
}
