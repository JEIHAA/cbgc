using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour, IDamagable
{
    [SerializeField]
    GameObject playerSprite, knockbackObject;
    Animator ani;
    [SerializeField]
    SpriteRenderer sr, campFireCompass, campFireSR;
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
            if (value.x != 0) playerSprite.transform.localScale = new Vector3(value.x < 0 ? -1 : 1, 1, 1);
        }
    }
    public int speed = 10;
    bool touchable = true;
    public void OnDamage() { GameOver(); }
    void GameOver()
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
        campFireCompass.transform.localPosition = -transform.position.normalized * 4f;
        campFireCompass.sprite = campFireSR.sprite;
        var mouseVec = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        if(Input.GetMouseButtonUp(0)) ani.SetBool("Axe", false);
        if (Input.GetMouseButton(0) && touchable)
        {
            touchable = false;
            if(!ani.GetBool("Axe")) ani.SetBool("Axe",true);
            Invoke("Touchable", 1f);
            StartCoroutine(Swing(mouseVec));
            Debug.DrawRay(transform.position, mouseVec);
            var layhit = Physics2D.Raycast(transform.position, mouseVec, AttackRange);
            layhit.collider?.gameObject.GetComponent<Enemy>()?.OnDamage();
        }
    }
    IEnumerator Swing(Vector2 mouseVec)
    {
        knockbackObject.SetActive(true);
        knockbackObject.transform.localPosition = mouseVec.normalized;
        knockbackObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(mouseVec.y, mouseVec.x) * Mathf.Rad2Deg);
        yield return new WaitForSeconds(0.125f);
        knockbackObject.SetActive(false);
    }
    void Touchable() { touchable = true; }
    IUsable obj;
    private void OnCollisionEnter2D(Collision2D collision) { collision.gameObject.TryGetComponent<IUsable>(out obj); }
    private void OnCollisionExit2D(Collision2D collision) { if (collision.gameObject.TryGetComponent<IUsable>(out obj)) obj = null; }
}
