using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject explosion;
    public GameObject damageEffect;
    public float speed = 10f;
    public float padding = 0.8f;
    public GameController gameController;
    float minX;
    float maxX;
    float minY;
    float maxY;

    public AudioSource audioSource;
    public AudioClip damageSound;
    public AudioClip explosionSound;
    public AudioClip coinSound;

    public PlayerHealthBarScript healthBarScript;
    public CoinCount CoinCount;
    public float health = 20f;
    float barFillAmount = 1f;
    float damage = 0;
    // Start is called before the first frame update
    void Start()
    {
        FindBoundaries();
        damage = barFillAmount/health;
    }

    void FindBoundaries()
    {
        Camera gameCamera = Camera.main;
        minX = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        maxX = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        minY = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        maxY = gameCamera.ViewportToWorldPoint(new Vector3(0,1, 0)).y - padding;
    }
    // Update is called once per frame
    void Update()
    {
        //float deltaX = Input.GetAxis("Horizontal")*Time.deltaTime*speed;
        //float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        //float newXpos =  Mathf.Clamp(transform.position.x + deltaX,minX,maxX);
        //float newYpos = Mathf.Clamp(transform.position.y + deltaY,minY,maxY);

        //transform.position = new Vector2(newXpos, transform.position.y);
        //transform.position = new Vector2(transform.position.x, newYpos);
        
        if (Input.GetMouseButton(0))
        {
            Vector2 newPos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            transform.position = Vector2.Lerp(transform.position, newPos, 10*Time.deltaTime );
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyBullet")
        {
            audioSource.PlayOneShot(damageSound, 0.5f);
            damagePlayerHealthbar();
            Destroy(collision.gameObject);
            GameObject DamageVFX = Instantiate(damageEffect, collision.transform.position, Quaternion.identity);
            Destroy(DamageVFX, 0.05f);
            if (health<=0)
            {
                AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, 0.5f);
                gameController.GameOver();
                Destroy(gameObject);
                GameObject blast = Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(blast, 2f);
            }
        }

        if(collision.gameObject.tag == "Coin")
        {
            audioSource.PlayOneShot(coinSound, 0.5f);
            Destroy(collision.gameObject);
            CoinCount.AddCount();
        }
    }

    void damagePlayerHealthbar()
    {
        if (health > 0)
        {
            health -= 1;
            barFillAmount = barFillAmount - damage;
            healthBarScript.SetAmount(barFillAmount);
        }
    }
}
