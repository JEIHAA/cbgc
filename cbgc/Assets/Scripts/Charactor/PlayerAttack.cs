using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool canAttack = true;
    [SerializeField]
    private float attackRange, attackDelay;
    private float attackDir;
    private void Start()
    {
        gameObject.transform.localScale = Vector3.one * attackRange;
    }
    public void Attack()
    {
        //block another attack while attack
        canAttack = false;
        //attack dir
        attackDir = (Player.playerTransform.position.x > Camera.main.ScreenToWorldPoint(Input.mousePosition).x ? -1 : 1);
        attackDir *= Player.playerTransform.localScale.x;
        gameObject.transform.localScale = new Vector3(attackDir, 1, 1) * attackRange;
        //attack delay
        gameObject.SetActive(true);
        StartCoroutine(AttackObjectOff());
    }
    IEnumerator AttackObjectOff()
    {
        yield return new WaitForSeconds(attackDelay);
        //can attack
        gameObject.SetActive(false);
        canAttack = true;
    }
}
