using UnityEngine;

public class Picking : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask layer;
    private Camera mainCam = null;

    private GameObject obj = null;
    public GameObject PickingObj => obj;

    private float timeElapsed = 0f;
    private float interval = 1f;


    private GameObject PickingProcess()
    {
        if (mainCam == null)
            mainCam = Camera.main;

        // 마우스 위치에 레이를 생성
        Vector2 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, layer);

        // 충돌한 오브젝트가 있다면 반환
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("InteractiveObject")) 
            {
                //Debug.Log(hit.collider.gameObject.name);
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
            obj?.GetComponentInParent<IInteractiveObject>().Interaction(99);
            //Debug.Log(obj.name);
        }

        if (Input.GetMouseButton(0))
        {
            timeElapsed += Time.deltaTime;
            //UnityEngine.Debug.Log(timeElapsed);

            if (timeElapsed >= interval)
            {
                /*if (animator.GetCurrentAnimatorStateInfo(0).IsName("character_axe_Clip"))
                {
                    interval -= 0.1f;
                }*/

                obj?.GetComponentInParent<IInteractiveObject>().Interaction(timeElapsed);
                timeElapsed = 0f;
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

        if (Input.GetMouseButtonUp(0))
        {
            obj = null;
            timeElapsed = 0f;
        }
    }

}


