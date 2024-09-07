using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool canAttack = true;
    [SerializeField]
    private float attackRange, attackDelay;
    private void Start()
    {
        gameObject.transform.localScale = Vector3.one * attackRange;
    }
    public void Attack()
    {
        canAttack = false;
        //attack delay
        gameObject.SetActive(true);
        StartCoroutine(AttackObjectOff());
    }
    IEnumerator AttackObjectOff()
    {
        
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
        gameObject.SetActive(false);
    }
}
