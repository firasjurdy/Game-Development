using UnityEngine;

public class HitCollider : MonoBehaviour
{
    public delegate void ColliderHit(float damageValue);
    public event ColliderHit E_ColliderHit;
    
    private void OnTriggerEnter2D(Collider2D thisHitMe)
    {
        Projectile projectile = thisHitMe.gameObject.GetComponent<Projectile>();
        if (projectile != null)
        {
            E_ColliderHit?.Invoke(projectile.Damage);
            projectile.Hit();
        }
    }
}
