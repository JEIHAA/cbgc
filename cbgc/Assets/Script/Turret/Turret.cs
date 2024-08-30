using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Turret : MonoBehaviour, IDamagable
{
    //�� ����, ������ �߻�
    WaitForSeconds shootDelayTime;
    public float shootSpeed, shootDelay;
    public Shooter shooter;
    public List<Enemy> enemyList;
    bool isUntouchable = false;
    public float ShootDelay
    {
        get { return shootDelay; }
        set { shootDelay = value; shootDelayTime = new WaitForSeconds(value); }
    }
    private void Start()
    {
        shootDelayTime = new(shootDelay);
        //�Ѿ� 10�� �߰�
        StartCoroutine(Shot());
    }
    public void OnDamage()
    {
            Debug.Log($"{gameObject.name} On Damage");   
    }
    IEnumerator Shot()
    {
        while (true)
        {
            yield return shootDelayTime;
            if (enemyList.Count > 0 && enemyList.First() != null) shooter.Shot((enemyList.First().transform.position - transform.position).normalized * shootSpeed);
        }
    }
} 