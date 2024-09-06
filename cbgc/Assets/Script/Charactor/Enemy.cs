using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class Enemy : MonoBehaviour, IDamagable
{
    public float speed; 
    public float health;
    private SpriteRenderer sr;
    private Rigidbody2D rigid;
    private Animator ani;
    private WaitForSeconds knockBackTime;
    public bool isUpdate;
    private Vector2 addVelocity;
    private Vector2 Velocity
    {
        get => rigid.velocity;
        set { if(rigid.bodyType != RigidbodyType2D.Static) rigid.velocity = value + addVelocity; sr.flipX = value.x < 0 ? true : false; }
    }
    void Start()
    {
        //component
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();

        knockBackTime = new(0.125f);

        if (isUpdate) StartCoroutine(ControlableUpdate());
    }
    //Update
    IEnumerator ControlableUpdate()
    {
        //delay
        yield return new WaitForSeconds(0.125f);
        while (gameObject.activeSelf)
        {
            //chase more bright light
            if (LightData.TorchIsBrightest) MoveToPlayer();
            else MoveToCenter();
            yield return null;
        }
    }
    //add speed for knockback
    IEnumerator MoveBackToKnockBack()
    {
        addVelocity = -Velocity * 10;
        yield return knockBackTime;
        addVelocity = Vector3.zero;
    }
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
    }
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        switch (_collision.gameObject.tag)
        {
            case "Player":
                if(!isUpdate) StartCoroutine(ControlableUpdate());
                break;
            case "PlayerAttack":
                OnDamage(5);
                if (gameObject.activeSelf) KnockBack();
                break;
            default:
                break;
        }
    }
    public void KnockBack() => StartCoroutine(MoveBackToKnockBack());
}