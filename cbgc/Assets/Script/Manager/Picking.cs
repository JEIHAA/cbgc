using UnityEngine;

public class Picking : MonoBehaviour
{
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

        // ���콺 ��ġ�� ���̸� ����
        Vector2 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, layer);

        // �浹�� ������Ʈ�� �ִٸ� ��ȯ
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
                obj?.GetComponentInParent<IInteractiveObject>().Interaction(timeElapsed);
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


