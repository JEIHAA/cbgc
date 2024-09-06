using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerContorller : MonoBehaviour
{
    private Rigidbody2D rigid;
    [SerializeField]
    private float speed;
    public bool canMove;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        canMove = true;
    }
    void Update() =>
        rigid.velocity = (new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))).normalized *
         (canMove ? speed : 0);
}
