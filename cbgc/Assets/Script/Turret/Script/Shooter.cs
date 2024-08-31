using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    //총알 발사
    public GameObject bulletPrefeb;
    int nowBulletIdx = 0;
    List<GameObject> bulletPool;
    WaitForSeconds bulletLife;
    private void Start()
    {
        bulletPool = new List<GameObject>();
        bulletLife = new(0.5f);
        //총알 10개 추가
        for (int i = 0; i < 10; i++) bulletPool.Add(Instantiate(bulletPrefeb));
    }
    public void Shot(Vector2 _dir)
    {
        //현재 총알
        var nowBullet = bulletPool[nowBulletIdx];
        //총알 리지드바디
        var rigid = nowBullet.GetComponent<Rigidbody2D>();
        //총알 위치를 현재 위치로
        nowBullet.transform.position = transform.position;
        //속도 부여 전 활성화
        nowBullet.SetActive(true);
        //총알에 속도 설정
        rigid.velocity = _dir;
        //다음 총알로
        StartCoroutine(BulletOff(nowBullet));
        nowBulletIdx = (nowBulletIdx + 1) % bulletPool.Count;
    }
    IEnumerator BulletOff(GameObject _bullet)
    {
        yield return bulletLife;
        _bullet.SetActive(false);
    }
}
