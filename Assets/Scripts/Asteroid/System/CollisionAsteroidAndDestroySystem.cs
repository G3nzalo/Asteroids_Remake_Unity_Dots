using Unity.Entities;
using Unity.Jobs;
using Unity.Physics.Stateful;
using UnityEngine;
using Score.View;


namespace Asteroids.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(TriggerEventConversionSystem))]
    public class CollisionAsteroidAndDestroySystem : SystemBase
    {
        private EndFixedStepSimulationEntityCommandBufferSystem m_CommandBufferSystem;

        private TriggerEventConversionSystem m_TriggerSystem;
        private EntityQueryMask m_NonTriggerMask;

        protected override void OnCreate()
        {
            m_CommandBufferSystem = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();
            m_TriggerSystem = World.GetOrCreateSystem<TriggerEventConversionSystem>();
            m_NonTriggerMask = EntityManager.GetEntityQueryMask(
                GetEntityQuery(new EntityQueryDesc
                {
                    None = new ComponentType[]
                    {
                    typeof(StatefulTriggerEvent)
                    }
                })
            );
        }

        protected override void OnUpdate()
        {
            Dependency = JobHandle.CombineDependencies(m_TriggerSystem.OutDependency, Dependency);

            var commandBuffer = m_CommandBufferSystem.CreateCommandBuffer();
            var nonTriggerMask = m_NonTriggerMask;

            Entities
                .WithoutBurst()
                .ForEach((Entity e, ref DynamicBuffer<StatefulTriggerEvent> triggerEventBuffer) =>
                {
                    for (int i = 0; i < triggerEventBuffer.Length; i++)
                    {
                        var triggerEvent = triggerEventBuffer[i];
                        var otherEntity = triggerEvent.GetOtherEntity(e);

                        if (triggerEvent.State == EventOverlapState.Stay || !nonTriggerMask.Matches(otherEntity))
                        {
                            continue;
                        }

                        if (triggerEvent.State == EventOverlapState.Enter)
                        {
                            commandBuffer.AddComponent(otherEntity, new DestroyTagAsteroids { });
                            ScoreManagerView.instance.CurentScore += 1;
                            ScoreManagerView.instance.SetScoreText(ScoreManagerView.instance.CurentScore);
                            SoundManager.instance.PlaySfxDestroy(Random.Range(-3f, 4f));
                        }
                    }
                }).Run();

            m_CommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}