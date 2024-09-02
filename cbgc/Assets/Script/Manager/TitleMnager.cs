using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMnager : MonoBehaviour
{
    [SerializeField]
    Animator ani;
    [SerializeField]
    SceneMoveManager smm;
    bool animationShowEnd = false;
    bool canTouch = true;
    int count = 0;
    IEnumerator BlockMouseTouch()
    {
        canTouch = false;
        yield return new WaitForSeconds(0.25f);
        canTouch = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (canTouch && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(BlockMouseTouch());
            if (ani.GetCurrentAnimatorClipInfo(0)[0].clip.name == "title_3_Clip") smm.LoadScene();
            ++count;
            ani.SetTrigger("Click");
        }
    }
}
