using UnityEngine;
public class PlayerController : Controller
{
    public bool lookMouse;
    public override void Move()
    {
        //�̵� ���� �Է¹ޱ�
        Velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * speed + addVelocity;
        //���콺�� �ٶ���� �Ѵٸ� ���콺 ���� (�ڿ� ĳ�µ���, �����ϴ� ����)
        if (lookMouse)
        {
            gameObject.transform.localScale = new Vector3((Player.playerTransform.position.x > Camera.main.ScreenToWorldPoint(Input.mousePosition).x ? -1 : 1), 1, 1);
        }
        //�ܼ� �̵�
        else if(Velocity.x != 0)
        {
            gameObject.transform.localScale = new Vector3(Velocity.x < 0 ? -1 : 1, 1, 1);
        }
    }
}