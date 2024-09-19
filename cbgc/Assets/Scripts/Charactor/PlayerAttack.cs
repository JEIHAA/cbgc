using System.Collections;
using UnityEngine;
public class PlayerAttack : MonoBehaviour
{
    public bool canAttack = true;
    [SerializeField]
    private float attackRange, attackDelay;
    private float attackEffectPlayTime = 0.5f;
    public void Attack()
    {
        //block another attack while attack
        canAttack = false;
        //attack animation init
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        StartCoroutine(AttackObjectOff());
        //effect size
        gameObject.transform.localScale = Vector3.one * attackRange;
        //check enemy
        RaycastHit2D[] hitObjects = Physics2D.BoxCastAll(transform.position + Vector3.right * transform.lossyScale.x * (attackRange/2 + 1), Vector2.one * attackRange * 4, 0f, Vector2.right, 1f);
        foreach (var item in hitObjects)
            if (item.transform.CompareTag("Monster") && item.transform.TryGetComponent<Enemy>(out Enemy nowEnemy))
            {
                nowEnemy.OnDamage(5);
                nowEnemy.KnockBackFromPlayer();
            }
    }
    IEnumerator AttackObjectOff()
    {
        yield return new WaitForSeconds(attackEffectPlayTime);
        //hide attack effect
        transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(attackDelay - 0.5f);
        canAttack = true;
    }
}
