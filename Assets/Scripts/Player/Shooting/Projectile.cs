using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    private float damage;
    private float lifetime;

    [SerializeField]
    private ParticleSystem explosionPrefab;

    private Rigidbody rb;

    public void SetParams(float _speed, float _damage, float _lifetime)
    {
        speed = _speed;
        damage = _damage;
        lifetime = _lifetime;
    }

    public void Shoot()
    {
        Debug.Log(speed);
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.forward * speed);
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
            DestroyProjectile();
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.GetComponentInParent<Enemy>() != null)
            {
                SpawnVFX();
                Enemy enemy = other.GetComponentInParent<Enemy>();
                enemy.Hit(damage);
                DestroyProjectile();
            }
        }
        else if (other.gameObject.tag == "StartButton")
        {
            if (other.GetComponentInParent<StartButton>() != null)
            {
                StartButton button = other.GetComponentInParent<StartButton>();
                button.Hit();
            }
        }
        else if (other.gameObject.tag != "Player")
        {
            SpawnVFX();
            DestroyProjectile();
        }
    }

    private void SpawnVFX()
    {
        ParticleSystem explosion = Instantiate(
            explosionPrefab,
            this.transform.position,
            Quaternion.Euler(Vector3.zero)
        );
    }

    private void DestroyProjectile()
    {
        Destroy(this.gameObject);
    }
}
