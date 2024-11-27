using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyScript : MonoBehaviour
{
    public Transform []GunPoint;
    public GameObject EnemyBullet;
    public GameObject EnemyFlash;
    public GameObject EnemyExplosionPrefab;
    public GameObject DamageEffect;
    public GameObject coinPrefab;
    public Healthbar healthbar;
    public float bulletSpawnTime = 1f;
    public float speed = 1f;
    public float health = 10f;

    public AudioClip bulletSound;
    public AudioClip damageSound;
    public AudioClip explosionSound;

    public AudioSource audioSource;

    float barSize = 1f;
    float damage = 0;
    // Start is called before the first frame update
    void Start()
    {
        EnemyFlash.SetActive(false);
        StartCoroutine(Shoot());
        damage = barSize / health;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerBullet")
        {
            audioSource.PlayOneShot(damageSound);
            DamageHealthBar();
            Destroy(collision.gameObject);
            GameObject DamageVFX = Instantiate(DamageEffect, collision.transform.position, Quaternion.identity);
            Destroy(DamageVFX, 0.05f);
            if (health <= 0)
            {
                AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position , 0.5f);
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
                GameObject enemyExplosion = Instantiate(EnemyExplosionPrefab, transform.position, Quaternion.identity);
                Destroy(enemyExplosion, 0.4f);
            }
        }
    }
    void DamageHealthBar()
    {
        if(health > 0)
        {
            health -= 1;
            barSize=barSize-damage;
            healthbar.SetSize(barSize);
        }
    }
    void Fire()
    {
        for (int i = 0; i < GunPoint.Length; i++)
        {
            Instantiate(EnemyBullet, GunPoint[i].position , Quaternion.identity);
        }
        //Instantiate(EnemyBullet, GunPoint1.position, Quaternion.identity);
        //Instantiate(EnemyBullet, GunPoint2.position, Quaternion.identity);
    }

    IEnumerator Shoot()
    {
        while (true)
        {

            yield return new WaitForSeconds(bulletSpawnTime);
            Fire();
            audioSource.PlayOneShot(bulletSound, 0.5f);
            EnemyFlash.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            EnemyFlash.SetActive(false);
        }
    }
}
