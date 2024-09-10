using UnityEngine;
public class PlayerController : Controller
{
    public bool lookMouse;
    public override void Move()
    {
        //이동 방향 입력받기
        Velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * speed + addVelocity;
        //마우스를 바라봐야 한다면 마우스 보기 (자원 캐는동안, 공격하는 동안)
        if (lookMouse)
        {
            gameObject.transform.localScale = new Vector3((Player.playerTransform.position.x > Camera.main.ScreenToWorldPoint(Input.mousePosition).x ? -1 : 1), 1, 1);
        }
        //단순 이동
        else if(Velocity.x != 0)
        {
            gameObject.transform.localScale = new Vector3(Velocity.x < 0 ? -1 : 1, 1, 1);
        }
    }
}