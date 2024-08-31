using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Picking : MonoBehaviour
{
    private Camera mainCam = null;

    private GameObject obj = null;
    public GameObject PickingObj => obj;

    private float timeElapsed = 0f;
    public float interval = 0.3f;


    private GameObject PickingProcess()
    {
        if (mainCam == null)
            mainCam = Camera.main;

        // 마우스 위치에 레이를 생성
        Vector2 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        // 충돌한 오브젝트가 있다면 반환
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("InteractiveObject")) 
            {
                return hit.collider.gameObject;
            }
        }

        return null;
    }

    public void PickingAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            obj = PickingProcess();
            if (obj == null) return;
            obj?.GetComponent<InteractiveObject>().Interaction(timeElapsed);
        }

        if (Input.GetMouseButton(0))
        {
            timeElapsed += Time.deltaTime;
            //UnityEngine.Debug.Log(timeElapsed);

            if (timeElapsed >= interval)
            {
                obj?.GetComponent<InteractiveObject>().Interaction(timeElapsed);
                timeElapsed = 0f;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            obj = null;
            timeElapsed = 0f;
        }
    }

}


