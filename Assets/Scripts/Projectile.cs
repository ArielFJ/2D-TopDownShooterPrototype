using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    [HideInInspector] public Level level;
    public float baseDamagePoints = 1;
    public float timeBetweenShots = .2f;
    public int price = 100;
    public Vector2 minMaxBulletForce = new Vector2(150f, 300f);
    public Color colorWhenMaxLevel;
    public float priceMultiplier = 1.25f;

    Rigidbody2D rigidbody2d;
    SpriteRenderer spriteRenderer;
    float damagePoints;
    float bulletForce;


    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        damagePoints = baseDamagePoints * ((int)level + 1);
        bulletForce = Mathf.Lerp(minMaxBulletForce.x, minMaxBulletForce.y, (float)level * .5f);
        spriteRenderer.color = Color.Lerp(Color.white, colorWhenMaxLevel, Mathf.Lerp(0, 1, (float)level * .5f ));        
    }


    private void Update()
    {
        if(GameManager.instance.state != GameState.InGame)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction)
    {
        rigidbody2d.AddForce(direction * bulletForce);
    }
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.DamageEnemy(damagePoints);
        }
    }

}
