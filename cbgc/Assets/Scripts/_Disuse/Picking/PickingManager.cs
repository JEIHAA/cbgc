using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PickingManager : MonoBehaviour
{
    [SerializeField] private Picking picking;
    [SerializeField] private bool isClose = false;

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
        
    }
}
