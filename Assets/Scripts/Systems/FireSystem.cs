using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class FireSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref Parent parent, ref LocalToWorld localToWorld, ref WeaponComponent weaponComponent) =>
        {
            if (EntityManager.GetComponentData<WeaponsControlComponent>(parent.Value).IsFiring && weaponComponent.IsReadyToFire)
            {
                weaponComponent.CooldownFrames = weaponComponent.ReloadFrames;

                var pos = new Vector3(localToWorld.Value.c3.x, localToWorld.Value.c3.y, localToWorld.Value.c3.z);
                var bullet = EntityManager.Instantiate(weaponComponent.Prefab);
                EntityManager.SetComponentData(bullet, new Translation() { Value = pos });
                EntityManager.SetComponentData(bullet, new Rotation() { Value = Quaternion.LookRotation(localToWorld.Forward, localToWorld.Up) });
                var teamId = EntityManager.GetComponentData<TeamMemberComponent>(parent.Value).TeamId;
                EntityManager.SetComponentData(bullet, new TeamMemberComponent() { TeamId = teamId });
            }
        });
    }
}
