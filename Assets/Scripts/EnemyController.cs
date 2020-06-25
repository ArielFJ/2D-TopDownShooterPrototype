using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float maxHealth = 3.0f;
    public float speed = 1.0f;
    public Vector2 scorePointsRange = new Vector2(1, 7);

    public Rigidbody2D playerRb;
    public Image healthFrameMask;

    Rigidbody2D rigidbody2d;

    float currentHealth;
    float originalHealthFrameMaskSize;
    float minDistance = .3f;
    static int diffLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.totalScore / 30 == diffLevel + 1)
        {
            diffLevel++;
            speed *= 1.3f;
        }
        //playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        originalHealthFrameMaskSize = healthFrameMask.rectTransform.rect.width;
    }

    private void Update()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(GameManager.instance.state == GameState.InGame)
        {
            if(playerRb != null)
            {
                Vector2 playerPos = playerRb.position;
                Vector2 direction = playerPos - rigidbody2d.position;

                //Debug.Log(playerPos);

                Debug.DrawRay(rigidbody2d.position, playerPos.normalized * 3);        

                if (Vector2.Distance(playerPos, rigidbody2d.position) > minDistance)
                {
                    //if GameObject has RigidBody, we have to move the RigidBody, not the Transform component of the GameObject
                    //transform.Translate(direction.normalized * speed * Time.deltaTime);
                    rigidbody2d.MovePosition(rigidbody2d.position + direction.normalized * speed * Time.deltaTime);
                }
            }
        }
    }

    public void DamageEnemy(float damageValue) 
    {

        currentHealth -= damageValue;
        float value = currentHealth / (float)maxHealth;
        healthFrameMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalHealthFrameMaskSize * value);
        if(currentHealth < 1)
        {
            //GameManager.instance.ChangeScore((int)(maxHealth * ((diffLevel + 1) * .75f)));
            GameManager.instance.ChangeScore((int)Mathf.Clamp(maxHealth * ((diffLevel + 1) * .25f), scorePointsRange.x, scorePointsRange.y), true);
            Spawner.instance.RemoveObjectFromList(gameObject);
            Destroy(gameObject);
        }
    }
}
