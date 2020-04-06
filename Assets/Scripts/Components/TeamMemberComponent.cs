using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct TeamMemberComponent : IComponentData
{
    public static int PlayerTeam = 0;
    public static int EnemyTeam = 1;

    public int TeamId;
}
