using Unity.Entities;
using Bullet.Component;

namespace Bullet.System
{
    public class BulletAgeSystem : SystemBase
    {
        private BeginSimulationEntityCommandBufferSystem m_BeginSimEcb;

        protected override void OnCreate()
        {
            m_BeginSimEcb = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = m_BeginSimEcb.CreateCommandBuffer().AsParallelWriter();

            var deltaTime = Time.DeltaTime;
            Entities.ForEach((Entity entity, int nativeThreadIndex, ref BulletAgeComponent age) =>
            {
                age.age += deltaTime;
                if (age.age > age.maxAge)
                    commandBuffer.DestroyEntity(nativeThreadIndex, entity);

            }).ScheduleParallel();
            m_BeginSimEcb.AddJobHandleForProducer(Dependency);
        }
    }
}