using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour, IDamagable
{
    
    Rigidbody2D rigid;
    Vector2 moveVec = Vector2.zero;
    Vector2 MoveVec
    {
        get { return moveVec; }
        set
        {
            if (value.magnitude > 1) moveVec = value.normalized;
            else moveVec = value;
        }
    }
    public int speed = 10;
    // Update is called once per frame
    public void OnDamage()
    {
        Debug.Log($"{gameObject.name} On Damage");
    }
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        MoveVec = Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.up;
        rigid.velocity = MoveVec * speed;
        if(Input.GetMouseButton(0) && obj != null)
        {
            obj.Use();
        }
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
