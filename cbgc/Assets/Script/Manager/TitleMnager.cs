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
    bool doubleClick = false;
    int count = 0;
    IEnumerator CheckDoubleClick()
    {
        doubleClick = true;
        yield return new WaitForSeconds(0.1f);
        doubleClick = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (doubleClick || ani.GetCurrentAnimatorClipInfo(0)[0].clip.name == "title_3_Clip") smm.LoadScene();
            ++count;
            ani.SetTrigger("Click");
            StartCoroutine(CheckDoubleClick());
        }
    }
}
