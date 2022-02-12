using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

namespace Asteroids.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateBefore(typeof(EndFixedStepSimulationEntityCommandBufferSystem))]
    public class AsteroidsOutOfBoundsSystem : SystemBase
    {
        private EndFixedStepSimulationEntityCommandBufferSystem m_EndFixedStepSimECB;
        protected override void OnCreate()
        {
            m_EndFixedStepSimECB = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();
            RequireSingletonForUpdate<GameSettings>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = m_EndFixedStepSimECB.CreateCommandBuffer().AsParallelWriter();
            var settings = GetSingleton<GameSettings>();
            Entities
            .WithAll<AsteroidTag>()
            .ForEach((Entity entity, int nativeThreadIndex, in Translation position) =>
            {
                if (Mathf.Abs(position.Value.x) > settings.levelWidth / 2 ||
                    Mathf.Abs(position.Value.y) > settings.levelHeight / 2 ||
                    Mathf.Abs(position.Value.z) > 0)
                {
                    commandBuffer.AddComponent(nativeThreadIndex, entity, new DestroyTagAsteroids());
                    return;
                }
            }).ScheduleParallel();
            m_EndFixedStepSimECB.AddJobHandleForProducer(Dependency);
        }
    }
}