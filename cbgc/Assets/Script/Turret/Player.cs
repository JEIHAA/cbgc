using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour, IDamagable
{
    [SerializeField]
    GameObject playerSprite;
    Animator ani;
    SpriteRenderer sr;
    Rigidbody2D rigid;
    public static Transform playerTransform;
    public float AttackRange;
    Vector2 moveVec = Vector2.zero;
    Vector2 MoveVec
    {
        get { return moveVec; }
        set
        {
            if (value.magnitude > 1) moveVec = value.normalized;
            else moveVec = value;
            ani.SetBool("Run", value.magnitude > 0.125f);
            if (value.x != 0) sr.flipX = value.x < 0 ? true : false;
        }
    }
    public int speed = 10;
    bool touchable = true;
    // Update is called once per frame
    public void OnDamage()
    {
        Debug.Log($"{gameObject.name} Is Dead.");
    }
    private void Start()
    {
        playerTransform = transform;
        rigid = GetComponent<Rigidbody2D>();
        sr = playerSprite.GetComponent<SpriteRenderer>();
        ani = playerSprite.GetComponent<Animator>();
    }
    void Update()
    {
        MoveVec = Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.up;
        rigid.velocity = MoveVec * speed;
        var mouseVec = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        if (Input.GetMouseButton(0) && touchable)
        {
            touchable = false;
            ani.SetTrigger("Pick");
            Invoke("Touchable", 0.5f);
            Debug.DrawRay(transform.position, mouseVec);
            var layhit = Physics2D.Raycast(transform.position, mouseVec, AttackRange);
            layhit.collider?.gameObject.GetComponent<Enemy>()?.OnDamage();
        }
    }
    void Touchable()
    {
        touchable = true;
    }
    IUsable obj;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.TryGetComponent<IUsable>(out obj);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<IUsable>(out obj))
        {
            obj = null;
        }
    }

}
