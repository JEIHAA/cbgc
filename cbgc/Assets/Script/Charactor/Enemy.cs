using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class Enemy : ResourceGeneratorBorder, IDamagable
{
    public float speed;
    public int health;
    SpriteRenderer sr;
    Rigidbody2D rigid;
    Animator ani;
    WaitForSeconds checkTime, knockBackTime;
    public bool moveCenterWhenStart;
    private Vector3 freePos;
    private Vector3 addVelocity;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();

        checkTime = new(1f);
        knockBackTime = new(0.125f);

        if (moveCenterWhenStart) MoveCenter();
    }
    IEnumerator MoveBackToKnockBack()
    {
        addVelocity = -rigid.velocity * 10;
        yield return knockBackTime;
        addVelocity = Vector3.zero;
    }
    private IEnumerator MoveFreePosition()
    {
        while (true)
        {
            rigid.velocity = (freePos - transform.position).normalized * speed;
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
        if (_collision.gameObject.tag == "Player") _collision.gameObject.GetComponent<Player>().OnDamage();
        if(rigid != null) rigid.velocity = Vector2.zero;
    }
    private void OnTriggerStay2D(Collider2D _collision)
    {
        if (_collision.gameObject.tag == "Player")
        {
            rigid.velocity =(Player.playerTransform.position - transform.position).normalized * speed + addVelocity;
            sr.flipX = rigid.velocity.x < 0 ? true : false;
        }
    }
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.tag == "KnockBack")
        {
            OnDamage();
            if(gameObject.activeSelf) StartCoroutine(MoveBackToKnockBack());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) MoveCenter();
    }
    void MoveCenter()
    {
        rigid.velocity = -transform.position.normalized * speed;
        sr.flipX = rigid.velocity.x < 0 ? true : false;
    }
}