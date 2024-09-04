using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickingManager : MonoBehaviour
{
    [SerializeField] private Picking picking;
    private Collider2D coll;
    [SerializeField] private bool isClose = false;

    private void Start()
    {
        coll = GetComponent<Collider2D>();
    }

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

    void Update()
    {

        if (isClose)
        {
            picking.PickingAction();
        }
    }
}
