using System.Collections;
using UnityEngine;
public class PlayerAttack : MonoBehaviour
{
    public bool canAttack = true;
    [SerializeField]
    private float attackRange, attackDelay;
    private float attackDir, mouseDir, attackEffectPlayTime = 0.5f;
    public void Attack()
    {
        //block another attack while attack
        canAttack = false;
        //attack dir
        mouseDir = (Player.playerTransform.position.x > Camera.main.ScreenToWorldPoint(Input.mousePosition).x ? -1 : 1);
        attackDir = mouseDir * Player.playerTransform.localScale.x;
        gameObject.transform.localScale = new Vector3(attackDir, 1, 1) * attackRange;
        //attack animation init
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        StartCoroutine(AttackObjectOff());
        //check enemy
        RaycastHit2D[] hitObjects = Physics2D.BoxCastAll(transform.position + Vector3.right * mouseDir * 2, Vector2.one * attackRange * 4, 0f, Vector2.right, 1f);
        foreach (var item in hitObjects)
            if (item.transform.CompareTag("Monster") && item.transform.TryGetComponent<Enemy>(out Enemy nowEnemy))
            {
                nowEnemy.OnDamage(5);
                nowEnemy.KnockBack(true);
            }
    }
    IEnumerator AttackObjectOff()
    {
        yield return new WaitForSeconds(0.5f);
        //hide attack effect
        transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(attackDelay - 0.5f);
        canAttack = true;
    }
}
