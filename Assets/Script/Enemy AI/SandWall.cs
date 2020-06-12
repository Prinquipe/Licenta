using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWall : MonoBehaviour
{
    public Transform destroyPoint;
    public float speed;
    public bool movesRight;
    public BoxCollider2D triggerBox;
    public const int PLAYER_DAMAGE = 1;
    public const float minDistance = 0.1f;
    public const float GROWTIME = .33f;

    private float GrowTime;
    private Rigidbody2D rigidBody;
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        triggerBox.enabled = false;
        rigidBody = (Rigidbody2D)gameObject.GetComponent<Rigidbody2D>();
        animator = (Animator)gameObject.GetComponent<Animator>();
        GrowTime = GROWTIME;
    }


    void Start()
    {
        if (movesRight)
        {
            Flip();
        }
    }
    // Update is called once per frame
    void Update()
    {
        Grow();
        rigidBody.position = Vector2.MoveTowards(rigidBody.position, new Vector2(destroyPoint.position.x, rigidBody.position.y), speed * Time.deltaTime);

        if (Mathf.Abs(destroyPoint.position.x - rigidBody.position.x) <= minDistance)
        {
            Destroy(gameObject);
        }
    }

    void Grow()
    {
        GrowTime -= Time.deltaTime;
        if (GrowTime <= 0)
        {
            triggerBox.enabled = true;
            Debug.Log("Setting is Traveling true");
            animator.SetBool("isTraveling", true);
        }
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerDamage"))
        {
            if (other.CompareTag("PlayerDamage"))
            {
                PlayerInteraction inter;
                inter = (PlayerInteraction)other.gameObject.GetComponent<PlayerInteraction>();
                inter.TakeDamage(PLAYER_DAMAGE);
            }
        }

    }

    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


}
