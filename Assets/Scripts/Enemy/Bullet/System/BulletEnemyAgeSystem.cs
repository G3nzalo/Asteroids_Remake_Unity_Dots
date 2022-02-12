using Unity.Entities;
using Enemy.Bullet.Component;

namespace Enemy.Bullet.System
{
    public class BulletEnemyAgeSystem : SystemBase
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

            Entities.ForEach((Entity entity, int nativeThreadIndex, ref BuelletEnemyAgeComponent age) =>
            {
                age.age += deltaTime;
                if (age.age > age.maxAge)
                    commandBuffer.DestroyEntity(nativeThreadIndex, entity);


            }).ScheduleParallel();
            m_BeginSimEcb.AddJobHandleForProducer(Dependency);
        }
    }
}