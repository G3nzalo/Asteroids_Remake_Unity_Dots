using Unity.Entities;
using Unity.Jobs;

namespace Enemy.System
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public class EnemyDestructionSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem m_EndSimEcb;

        protected override void OnCreate()
        {
            m_EndSimEcb = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = m_EndSimEcb.CreateCommandBuffer().AsParallelWriter();
            Entities
            .WithAll<DestroyEnemyTag, EnemyTag>()
            .ForEach((Entity entity, int nativeThreadIndex) =>
            {
                commandBuffer.DestroyEntity(nativeThreadIndex, entity);

            }).ScheduleParallel();
            m_EndSimEcb.AddJobHandleForProducer(Dependency);

        }
    }
}