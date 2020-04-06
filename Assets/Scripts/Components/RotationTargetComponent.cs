using System;
using Unity.Entities;
using UnityEngine;

[Serializable]
[GenerateAuthoringComponent]
public struct RotationTargetComponent : IComponentData
{
    public float RotationSpeedDegrees;
    public Quaternion RotationTarget;
}
