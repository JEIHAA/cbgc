using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] private float health;
    [SerializeField] private float damage;
    [SerializeField] private float delay = 0.8f;
    [SerializeField] private ObjectPoolManager.Pool pool;
    [SerializeField] private Controller controller;
    private SpriteRenderer sr;
    private Rigidbody2D rigid;
    private Animator ani;
    private WaitForSeconds attackCycle;
    public bool isUpdate;
    
    void Start()
    {
        //component
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
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
            if (LightData.TorchIsBrightest) controller.MoveToPlayer();
            else controller.MoveToCenter();
            yield return null;
        }
    }
    public void OnDamage(float _damage) {
        health -= _damage;
        ani.SetTrigger("Hit");
        if (health <= 0 && gameObject.activeSelf) StartCoroutine(Dying());
    }
    IEnumerator Dying()
    {
        controller.CanMove = false;
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
    public void KnockBack(bool _isPlayer = false) => controller.KnockBack(_isPlayer);
}