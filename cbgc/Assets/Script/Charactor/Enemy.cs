using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class Enemy : ResourceGeneratorBorder, IDamagable
{
    public float speed, attackDelay;
    float leftTime;
    public int health;
    SpriteRenderer sr;
    Rigidbody2D rigid;
    Animator ani;
    WaitForSeconds checkTime;
    bool freeze = true;

    private Vector3 freePos;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
    }
    IEnumerator CheckPath()
    {
        leftTime = 0.25f;
        while (true)
        {
            if (leftTime < 0f)
            {
                rigid.velocity = rigid.velocity * 0.125f + (Vector2)((Player.playerTransform.position - transform.position).normalized * speed);
                sr.flipX = rigid.velocity.x < 0 ? true : false;
                yield return checkTime;
                leftTime = 0.5f;
            }
            else leftTime -= 0.25f;
        }
    }

    private IEnumerator MoveFreePosition()
    {
        while (true)
        {
            rigid.velocity = rigid.velocity * 0.125f + (Vector2)((freePos - transform.position).normalized * speed);
            sr.flipX = rigid.velocity.x < 0 ? true : false;
            yield return checkTime;
        }
    }
    public void OnDamage() {
        --health;
        ani.SetTrigger("Hit");
        if (health <= 0) gameObject.SetActive(false);
        Debug.Log($"{gameObject.name} On Damage");
    }
    private void OnCollisionEnter2D(Collision2D _collision)
    {
        
        if (_collision.gameObject.tag == "Player")
        {
            _collision.gameObject.GetComponent<Player>().OnDamage();
        }
        if(rigid != null) rigid.velocity = Vector2.zero;
    }
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (freeze && _collision.gameObject.tag == "Player") 
        {
            StopCoroutine(MoveFreePosition());
            freeze = false;
            checkTime = new(.25f);
            StartCoroutine(CheckPath());
        }
        if (_collision.gameObject.tag == "KnockBack")
        {
            leftTime += 0.5f;
            rigid.velocity = (Vector2)(transform.position - Player.playerTransform.position).normalized * 10;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StopCoroutine(CheckPath());
            freePos = SetRendomPosValue();
            //Debug.Log("freePos"+freePos);
            StartCoroutine(MoveFreePosition());
            freeze = true;
        }
    }
}
