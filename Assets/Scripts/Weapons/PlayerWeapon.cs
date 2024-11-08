using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : WeaponBase
{
    public override void Shoot()
    {
        AudioManager.Instance.PlaySFX(SFXType.Shoot);

        Projectile projectile = _projectilePool.Get();
        projectile.Initialize(shootSpeed);

        cooldownTimer = cooldown;
    }

    public override bool CanShoot()
    {
        return cooldownTimer <= 0;
    }
}
