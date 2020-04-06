using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class WeaponCooldownSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref WeaponComponent weaponComponent) =>
        {
            if (weaponComponent.CooldownFrames > 0)
            {
                weaponComponent.CooldownFrames--;
            }
        });
    }
}
