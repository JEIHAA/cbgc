using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intertaction : MonoBehaviour
{
    private float timeElapsed = 0f;
    private float interval = 1f;

    private void Update()
    {
        InteractionProcess();
    }

    public void InteractionProcess()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CloseChacker.NearestObject?.GetComponentInParent<IInteractiveObject>().Interaction(99);
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
                CloseChacker.NearestObject?.GetComponentInParent<IInteractiveObject>().Interaction(timeElapsed);
                timeElapsed = 0f;
                /*if (animator.GetCurrentAnimatorStateInfo(0).IsName("character_axe_Clip"))
                {
                    interval -= 0.1f;
                }*/
            }
            /*
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("character_axe_Clip"))
            {
                Debug.Log("1");
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    Debug.Log("2");
                    obj?.GetComponentInParent<IInteractiveObject>().Interaction(timeElapsed);
                }
                timeElapsed = 0f;
            }
            else 
            {
                if(timeElapsed >= interval)
                {
                    obj?.GetComponentInParent<IInteractiveObject>().Interaction(timeElapsed);
                    timeElapsed = 0f;
                }
            }*/
        }
    }
}
