using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class FollowMouseSystem : ComponentSystem
{
    private Rect _worldBoundsRect = new Rect();

    protected override void OnStartRunning()
    {
        _worldBoundsRect = WorldBounds.Bounds;
    }

    protected override void OnUpdate()
    {
        var mousePosNorm = new Vector3(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
        Entities.ForEach((ref FollowMouseComponent f, ref Translation translation, ref RotationTargetComponent rotation) =>
        {
            var targetX = _worldBoundsRect.x + mousePosNorm.x * _worldBoundsRect.width;
            var targetY = _worldBoundsRect.y + mousePosNorm.y * _worldBoundsRect.height;
            var newX = translation.Value.x + (targetX - translation.Value.x) * f.DeltaMultiplier;
            var newY = translation.Value.y + (targetY - translation.Value.y) * f.DeltaMultiplier + f.OffsetY;
            translation.Value = new Vector3(newX, newY, 0);

            var lookAtY = targetY - translation.Value.y + _worldBoundsRect.height * f.LookPointYDistanceInScreens;
            rotation.RotationTarget = Quaternion.LookRotation(new Vector3(targetX - translation.Value.x, lookAtY, 0), new Vector3(0, 0, -1));
        });
    }
}
