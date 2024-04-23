using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] LayerMask blockLayer;
    [SerializeField] GameManager gameManager;

    public enum DIRECTION_TYPE
    {
        STOP,
        RIGHT,
        LEFT,
    }
    DIRECTION_TYPE direction = DIRECTION_TYPE.STOP;

    Rigidbody2D rigidbody2D;
    Animator animator;
    float speed;
    float jumpPower = 400;
    bool isDeath = false;

    private void Start()
    {;
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDeath)
        {
            return;
        }
        float x = Input.GetAxis("Horizontal");
        animator.SetFloat("speed", Mathf.Abs(x));

        if (x == 0)
        {
            //tomatteiru
            direction = DIRECTION_TYPE.STOP;

        } else if (x > 0)
        {
            direction = DIRECTION_TYPE.RIGHT;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (x < 0)
        {
            direction = DIRECTION_TYPE.LEFT;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        //jump space
        if (IsGround())
        {
            if (Input.GetKeyDown("space"))
            {
                Jump();
            }
            else
            {
                animator.SetBool("isJump", false);
            }

        }
    }

    private void FixedUpdate()
    {
        if (isDeath)
        {
            return;
        }
        switch (direction)
        {
            case DIRECTION_TYPE.STOP:
                speed = 0;
                break;
            case DIRECTION_TYPE.RIGHT:
                speed = 5;
                break;
            case DIRECTION_TYPE.LEFT:
                speed = -5;
                break;
        }
        rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
    }

    void Jump()
    {
        rigidbody2D.AddForce(Vector2.up * jumpPower);
        Debug.Log("jumping");
        animator.SetBool("isJump",true);
    }

    bool IsGround()
    {
        Vector3 leftStartPoint = transform.position - Vector3.right * 0.2f;
        Vector3 rightStartPoint = transform.position + Vector3.right * 0.2f;
        Vector3 endPoit = transform.position - Vector3.up * 0.1f;
        Debug.DrawLine(leftStartPoint, endPoit);
        Debug.DrawLine(rightStartPoint, endPoit);

        return Physics2D.Linecast(leftStartPoint, endPoit, blockLayer) || Physics2D.Linecast(rightStartPoint, endPoit, blockLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDeath)
        {
            return;
        }
        if (collision.gameObject.tag == "Trap")
        {
            PlayerDeath();
        }
        if (collision.gameObject.tag == "Finish")
        {
            Debug.Log("GAME CLEAR");
            gameManager.GameClear();
        }
        if (collision.gameObject.tag == "Item")
        {
            Debug.Log("Get Item");
            collision.gameObject.GetComponent<ItemManager>().GetItem();
        }
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyManager enemy = collision.gameObject.GetComponent<EnemyManager>();
            if (this.transform.position.y +0.2f > enemy.transform.position.y)
            {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x,0);
                Jump();
                enemy.DestroyEnemy();
            }
            else
            {
                PlayerDeath(); 
            }
        }
    }
    void PlayerDeath()
    {
        isDeath = true;
        rigidbody2D.velocity = new Vector2(0, 0);
        rigidbody2D.AddForce(Vector2.up * jumpPower);
        animator.Play("PlayerDeathAnimation");
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        Destroy(boxCollider2D);
        gameManager.GameOver();
    }
}
