using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class SpawnEnemiesSystem : ComponentSystem
{
    private Rect _worldBoundsRect;

    protected override void OnStartRunning()
    {
        _worldBoundsRect = WorldBounds.Bounds;
    }

    protected override void OnUpdate()
    {
        Entities.ForEach((ref SpawnEntityComponent spawn) =>
        {
            if (spawn.Cooldown <= 0)
            {
                spawn.Cooldown = spawn.DelayBetweenSpawns;

                var entity = EntityManager.Instantiate(spawn.Entity);

                var screenTop = _worldBoundsRect.y + _worldBoundsRect.height;
                var spawnPoint = new Vector3(
                    UnityEngine.Random.Range(_worldBoundsRect.x, _worldBoundsRect.x + _worldBoundsRect.width),
                    screenTop + UnityEngine.Random.Range(_worldBoundsRect.y, screenTop),
                    EntityManager.GetComponentData<Translation>(entity).Value.z);
                EntityManager.SetComponentData(entity, new Translation { Value = spawnPoint });

                var localToWorld = EntityManager.GetComponentData<LocalToWorld>(entity);
                var startRotation = Quaternion.LookRotation(-localToWorld.Forward, localToWorld.Up);
                PostUpdateCommands.SetComponent(entity, new Rotation { Value = startRotation });

                PostUpdateCommands.AddComponent(entity, new EnemyAIComponent());

                PostUpdateCommands.AddComponent(entity, new RotationTargetComponent() { RotationSpeedDegrees = 4 });

                PostUpdateCommands.AddComponent(entity, new WeaponsControlComponent());

                PostUpdateCommands.AddComponent(entity, new TeamMemberComponent { TeamId = TeamMemberComponent.EnemyTeam });

                PostUpdateCommands.AddComponent(entity, new HPComponent() { HP = 10 });

                PostUpdateCommands.AddComponent(entity, new MoveSpeedComponent { MoveSpeed = 0.8f });
            }
            else
            {
                spawn.Cooldown--;
            }
        });
    }
}
