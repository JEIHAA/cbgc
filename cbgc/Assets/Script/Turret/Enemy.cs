using System.Collections;
using UnityEngine;
public class Enemy : MonoBehaviour, IDamagable
{
    public float speed, attackDelay;
    public int health;
    Rigidbody2D rigid;
    Animator ani;
    WaitForSeconds checkTime;
    bool freeze = true;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        ani.speed = Random.value * 0.25f;
    }
    IEnumerator CheckPath()
    {
        while (true)
        {
            yield return checkTime;
            rigid.velocity = (Player.playerTransform.position - transform.position).normalized * speed;
        }
    }
    public void OnDamage() {
        
        --health;
        if (health <= 0) gameObject.SetActive(false);
        Debug.Log($"{gameObject.name} On Damage");
    }
    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.gameObject.tag == "Player")
        {
            _collision.gameObject.GetComponent<Player>().OnDamage();
        }
    }
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (freeze && _collision.gameObject.tag == "Player") 
        {
            freeze = false;
            if(ani != null) ani.speed = 2;
            checkTime = new(.25f);
            StartCoroutine(CheckPath());
        }
    }
}
