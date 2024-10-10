using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBulletBehaviour : MonoBehaviour, IPooledObject
{
    public Rigidbody2D rb;
    public PlayerWeaponType bulletData;

    protected float projectileSpeed;
    protected float projectileDamage;
    public float bulletLifetime;
    protected float startTime;

    // Originally the start method but now implements the interface
    // We have to use the interface method because we are no longer instantiating
    //  By instantiating, it would be appropriate to have the start method but
    //  since we are using a queue and activating and deactivating, we cannot
    //  rely on the start method because it can only run on a script ONCE.
    public virtual void OnObjectSpawn()
    {
        projectileSpeed = bulletData.projectileSpeed;
        projectileDamage = bulletData.damage;
        bulletLifetime = bulletData.projectileLifetime;
        startTime = Time.time;

        // Get the layer mask for the "Bullet" layer
        int bulletLayer = LayerMask.NameToLayer("IgnoreBulletCollision");

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameObject.Find("Tilemap_Water").GetComponent<Collider2D>());

        // Ignore collisions between the current bullet and other bullets
        Physics2D.IgnoreLayerCollision(gameObject.layer, bulletLayer);

        // Ignore collisions between the current bullet and the player layer
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"));
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Time.time - startTime >= bulletLifetime)
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void FixedUpdate()
    {
        rb.velocity = projectileSpeed * Time.fixedDeltaTime * transform.up;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(projectileDamage);
            }
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Hit" + collision.gameObject.name);
            gameObject.SetActive(false);
        }
    }
}
