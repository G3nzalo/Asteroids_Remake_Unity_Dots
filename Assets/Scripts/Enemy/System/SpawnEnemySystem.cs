using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Enemy.Bullet.Component;

namespace Enemy.System
{
    public class SpawnEnemySystem : SystemBase
    {
        private EntityQuery m_EnemyQuery;
        private BeginSimulationEntityCommandBufferSystem m_BeginSimECB;
        private Entity m_EnemyPrefab;
        private Entity m_BulletEnemyPrefab;
        private float m_PerSecond = UnityEngine.Random.Range(5, 10);
        private float m_NextTime = 0;

        protected override void OnCreate()
        {
            m_EnemyQuery = GetEntityQuery(ComponentType.ReadWrite<EnemyTag>());
            m_BeginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
            RequireSingletonForUpdate<GameSettings>();
        }

        protected override void OnUpdate()
        {
            if (m_EnemyPrefab == Entity.Null || m_EnemyPrefab == Entity.Null)
            {
                m_EnemyPrefab = GetSingleton<EnemyAuthoringComponent>().Prefab;
                m_BulletEnemyPrefab = GetSingleton<BulletEnemyAuthoringComponent>().Prefab;
                return;
            }

            byte shoot;
            shoot = 1;
            var enemyCount = m_EnemyQuery.CalculateEntityCountWithoutFiltering();

            if (shoot == 1 && enemyCount < 1)
            {
                return;
            }

            var commandBuffer = m_BeginSimECB.CreateCommandBuffer().AsParallelWriter();
            var gameSettings = GetSingleton<GameSettings>();
            var bulletPrefab = m_BulletEnemyPrefab;

            var canShoot = false;
            if (UnityEngine.Time.time >= m_NextTime)
            {
                canShoot = true;
                m_NextTime += (1 / m_PerSecond);
            }

            Entities
            .WithAll<EnemyTag>()
            .ForEach((Entity entity, int nativeThreadIndex, in Translation position, in Rotation rotation,
                    in PhysicsVelocity velocity, in BulletEnemySpawnOffsetComponent bulletOffset) =>
            {
            if (shoot != 1 || !canShoot)
                {
                    return;
                }

            var bulletEntity = commandBuffer.Instantiate(nativeThreadIndex, bulletPrefab);

            var newPosition = new Translation { Value = position.Value + math.mul(rotation.Value, bulletOffset.Value).xyz };
                commandBuffer.SetComponent(nativeThreadIndex, bulletEntity, newPosition);

            var vel = new PhysicsVelocity { Linear = (gameSettings.bulletVelocity * math.mul(rotation.Value, new float3(0, 1, 0)).xyz) + velocity.Linear };

                commandBuffer.SetComponent(nativeThreadIndex, bulletEntity, vel);

            }).ScheduleParallel();

            m_BeginSimECB.AddJobHandleForProducer(Dependency);
        }
    }

}