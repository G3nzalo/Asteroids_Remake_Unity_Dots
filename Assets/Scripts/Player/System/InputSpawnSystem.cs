using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace Player.System
{
    public class InputSpawnSystem : SystemBase
    {
        private EntityQuery m_PlayerQuery;
        private BeginSimulationEntityCommandBufferSystem m_BeginSimECB;
        private Entity m_PlayerPrefab;
        private Entity m_BulletPrefab;
        private Entity m_Particles;
        private float m_PerSecond = 10f;
        private float m_NextTime = 0;

        protected override void OnCreate()
        {
            m_PlayerQuery = GetEntityQuery(ComponentType.ReadWrite<CharacterTag>());
            m_BeginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
            RequireSingletonForUpdate<GameSettings>();
        }

        protected override void OnUpdate()
        {
            if (m_PlayerPrefab == Entity.Null || m_BulletPrefab == Entity.Null)
            {
                m_PlayerPrefab = GetSingleton<PlayerAuthoringComponent>().Prefab;
                m_BulletPrefab = GetSingleton<BulletAuthoringComponent>().Prefab;
                m_Particles = GetSingleton<ParticlesAuthoringComponent>().Prefab;
                return;
            }

            byte shoot;
            shoot = 0;
            var playerCount = m_PlayerQuery.CalculateEntityCountWithoutFiltering();

            if (Input.GetKey("space"))
            {
                shoot = 1;
            }

            if (shoot == 1 && playerCount < 1)
            {
                var entity = EntityManager.Instantiate(m_PlayerPrefab);
                return;
            }

            var commandBuffer = m_BeginSimECB.CreateCommandBuffer().AsParallelWriter();
            var gameSettings = GetSingleton<GameSettings>();
            var bulletPrefab = m_BulletPrefab;
            var particlesPrefab = m_Particles;

            var canShoot = false;
            if (UnityEngine.Time.time >= m_NextTime)
            {
                canShoot = true;
                m_NextTime += (1 / m_PerSecond);
            }

            Entities
            .WithAll<CharacterTag>()
            .ForEach((Entity entity, int nativeThreadIndex, in Translation position, in Rotation rotation,
                    in PhysicsVelocity velocity, in BulletSpawnOffsetComponent bulletOffset) =>
            {
            if (shoot != 1 || !canShoot)
                {
                    return;
                }

            var bulletEntity = commandBuffer.Instantiate(nativeThreadIndex, bulletPrefab);
            var particlesEntity = commandBuffer.Instantiate(nativeThreadIndex, particlesPrefab);
            var newPosition = new Translation { Value = position.Value + math.mul(rotation.Value, bulletOffset.Value).xyz };

                commandBuffer.SetComponent(nativeThreadIndex, bulletEntity, newPosition);
                commandBuffer.SetComponent(nativeThreadIndex, particlesEntity, newPosition);

            var vel = new PhysicsVelocity { Linear = (gameSettings.bulletVelocity * math.mul(rotation.Value, new float3(0, 1, 0)).xyz) + velocity.Linear };

                commandBuffer.SetComponent(nativeThreadIndex, bulletEntity, vel);

            }).ScheduleParallel();

            m_BeginSimECB.AddJobHandleForProducer(Dependency);
        }
    }

}