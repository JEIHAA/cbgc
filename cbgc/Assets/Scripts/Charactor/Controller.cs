using System.Collections;
using UnityEngine;
public class Controller : MonoBehaviour
{
    private Rigidbody2D rigid;
    [SerializeField, Range(1, 15)]
    private int speed;
    public bool CanMove
    {
        get => rigid.bodyType != RigidbodyType2D.Kinematic;
        set {
            if (value) rigid.bodyType = RigidbodyType2D.Dynamic;
            else
            {
                rigid.velocity = Vector3.zero;
                rigid.bodyType = RigidbodyType2D.Kinematic;
            }
        }
    }
    [SerializeField, Range(1, 15)]
    WaitForSeconds knockBackTime;
    private Vector3 addVelocity;
    private Vector3 Velocity
    {
        get => rigid.velocity;
        set {
            if (CanMove)
            {
                rigid.velocity = value + addVelocity;
            }
        }
    }
    void Start()
    {
        addVelocity = Vector2.zero;
        rigid = GetComponent<Rigidbody2D>();
        knockBackTime = new(0.125f);
        CanMove = true;
    }
    public void MoveInput()
    {
        Velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * speed + addVelocity;
        gameObject.transform.localScale = new Vector3((Player.playerTransform.position.x > Camera.main.ScreenToWorldPoint(Input.mousePosition).x ? -1 : 1), 1, 1);
    }
    public void MoveToCenter()
    {
        Velocity = -transform.position.normalized* speed + addVelocity;
        if (Velocity.x != 0) gameObject.transform.localScale = new Vector3(Velocity.x > 0 ? 1 : -1, 1, 1);
    }
    public void MoveToPlayer()
    {
        Velocity = (Player.playerTransform.position - transform.position).normalized * speed + addVelocity;
        if (Velocity.x != 0) gameObject.transform.localScale = new Vector3(Velocity.x > 0 ? 1 : -1, 1, 1);
    }
    public void KnockBack(bool _isPlayer = false) => KnockBack(_isPlayer ? Player.playerTransform.position : Vector3.zero);    
    public void KnockBack(Vector3 origin)
    {
        //add speed for knockback
        addVelocity = -(origin - transform.position).normalized * 10;
        StartCoroutine(ResetAddVelocity());
    }
    IEnumerator ResetAddVelocity()
    {
        yield return knockBackTime;
        addVelocity = Vector3.zero;
    }
}
