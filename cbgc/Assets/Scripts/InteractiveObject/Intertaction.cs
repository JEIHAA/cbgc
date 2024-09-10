using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intertaction : MonoBehaviour
{
    [SerializeField] private CloseChecker closeChecker;
    private float timeElapsed = 0f;
    private float interval = 1f;

    private void Update()
    {
        InteractionProcess();
        Debug.Log(timeElapsed);
    }

    public void InteractionProcess()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (closeChecker.NearestObject == null) return;
            closeChecker?.NearestObject?.GetComponentInParent<IInteractiveObject>().Interaction(0);
        }

        if (Input.GetMouseButtonUp(0))
        {
            timeElapsed = 0f;
        }

        if (Input.GetMouseButton(0))
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed >= interval)
            {
                if (closeChecker.NearestObject == null) return;
                closeChecker?.NearestObject?.GetComponentInParent<IInteractiveObject>().Interaction(timeElapsed);
                timeElapsed = 0f;
            }
        }
    }
}
