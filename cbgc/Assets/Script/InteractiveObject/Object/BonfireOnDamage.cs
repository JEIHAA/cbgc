using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireOnDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float delay = 0.8f;
    private WaitForSeconds attackCycle;

    private Dictionary<GameObject, Coroutine> activeCoroutines = new Dictionary<GameObject, Coroutine>();

    private void Start()
    {
        attackCycle = new WaitForSeconds(delay);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            if (!activeCoroutines.ContainsKey(collision.gameObject))
            {
                Coroutine coroutine = StartCoroutine(DealDamageOverTime(collision.gameObject));
                activeCoroutines.Add(collision.gameObject, coroutine);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            if (activeCoroutines.ContainsKey(collision.gameObject))
            {
                StopCoroutine(activeCoroutines[collision.gameObject]);
                activeCoroutines.Remove(collision.gameObject);
            }
        }
    }

    private IEnumerator DealDamageOverTime(GameObject enemy)
    {
        // 적이 존재하고 죽지 않았다면 지속적으로 데미지 적용
        while (enemy != null && enemy.GetComponent<Enemy>() != null)
        {
            enemy.GetComponent<Enemy>().OnDamage(damage);
            yield return attackCycle;
        }

        if (activeCoroutines.ContainsKey(enemy))
        {
            activeCoroutines.Remove(enemy);
        }
    }
}
