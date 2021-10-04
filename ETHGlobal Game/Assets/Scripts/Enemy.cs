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

    public Animator anim;

    public DungeonRoom belongsToRoom;
    private void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        ToggleHealthBar(false);

        target = GetComponent<Target>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        belongsToRoom = GetComponentInParent<DungeonRoom>();
        belongsToRoom.AllEnemiesInRoom.Add(this);
    }

    public void TakeDamage(int damage)
    {
        
        Vector3 damagePos = transform.position;
        damagePos.y += 0.5f;
        GameObject damageNumber =  ObjectPooler.DamageNumber.SetObject(damagePos);
        anim.SetTrigger("isDamaged");
        damageNumber.GetComponent<DamageNumber>().SetNumber(damage);

     

        ToggleHealthBar(true);

        currentHealth -= damage;

       

        EnemyRenderer.DOShakeScale(0.1f).OnComplete(ResetScale);
        void ResetScale()
        {
            EnemyRenderer.localScale = new Vector3(1, 1, 1);
        }

        if (currentHealth <= 0 && !hasDied)
        {
            CameraController.cameraController.Shake(Random.onUnitSphere, 1f, 0.05f);
            EnemyDied();
        }
        else
        {
            UpdateHealthBar();
            CameraController.cameraController.Shake(Random.onUnitSphere, 0.35f, 0.05f);
        }
    }

    public void EnemyDied()
    {
        hasDied = true;
        target.SetNewTarget();

        belongsToRoom.AllEnemiesInRoom.Remove(this);

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
