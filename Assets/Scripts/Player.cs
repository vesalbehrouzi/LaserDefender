using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] float MoveSpeed = 10f;
    [SerializeField] float Padding = 0.5f;
    [SerializeField] GameObject LaserPrefab;
    [SerializeField] float ProjectileSpeed = 10f;
    [SerializeField] float ProjectileFiringPeriod = 0.05f;
    [SerializeField] float Health = 100;
    [SerializeField] GameObject ExplosionEffect;
    [SerializeField] AudioClip DeathSound;
    [SerializeField] [Range(0, 1)] float DeathSoundValoume = 0.75f;
    [SerializeField] AudioClip ShootSound;
    [SerializeField] [Range(0, 1)] float ShootSoundValoume = 0.75f;

    Coroutine firingCoroutine;
    bool isFiring = false;

    float minX, MaxX, minY, MaxY;

    void Start()
    {
        SetupViewportBoundries();
    }
    private void SetupViewportBoundries()
    {
        Camera gameCamera = Camera.main;

        Vector3 minVector = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 maxVector = gameCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        minX = minVector.x + Padding;
        MaxX = maxVector.x - Padding;

        minY = minVector.y + Padding;
        MaxY = maxVector.y - Padding;

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }
    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (isFiring)
                return;
            firingCoroutine =  StartCoroutine(FireCouroutine());
        }

        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
            isFiring = false;
        }
    }

    private IEnumerator FireCouroutine()
    {
        while (true)
        {
            isFiring = true;
            var laser = Instantiate(LaserPrefab, transform.position, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, ProjectileSpeed);
            AudioSource.PlayClipAtPoint(ShootSound, Camera.main.transform.position, ShootSoundValoume);
            yield return new WaitForSeconds(ProjectileFiringPeriod);
        }        
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal");
        var deltaY = Input.GetAxis("Vertical");
        var newXPos = Mathf.Clamp(transform.position.x + deltaX * Time.deltaTime * MoveSpeed, minX, MaxX);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY * Time.deltaTime * MoveSpeed, minY, MaxY);
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var component = collision.GetComponent<DamageDealer>();

        if (component != null)
        {
            Health -= component.GetDamage();
            component.Hit();

            if (Health <= 0)
                Die();
        }
            
    }

    public void Die()
    {
        Destroy(gameObject);
        GameObject explosion = Instantiate(ExplosionEffect, transform.position, transform.rotation);
        Destroy(explosion, 1f);
        AudioSource.PlayClipAtPoint(DeathSound, Camera.main.transform.position, DeathSoundValoume);
    }
}
