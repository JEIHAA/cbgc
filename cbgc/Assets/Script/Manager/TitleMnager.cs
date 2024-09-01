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
    // Start is called before the first frame update
    private void Start()
    {
        Invoke("AnimationShowEnd", 3f);
    }
    void AnimationShowEnd()
    {
        animationShowEnd = true;
    }
    IEnumerator BlockMouseTouch()
    {
        canTouch = false;
        yield return new WaitForSeconds(0.5f);
        canTouch = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (canTouch && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(BlockMouseTouch());
            if(animationShowEnd) smm.LoadScene();
            if (ani.gameObject.activeSelf == false) ani.gameObject.SetActive(true);
            else ani.SetTrigger("Click");
        }
    }
}
