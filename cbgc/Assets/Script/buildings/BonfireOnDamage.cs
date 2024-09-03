using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireOnDamage : MonoBehaviour
{
    [SerializeField] private Bonfire bonfire;
    private Enemy enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.gameObject.CompareTag("Monster"))
        {

            enemy = collision.gameObject.GetComponent<Enemy>();
            bonfire.LeftTime -= 10;

            enemy.KnockBack();
            enemy.OnDamage(3.5f);
        }
    }
}
