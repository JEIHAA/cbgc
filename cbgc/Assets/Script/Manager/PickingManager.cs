using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickingManager : MonoBehaviour
{
    [SerializeField] private Picking picking;

    void Update()
    {
        picking.PickingAction();
    }
}
