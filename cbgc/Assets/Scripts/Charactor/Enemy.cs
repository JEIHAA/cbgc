using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] private float speed;
    [SerializeField] private float health;
    [SerializeField] private float damage;
    [SerializeField] private float delay = 0.8f;
    [SerializeField] private ObjectPoolManager.Pool pool;
    private SpriteRenderer sr;
    private Rigidbody2D rigid;
    private Animator ani;
    private WaitForSeconds knockBackTime, attackCycle;
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
        attackCycle = new WaitForSeconds(damage);

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
    IEnumerator MoveBackToKnockBack(bool isPlayer = false)
    {
        if(isPlayer) addVelocity = -(Player.playerTransform.position - transform.position).normalized * 10;
        else addVelocity = -Velocity * 10;
        yield return knockBackTime;
        addVelocity = Vector3.zero;
    }
    void MoveToPlayer() => Velocity = (Player.playerTransform.position - transform.position).normalized * speed;
    void MoveToCenter() => Velocity = -transform.position.normalized * speed;
    public void OnDamage(float _damage) {
        health -= _damage;
        ani.SetTrigger("Hit");
        if (health <= 0 && gameObject.activeSelf) StartCoroutine(Dying());
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
        ObjectPoolManager.instance.GetPool(pool).Release(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.gameObject.CompareTag("Player"))
            _collision.gameObject.GetComponent<Player>().OnDamage(100);
    }
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.CompareTag("Player") && !isUpdate) StartCoroutine(ControlableUpdate());
        if (_collision.gameObject.CompareTag("PlayerAttack"))
        {
            OnDamage(5);
            if (gameObject.activeSelf) KnockBack(true);
        }
        if (_collision.gameObject.layer == LayerMask.NameToLayer("Bonfire"))
        {
            StartCoroutine(AttackingBonfire(_collision.gameObject));
        }
    }

    private IEnumerator AttackingBonfire(GameObject _go)
    {
        _go.GetComponent<Bonfire>()?.OnDamage(damage);
        KnockBack();
        yield return attackCycle;
    }
    public void KnockBack(bool isPlayer = false) => StartCoroutine(MoveBackToKnockBack(isPlayer));
}