using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    //�Ѿ� �߻�
    public GameObject bulletPrefeb;
    int nowBulletIdx = 0;
    List<GameObject> bulletPool;
    WaitForSeconds bulletLife;
    private void Start()
    {
        bulletPool = new List<GameObject>();
        bulletLife = new(0.5f);
        //�Ѿ� 10�� �߰�
        for (int i = 0; i < 10; i++) bulletPool.Add(Instantiate(bulletPrefeb));
    }
    public void Shot(Vector2 _dir)
    {
        //���� �Ѿ�
        var nowBullet = bulletPool[nowBulletIdx];
        //�Ѿ� ������ٵ�
        var rigid = nowBullet.GetComponent<Rigidbody2D>();
        //�Ѿ� ��ġ�� ���� ��ġ��
        nowBullet.transform.position = transform.position;
        //�ӵ� �ο� �� Ȱ��ȭ
        nowBullet.SetActive(true);
        //�Ѿ˿� �ӵ� ����
        rigid.velocity = _dir;
        //���� �Ѿ˷�
        StartCoroutine(BulletOff(nowBullet));
        nowBulletIdx = (nowBulletIdx + 1) % bulletPool.Count;
    }
    IEnumerator BulletOff(GameObject _bullet)
    {
        yield return bulletLife;
        _bullet.SetActive(false);
    }
}
