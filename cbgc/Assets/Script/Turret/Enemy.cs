using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    public float speed, attackDelay;
    Rigidbody2D rigid;
    public Vector2 baseCampPos;
    WaitForSeconds checkTime, attackDelayTime;
    List<Transform> nearObjs;
    bool canAttack = true;
    // Start is called before the first frame update
    void Start()
    {
        nearObjs = new List<Transform>();
        rigid = GetComponent<Rigidbody2D>();
        checkTime = new(.25f);
        attackDelayTime = new(attackDelay);
        if(baseCampPos == null) { baseCampPos = new Vector2(0, 0); }
        StartCoroutine(CheckPath());
    }
    IEnumerator CheckAttack()
    {
        canAttack = false;
        yield return attackDelayTime;
        canAttack = true;
    }
    IEnumerator CheckPath()
    {
        while (true)
        {
            yield return checkTime;
            if (nearObjs.Count > 0)
            {
                if (nearObjs[0] == null) nearObjs.Remove(nearObjs[0]);
                else rigid.velocity = (nearObjs[0].position - transform.position).normalized * speed;
            }
            else rigid.velocity = -transform.position.normalized * speed;
        }
    }
    public void OnDamage() { Debug.Log($"{gameObject.name} On Damage"); }
    
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        nearObjs.Add(_collision.transform);
        if(_collision.gameObject.tag == "Bullet")
        {
            OnDamage();
        }
        if (_collision.gameObject.tag == "Turret")
        {
            _collision.GetComponent<Turret>().enemyList.Add(this);
        }
    }
    private void OnTriggerExit2D(Collider2D _collision)
    {
        nearObjs.Remove(_collision.transform);
        if (_collision.gameObject.tag == "Turret")
        {
            _collision.GetComponent<Turret>().enemyList.Remove(this);
        }
    }
}
