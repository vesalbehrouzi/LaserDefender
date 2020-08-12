using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float Health = 100;
    [SerializeField] float ShootCounter;
    [SerializeField] float MinTimeBetweenShoots = 0.2f;
    [SerializeField] float MaxTimeBetweenShoots = 3f;
    [SerializeField] float ProjectileSpeed = 10f;
    [SerializeField] GameObject LaserPrefab;
    [SerializeField] GameObject ExplosionEffect;
    [SerializeField] GameObject HitEffect;
    [SerializeField] AudioClip DeathSound;
    [SerializeField] [Range(0,1)] float DeathSoundValoume=0.75f;
    [SerializeField] AudioClip ShootSound;
    [SerializeField] [Range(0, 1)] float ShootSoundValoume = 0.75f;
    [SerializeField] AudioClip HitSound;
    [SerializeField] [Range(0, 1)] float HitSoundValoume = 0.75f;


    // Start is called before the first frame update
    void Start()
    {
        ResetShootCounter();
    }

    // Update is called once per frame
    void Update()
    {
        CountdownAndShoot();
    }

    private void ResetShootCounter()
    {
        ShootCounter = UnityEngine.Random.Range(MinTimeBetweenShoots, MaxTimeBetweenShoots);
    }

    private void CountdownAndShoot()
    {
        ShootCounter -= Time.deltaTime;
        if (ShootCounter <= 0)
        {
            Fire();
            ResetShootCounter();
        }
    }

    private void Fire()
    {
        var laser = Instantiate(LaserPrefab, transform.position, Quaternion.identity);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, ProjectileSpeed*-1);
        AudioSource.PlayClipAtPoint(ShootSound, Camera.main.transform.position, ShootSoundValoume);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damageDealer = collision.GetComponent<DamageDealer>();

        if (damageDealer != null)
        {
            AudioSource.PlayClipAtPoint(HitSound, Camera.main.transform.position, HitSoundValoume);
            Health -= damageDealer.GetDamage();
            GameObject hitEffect = Instantiate(HitEffect, transform.position, transform.rotation);
            Destroy(hitEffect, 1f);
            damageDealer.Hit();
            if (Health <= 0)
                Die();
        }
        else
        {
            var player = collision.GetComponent<Player>();

            if (player != null)
            {
                player.Die();
                Die();
            }
        }


    }

    private void Die()
    {        
        Destroy(gameObject);
        GameObject explosion = Instantiate(ExplosionEffect, transform.position, transform.rotation);
        Destroy(explosion, 1f);
        AudioSource.PlayClipAtPoint(DeathSound, Camera.main.transform.position, DeathSoundValoume);
    }
}
