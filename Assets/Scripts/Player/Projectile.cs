using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("General Projectile Stats")]
    [SerializeField] private float projectileLifetime = 5.0f;
    [SerializeField] private float projectileDamage = 1.0f;
    [SerializeField] private float projectileSpeed = 1.0f;

    private Vector2 direction;
    private Vector2 velocity;
    private const float gravity = -9.81f;

    // Start is called before the first frame update
    private void Start()
    {
        direction = transform.right;
        velocity = direction * projectileSpeed;
        SetDestroyTime();
    }

    // Update is called once per frame
    private void Update()
    {
        ApplyGravity();
        SimulateProjectileMovement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();
        if (iDamageable != null)
        {
            iDamageable.Damage(projectileDamage);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }

    private void SetDestroyTime()
    {
        Destroy(gameObject, projectileLifetime);
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }

    private void SimulateProjectileMovement()
    {
        Vector2 newPosition = (Vector2)transform.position + velocity * Time.deltaTime;
        transform.position = newPosition;
    }

    public void SetSpeed(float speed)
    {
        projectileSpeed = speed;
        velocity = direction * projectileSpeed;
    }
}
