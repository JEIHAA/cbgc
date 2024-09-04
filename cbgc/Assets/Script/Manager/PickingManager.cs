using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PickingManager : MonoBehaviour
{
    [SerializeField] private Picking picking;
    [SerializeField] private bool isClose = false;
    [SerializeField] private float bombRadius = 15;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("InteractiveObject"))
        {
            //Debug.Log(collision.name);
            isClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("InteractiveObject"))
        {
            //Debug.Log(collision.name);
            isClose = false;
        }
    }

    private void Update()
    {
        if (isClose)
        {
            picking.PickingAction();
        }
        if (Input.GetKey(KeyCode.F))
        {
            ResourceData.LogAmount += 1;
        }
        if (Input.GetKey(KeyCode.G))
        {
            Bomb();
        }
    }

    private void Bomb()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bombRadius);
        foreach (Collider2D coll in colliders)
        { 
            if(coll.gameObject.CompareTag("Monster"))
            {
                coll.gameObject.GetComponent<Enemy>().OnDamage(9999);
            }
        }
    }
}
