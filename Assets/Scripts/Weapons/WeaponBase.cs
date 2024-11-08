using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] protected float shootSpeed;
    [SerializeField] protected Projectile bulletPrefab;
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;
    protected Transform shootPoint;
    
    protected ObjectPool<Projectile> _projectilePool;
    protected virtual void Start()
    {
        _projectilePool =
            new ObjectPool<Projectile>(CreateProjectile, GetProjectile, ReleaseProjecitle, DestroyProjectile);
    }

    #region Projectile Pool
    protected virtual void DestroyProjectile(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }

    protected virtual void ReleaseProjecitle(Projectile projectile)
    {
        projectile.Reset();
        projectile.gameObject.SetActive(false);
    }

    protected virtual void GetProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
        projectile.transform.SetPositionAndRotation(shootPoint.position, transform.rotation);

    }

    protected virtual Projectile CreateProjectile()
    {
        Projectile projectile = Instantiate(bulletPrefab, shootPoint.position, transform.rotation);
        projectile.SetPool(_projectilePool);
        return projectile;
    }
    
    #endregion

    protected virtual void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public abstract void Shoot();

    public abstract bool CanShoot();

    public virtual void SetShootPoint(Transform point)
    {
        shootPoint = point;
    }
}
