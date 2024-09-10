using System.Collections;
using UnityEngine;
public class Controller : MonoBehaviour
{
    private Rigidbody2D rigid;
    [SerializeField, Range(1, 15)]
    protected int speed;
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
    protected Vector3 addVelocity;
    protected Vector3 Velocity
    {
        get => rigid.velocity;
        set { if (CanMove) rigid.velocity = value + addVelocity; }
    }
    void Start()
    {
        addVelocity = Vector2.zero;
        rigid = GetComponent<Rigidbody2D>();
        knockBackTime = new(0.125f);
        CanMove = true;
    }
    public virtual void Move() { }
    public virtual void KnockBack() { }
    public void KnockBack(Vector3 origin)
    {
        //add speed for knockback
        addVelocity = -(origin - transform.position).normalized * 10;
        StartCoroutine(ResetAddVelocity());
    }
    protected IEnumerator ResetAddVelocity()
    {
        yield return knockBackTime;
        addVelocity = Vector3.zero;
    }
}