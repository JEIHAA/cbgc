using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour, IDamagable
{
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
    void Update()
    {
        MoveVec = Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.up;

        transform.position += (Vector3)MoveVec * Time.deltaTime * speed;
    }
}
