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
    public bool isUpdate;
    private Vector3 freePos;
    private Vector3 addVelocity;
    private Vector3 Velocity
    {
        get => rigid.velocity;
        set { rigid.velocity = value + addVelocity; sr.flipX = value.x < 0 ? true : false; }
    }
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();

        checkTime = new(1f);
        knockBackTime = new(0.125f);

        if (isUpdate) StartCoroutine(ControlableUpdate());
    }
    IEnumerator ControlableUpdate()
    {
        yield return new WaitForSeconds(0.125f);
        while (gameObject.activeSelf)
        {
            if (LightData.TorchIsBrightest) MoveToPlayer();
            else MoveToCenter();
            yield return null;
        }
    }
    IEnumerator MoveBackToKnockBack()
    {
        addVelocity = -rigid.velocity * 10;
        yield return knockBackTime;
        addVelocity = Vector3.zero;
    }
    void MoveFreePosition() => Velocity = (freePos - transform.position).normalized * speed;
    void MoveToPlayer() => Velocity = (Player.playerTransform.position - transform.position).normalized * speed;
    void MoveToCenter() => Velocity = -transform.position.normalized * speed;
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
        if (_collision.gameObject.tag == "Player")
            _collision.gameObject.GetComponent<Player>().OnDamage(100);
        if(rigid != null) rigid.velocity = Vector2.zero;
    }
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.tag == "Player" && !isUpdate)
        {
            StartCoroutine(ControlableUpdate());
        }
        if (_collision.gameObject.tag == "PlayerAttack")
        {
            OnDamage(5);
            if(gameObject.activeSelf) KnockBack();
        }
    }
    public void KnockBack() => StartCoroutine(MoveBackToKnockBack());
}