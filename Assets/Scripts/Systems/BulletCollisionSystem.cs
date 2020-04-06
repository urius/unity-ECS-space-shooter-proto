using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

public class BulletCollisionSystem : JobComponentSystem
{
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;

    private static int collisions = 0;

    protected override void OnCreate()
    {
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    private struct CollisionJob : ITriggerEventsJob
    {
        internal ComponentDataFromEntity<DamageApplyer> damageApplyerGroup;
        internal ComponentDataFromEntity<HPComponent> hpGroup;
        internal ComponentDataFromEntity<TeamMemberComponent> teamMemberGroup;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity bullet;
            Entity target;
            if (damageApplyerGroup.HasComponent(triggerEvent.Entities.EntityA) && hpGroup.HasComponent(triggerEvent.Entities.EntityB))
            {
                bullet = triggerEvent.Entities.EntityA;
                target = triggerEvent.Entities.EntityB;
            } else if (damageApplyerGroup.HasComponent(triggerEvent.Entities.EntityB) && hpGroup.HasComponent(triggerEvent.Entities.EntityA))
            {
                bullet = triggerEvent.Entities.EntityB;
                target = triggerEvent.Entities.EntityA;
            } else
            {
                return;
            }

            if (teamMemberGroup[bullet].TeamId != teamMemberGroup[target].TeamId)
            {
                Debug.Log("Hit! " + teamMemberGroup[bullet].TeamId + "  " + teamMemberGroup[target].TeamId);

                var hp = hpGroup[target];
                hp.HP -= damageApplyerGroup[bullet].damage;
                hpGroup[target] = hp;
                if (hpGroup[target].HP <= 0)
                {
                    Debug.Log("HP<0!");
                }
            }        

            collisions++;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var collisionJob = new CollisionJob()
        {
            damageApplyerGroup = GetComponentDataFromEntity<DamageApplyer>(),
            hpGroup = GetComponentDataFromEntity<HPComponent>(),
            teamMemberGroup = GetComponentDataFromEntity<TeamMemberComponent>(),
        };

        var jobHandle = collisionJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
        jobHandle.Complete();

        return jobHandle;
    }
}
