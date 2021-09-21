using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 dir;
    public float speed;
    public float lifeTime = 2.5f;

    public void SetUp(Vector3 _dir)
    {
        dir = _dir;
        dir.y = 0;
        StartCoroutine(LifeTime());
    }

    void Move()
    {
        Vector3 tempPos = transform.position; 
        tempPos += dir * speed * Time.deltaTime; 
        transform.position = tempPos; 
    }

    private void Update()
    {
        Move();
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(lifeTime);

        KillProjectile();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            KillProjectile();

        }
        else if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(1);
            KillProjectile();
        }
    }

    private void KillProjectile()
    {
        Destroy(gameObject);
    }
}
