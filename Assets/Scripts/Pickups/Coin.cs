using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Coin : MonoBehaviour, IPooledObject
{
    public Rigidbody2D rb;

    private Tilemap rockTilemap;
    private Tilemap waterTilemap;

    private void Start()
    {
        rockTilemap = GameObject.Find("Tilemap_Rock").GetComponent<Tilemap>();
        waterTilemap = GameObject.Find("Tilemap_Water").GetComponent<Tilemap>();
    }
    public void OnObjectSpawn()
    {
        Vector2 randomVector = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
        rb.AddForce(randomVector, ForceMode2D.Impulse);

    }

    private Vector2 GetCollisionNormal(Collider2D collider)
    {
        Vector2 collisionPoint = collider.ClosestPoint(transform.position);
        Vector2 normal = (transform.position - (Vector3)collisionPoint).normalized;
        return normal;
    }

    private void ReflectCoin(Vector2 normal)
    {
        Vector2 currentVelocity = rb.velocity;
        Vector2 reflectedVelocity = Vector2.Reflect(currentVelocity, normal);
        rb.velocity = reflectedVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == rockTilemap.gameObject || collision.gameObject == waterTilemap.gameObject)
        {
            Vector2 collisionNormal = GetCollisionNormal(collision);
            ReflectCoin(collisionNormal);
            Debug.Log("Hit: " + collision.gameObject.name);
        }
    }

}
