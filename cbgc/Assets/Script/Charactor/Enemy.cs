using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class Enemy : MonoBehaviour, IDamagable
{
    public float speed; 
    public float health;
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

        if (moveCenterWhenStart) Invoke("MoveCenter",0.5f);
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
    public void OnDamage(float _damage) {
        health -= _damage;
        ani.SetTrigger("Hit");
        if (health <= 0) StartCoroutine(Dying());
    }
    IEnumerator Dying()
    {
        rigid.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
        float leftTime = 3;
        while (leftTime > 0)
        {
            leftTime -= Time.deltaTime;
            sr.color = Color.Lerp(Color.clear,Color.white,leftTime/3);
            yield return null;
        }
        gameObject.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.gameObject.tag == "Player") _collision.gameObject.GetComponent<Player>().OnDamage(100);
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
        if (_collision.gameObject.tag == "PlayerAttack")
        {
            OnDamage(5);
            if(gameObject.activeSelf) KnockBack();
        }
    }
    private void OnTriggerExit2D(Collider2D _collision)
    { if (_collision.gameObject.CompareTag("Player")) MoveCenter(); }
    void MoveCenter()
    {
        rigid.velocity = -transform.position.normalized * speed;
        sr.flipX = rigid.velocity.x < 0 ? true : false;
    }
    public void KnockBack() 
    {
        StartCoroutine(MoveBackToKnockBack());
    }
}