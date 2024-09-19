using UnityEngine;
public class EnemyController : Controller
{
    public bool followPlayer;
    public override void Move()
    {
        if(followPlayer) Velocity = (Player.playerTransform.position - transform.position).normalized * speed + addVelocity;
        else Velocity = -transform.position.normalized * speed + addVelocity; 
        if (Velocity.x != 0) gameObject.transform.localScale = new Vector3(Velocity.x > 0 ? 1 : -1, 1, 1);
    }
    public override void KnockBack()
    {
        if (followPlayer) KnockBack(Player.playerTransform.position);
        else KnockBack(Vector3.zero);
    }
}
