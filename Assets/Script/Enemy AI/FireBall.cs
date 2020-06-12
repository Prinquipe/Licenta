using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public Transform Target;
    public float speed;
    public const int PLAYER_DAMAGE = 1;

    private Rigidbody2D rigidBody;

    void Awake()
    {
        rigidBody = (Rigidbody2D)gameObject.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Vector2 moveDirection;
        float angle;
        Vector2 targetPos;
        targetPos.x = Target.position.x - gameObject.transform.position.x;
        targetPos.y = Target.position.y - gameObject.transform.position.y;
        angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));
        moveDirection = (Target.position - transform.position).normalized * speed;
        rigidBody.velocity = new Vector2(moveDirection.x, moveDirection.y);
    }
    // Update is called once per frame
    void Update()
    {
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerDamage"))
        {
            Debug.Log("Destroy Object. Contact with Player");
            PlayerInteraction inter;
            inter = (PlayerInteraction)other.gameObject.GetComponent<PlayerInteraction>();
            inter.TakeDamage(PLAYER_DAMAGE);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Platform"))
        {
            Debug.Log("Destroy Object. Contact with Platform");
            Destroy(gameObject);
        }
    }
}
