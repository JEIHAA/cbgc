using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonefire : MonoBehaviour, IUsable
{
    Animator ani;
    WaitForSeconds delay;
    bool canUse = true;
    float fireLevel = 10f, decreaseDelay = 1f;
    public float decreaseAmount = 1f;
    public float DecreaseDelay { set { decreaseDelay = value; delay = new WaitForSeconds(value); } }
    public float FireLevel
    {
        get { return fireLevel; }
        set {
                fireLevel = value;
                ani.SetFloat("FireLevel", value);
                UIManager.instance.UpdateCampFireLeftTimetUI((int)(value / decreaseAmount * decreaseDelay));
            }
    }
        
    // Start is called before the first frame update
    void Start()
    {
        fireLevel = 10f;
        ani = GetComponent<Animator>();
        DecreaseDelay = 1f;
        StartCoroutine(FireLevelDown());
    }
    public void ChangeDelay(float _newDelay)
    {
        DecreaseDelay = _newDelay;
    }
    IEnumerator FireLevelDown()
    {
        while (true)
        {
            yield return delay;
            FireLevel -= decreaseAmount;
            if(FireLevel <= 0)
            {
                Debug.Log("BoneFire is Dead.");
                break;
            }
        }        
    }
    public IEnumerator ReUseTime(float _t)
    {
        yield return new WaitForSeconds(_t);
        canUse = true;
    }
    public void Use()
    {
        if (canUse)
        {
            canUse = false;
            StartCoroutine(ReUseTime(1f));
            FireLevel = 4;
        }

    }
}
