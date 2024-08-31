using System.Diagnostics;
using UnityEngine;

public class Picking : MonoBehaviour
{
    private Camera mainCam = null;

    private GameObject obj = null;
    public GameObject PickingObj => obj;

    private Stopwatch stopwatch;
    private float time;
    private bool functionExecuted;

    private void Start()
    {
        stopwatch = new Stopwatch();
    }

    private GameObject PickingProcess()
    {
        if (mainCam == null)
            mainCam = Camera.main;

        // ���콺 ��ġ�� ���̸� ����
        Vector2 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        // �浹�� ������Ʈ�� �ִٸ� ��ȯ
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("InteractiveObject")) 
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
            UnityEngine.Debug.Log(obj.name);
            time = 0;
            functionExecuted = false;
            obj?.GetComponent<InteractiveObject>().Interaction(time);
            stopwatch.Restart();
        }

        if (Input.GetMouseButtonUp(0))
        {
            stopwatch.Stop();
            return;
        }

        // ���콺�� ������ �ִ� ���� ��� �ð��� Ȯ��
        if (Input.GetMouseButton(0))
        {
            if (!functionExecuted && stopwatch.Elapsed.TotalSeconds >= 1.0)
            {
                time = (float) stopwatch.Elapsed.TotalSeconds;
                UnityEngine.Debug.Log(time);
                obj?.GetComponent<InteractiveObject>().Interaction(time);
                functionExecuted = true;
                stopwatch.Stop();
            }
        }
    }

    
}
