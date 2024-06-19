using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBulletBehaviour : MonoBehaviour, IPooledObject
{
    public Rigidbody2D rb;
    public PlayerWeaponType bulletData;

    private float projectileSpeed;
    private float projectileDamage;
    public float bulletLifetime = 0.5f;
    private float startTime;

    public void OnObjectSpawn()
    {
        projectileSpeed = bulletData.projectileSpeed;
        projectileDamage = bulletData.damage;
        startTime = Time.time;

        int bulletLayer = LayerMask.NameToLayer("IgnoreBulletCollision");
        Physics2D.IgnoreLayerCollision(gameObject.layer, bulletLayer);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameObject.Find("Tilemap_Water").GetComponent<Collider2D>());
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > bulletLifetime)
        {
            gameObject.SetActive(false);
        }
        rb.velocity = transform.up * projectileSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            damageable.TakeDamage(projectileDamage);
            Debug.Log("Collision found");
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
