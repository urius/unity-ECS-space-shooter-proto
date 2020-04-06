using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

public class EnemyAISystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var minClosingDistance = WorldBounds.Bounds.height / 4;
        var screenTopY = WorldBounds.Bounds.yMax;
        var query = GetEntityQuery(typeof(PlayerTag), typeof(Translation));
        var playerTranslations = query.ToComponentDataArray<Translation>(Allocator.TempJob);
        var playerPosition = (Vector3) playerTranslations[0].Value;
        playerTranslations.Dispose();

        var playerComponents = query.ToComponentDataArray<PlayerTag>(Allocator.TempJob);
        var playerFlySpeed = playerComponents[0].FlySpeed;
        playerComponents.Dispose();

        var jobHandle = Entities.ForEach((
            ref EnemyAIComponent ai,
            ref Translation translation,
            ref RotationTargetComponent rotationTarget,
            ref LocalToWorld localToWorld,
            ref WeaponsControlComponent weaponsControl) =>
        {
            var t = translation.Value;
            t.y -= playerFlySpeed;
            translation.Value = t;

            switch (ai.State)
            {
                case EnemyAIComponent.States.SimpleFly:
                    if(translation.Value.y < screenTopY)
                    {
                        ai.State = EnemyAIComponent.States.MovingIn;
                    }
                    break;
                case EnemyAIComponent.States.MovingIn:
                    if (translation.Value.y > playerPosition.y + minClosingDistance)
                    {
                        var direction = playerPosition - (Vector3)translation.Value;
                        rotationTarget.RotationTarget = Quaternion.LookRotation(direction, localToWorld.Up);
                    }
                    else
                    {
                        var directionOut = (Vector3)translation.Value - playerPosition;
                        directionOut.y = -Math.Abs(directionOut.y);
                        rotationTarget.RotationTarget = Quaternion.LookRotation(directionOut, localToWorld.Up);
                        ai.State = EnemyAIComponent.States.MovingOut;
                    }
                    break;
                case EnemyAIComponent.States.MovingOut:
                    break;
            }

            weaponsControl.IsFiring = false;
            if (translation.Value.y < screenTopY)
            {
                var lookDirection = playerPosition - (Vector3)translation.Value;
                var angle = Quaternion.Angle(Quaternion.LookRotation(lookDirection, localToWorld.Up), localToWorld.Rotation);
                if(Math.Abs(angle) < 25)
                {
                    //weaponsControl.IsFiring = true;
                }
            }
        })
        .Schedule(inputDeps);

        return jobHandle;
    }
}
