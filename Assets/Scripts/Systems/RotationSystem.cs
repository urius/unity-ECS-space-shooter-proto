using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

public class RotationSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = Entities.ForEach((ref Rotation rotation, ref RotationTargetComponent rotationTarget) =>
        {
            rotation.Value = Quaternion.RotateTowards(rotation.Value, rotationTarget.RotationTarget, rotationTarget.RotationSpeedDegrees);
        })
        .Schedule(inputDeps);

        return jobHandle;
    }
}
