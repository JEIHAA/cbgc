using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] private float health, damage, delay = 0.8f;
    [SerializeField] private ObjectPoolManager.Pool pool;
    [SerializeField] private Controller controller;
    private Animator ani;
    private WaitForSeconds attackCycle;
    public void StartUpdate() => StartCoroutine(ControlableUpdate());
    //Update
    IEnumerator ControlableUpdate()
    {
        if(!ani.IsUnityNull()) yield break;
        //getComponent
        ani = GetComponent<Animator>();
        attackCycle = new WaitForSeconds(damage);
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
    public void PlayerDetected() => StartCoroutine(ControlableUpdate());
    private IEnumerator AttackingBonfire(GameObject _go)
    {
        _go.GetComponent<Bonfire>()?.OnDamage(damage);
        KnockBack();
        yield return attackCycle;
    }
    public void OnDamage(float _damage)
    {
        ani.SetTrigger("Hit");
        if ((health -= _damage) <= 0 && gameObject.activeSelf) StartCoroutine(Dying());
    }
    public void KnockBack(bool _isPlayer = false) => controller.KnockBack(_isPlayer);
    private IEnumerator Dying()
    {
        controller.CanMove = false;
        GetComponent<Collider2D>().enabled = false;
        var sr = GetComponent<SpriteRenderer>();
        float leftTime = 3;
        while (leftTime > 0)
        {
            leftTime -= Time.deltaTime;
            sr.color = Color.Lerp(Color.clear, Color.white, leftTime / 3);
            yield return null;
        }
        ObjectPoolManager.instance.GetPool(pool).Release(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IDamagable>().OnDamage(damage);
            KnockBack(); //юс╫ц
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Bonfire"))
            StartCoroutine(AttackingBonfire(collision.gameObject));
    }
}