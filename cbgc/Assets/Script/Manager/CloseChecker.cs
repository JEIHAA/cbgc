using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseChecker : MonoBehaviour
{
    [SerializeField] private GameObject nearestObject;
    public GameObject NearestObject => nearestObject;

    private SpriteChanger spriteChanger;

    private float distance;
    private float minDistance = float.MaxValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("InteractiveObject"))
        {
            GetNearestInteractiveObject(collision.gameObject);
            SetNearestObjectValues(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == nearestObject)
        {
            SetNearestObjectValues(false);
            nearestObject = null;
        }
    }

    private void GetNearestInteractiveObject(GameObject _go)
    {
        distance = Vector2.Distance(transform.position, _go.transform.position);
        if (distance < minDistance)
        {
            if(nearestObject != null) SetNearestObjectValues(false);
            nearestObject = _go;
        }
    }

    private void SetNearestObjectValues(bool _value)
    {
        spriteChanger = nearestObject.GetComponent<SpriteChanger>();
        if (spriteChanger == null) return;
        spriteChanger.IsNearest = _value;
    }

}
