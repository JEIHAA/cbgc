using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireOnDamage : MonoBehaviour
{
    [SerializeField] private Bonfire bonfire;
    [SerializeField] private int fireConsumption;
    [SerializeField] private float damage;
    private Enemy enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {

            enemy = collision.gameObject.GetComponent<Enemy>();
            if(bonfire != null)
            bonfire.LeftTime -= fireConsumption;

            enemy.KnockBack();
            enemy.OnDamage(damage);
        }
    }
}
