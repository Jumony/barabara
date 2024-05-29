using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolverBulletBehaviour : MonoBehaviour, IPooledObject
{
    public Rigidbody2D rb;
    public PlayerWeaponType bulletData;

    private float projectileSpeed;
    private float projectileDamage;
    private float startTime;

    // Originally the start method but now implements the interface
    // We have to use the interface method because we are no longer instantiating
    //  By instantiating, it would be appropriate to have the start method but
    //  since we are using a queue and activating and deactivating, we cannot
    //  rely on the start method because it can only run on a script ONCE.
    public void OnObjectSpawn()
    {
        projectileSpeed = bulletData.projectileSpeed;
        projectileDamage = bulletData.damage;
        startTime = Time.time;

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameObject.Find("Tilemap_Water").GetComponent<Collider2D>());

        // Get the layer mask for the "Bullet" layer
        int bulletLayer = LayerMask.NameToLayer("IgnoreBulletCollision");

        // Ignore collisions between the current bullet and other bullets
        Physics2D.IgnoreLayerCollision(gameObject.layer, bulletLayer);
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time - startTime >= 10f)
        {
            gameObject.SetActive(false);
        }
        rb.velocity = transform.up * projectileSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            BasicEnemy enemyScript = collision.gameObject.GetComponent<BasicEnemy>();
            enemyScript.TakeDamage(projectileDamage);
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
