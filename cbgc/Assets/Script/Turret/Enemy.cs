using System.Collections;
using UnityEngine;
public class Enemy : MonoBehaviour, IDamagable
{
    public float speed, attackDelay;
    float untouchableTime;
    public int health;
    Rigidbody2D rigid;
    WaitForSeconds checkTime;
    bool freeze = true;
    // Start is called before the first frame update
    void Start()
    {
        untouchableTime = 0.5f;
        rigid = GetComponent<Rigidbody2D>();
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
            checkTime = new(.25f);
            StartCoroutine(CheckPath());
        }
    }
}
