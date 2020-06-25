using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [HideInInspector] public GameObject projectilePrefab;
    public Camera cam;
    public Transform firePoint;

    public Image mask;

    public int maxHealth = 5;
    public float speed = 5.0f;   
    [HideInInspector] public float timeBetweenShots;

    public AudioClip shootSound;

    Rigidbody2D playerRb;
    SpriteRenderer spriteRenderer;
    AudioSource audioSource;

    Vector2 lookDirection = new Vector2(1, 0);
    Vector2 position;
    Vector2 mousePos;
    
    int currentHealth;
    float timeSinceLastShot;
    float timeInvincible = 2.0f;
    float invincibleTimer;
    float angle;
    float maskOriginalSize;
    bool IsInvincible = false;
    bool CR_running = false;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        firePoint = transform.GetChild(0);
        currentHealth = maxHealth;
        maskOriginalSize = mask.rectTransform.rect.width;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        GameManager.instance.OnRestart += RestartPlayerState;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.state == GameState.InGame)
        {            
            GetMovementInput();
            GetMouseInput();

            if (IsInvincible)
            {
                if (!CR_running)
                {
                    StartCoroutine(PlayDamageAnim());
                }
                invincibleTimer -= Time.deltaTime;
                if(invincibleTimer < 0)
                {
                    IsInvincible = false;
                }
            }
            
            if(timeSinceLastShot < 0)
            {
                if (Input.GetMouseButton(0))
                {
                    Shoot();
                }
            }
            timeSinceLastShot -= Time.deltaTime;
        }        
    }

    private void FixedUpdate()
    {
        Move();
    }

    void GetMouseInput()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        lookDirection = mousePos - playerRb.position;
        lookDirection.Normalize();
        angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90;
        playerRb.rotation = angle;
    }

    void GetMovementInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);       

        position = move.normalized * speed;        
    }

    void Move()
    {             
        playerRb.MovePosition(playerRb.position + position * Time.fixedDeltaTime);        
    }

    void Shoot()
    {
        timeSinceLastShot = timeBetweenShots;
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectile = bullet.GetComponent<Projectile>();
        projectile.Launch(lookDirection);
        audioSource.PlayOneShot(shootSound);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
        if(enemy != null)
        {
            if (!IsInvincible)
            {
                IsInvincible = true;
                invincibleTimer = timeInvincible;
                currentHealth--;
                float value = currentHealth / (float)maxHealth;
                mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maskOriginalSize * value);
                if(currentHealth < 1)
                {
                    spriteRenderer.enabled = false;
                    GameManager.instance.GameOver();
                }
            }
        }
    }
    
    void RestartPlayerState()
    {
        currentHealth = maxHealth;
        playerRb.position = new Vector2(0, 0);
        spriteRenderer.enabled = true;
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maskOriginalSize);
    }

    IEnumerator PlayDamageAnim()
    {
        CR_running = true;
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(.2f);
            
        spriteRenderer.enabled = true;
        yield return new WaitForSeconds(.2f);
        CR_running = false;
    }
}
