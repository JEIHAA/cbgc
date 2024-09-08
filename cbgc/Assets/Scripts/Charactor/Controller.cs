using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Rigidbody2D rigid;
    [SerializeField, Range(1,15)]
    private int speed;
    public bool CanMove
    {
        get => rigid.bodyType != RigidbodyType2D.Static;
        set => rigid.bodyType = value ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
    }
    [SerializeField, Range(1,15)]
    WaitForSeconds knockBackTime;
    private Vector3 addVelocity;
    private Vector3 Velocity
    {
        get => rigid.velocity;
        set {
            if (CanMove)
            {
                rigid.velocity = value + addVelocity;
                if (value.x != 0) gameObject.transform.localScale = new Vector3(value.x > 0 ? 1 : -1, 1, 1);
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
    public void MoveInput() => Velocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized * speed + addVelocity;
    public void MoveToCenter() => Velocity = -transform.position.normalized * speed + addVelocity;
    public void MoveToPlayer() => Velocity = (Player.playerTransform.position - transform.position).normalized * speed + addVelocity;
    public void KnockBack(bool isPlayer)
    {
        //add speed for knockback
        if (isPlayer) addVelocity = -(Player.playerTransform.position - transform.position).normalized * 10;
        else addVelocity = -Velocity * 10;
        StartCoroutine(ResetAddVelocity(isPlayer));
    }
    IEnumerator ResetAddVelocity(bool isPlayer = false)
    {
        yield return knockBackTime;
        addVelocity = Vector3.zero;
    }
}
