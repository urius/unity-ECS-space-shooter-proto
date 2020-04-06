using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct FollowMouseComponent : IComponentData
{
    public float DeltaMultiplier;
    public float OffsetY;
    public float LookPointYDistanceInScreens;
}
