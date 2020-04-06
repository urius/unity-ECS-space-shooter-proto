using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class MoveSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = Entities
            .ForEach((Entity entity, ref Translation t, ref LocalToWorld localToWorld, ref MoveSpeedComponent moveSpeed) =>
            {
                var pos = t.Value;
                t.Value = pos + (float3)(Vector3.Normalize(localToWorld.Forward) * moveSpeed.MoveSpeed);
            })
            .Schedule(inputDeps);

        return jobHandle;
    }
}
