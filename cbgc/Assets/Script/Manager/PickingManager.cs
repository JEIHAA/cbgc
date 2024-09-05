using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PickingManager : MonoBehaviour
{
    [SerializeField] private Picking picking;
    [SerializeField] private bool isClose = false;
    [SerializeField] private float bombRadius = 15;
    [SerializeField] private GameObject dark;


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
        if (Input.GetKeyDown(KeyCode.F))
        {
            ResourceData.LogAmount += 10;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Bomb();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LightOn();
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

    private void LightOn()
    {
        if (dark.activeSelf) dark.SetActive(false);
        else dark.SetActive(true);
    }
}
