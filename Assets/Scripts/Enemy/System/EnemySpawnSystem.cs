using System.Diagnostics;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Burst;
using Unity.Physics;

namespace Enemy.System
{
    public class EnemySpawnSystem : SystemBase
    {
        private EntityQuery m_EnemyQuery;
        private BeginSimulationEntityCommandBufferSystem m_BeginSimECB;
        private EntityQuery m_GameSettingsQuery;
        private Entity m_Prefab;

        protected override void OnCreate()
        {
            m_EnemyQuery = GetEntityQuery(ComponentType.ReadWrite<EnemyTag>());
            m_BeginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
            m_GameSettingsQuery = GetEntityQuery(ComponentType.ReadWrite<GameSettings>());
            RequireForUpdate(m_GameSettingsQuery);
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            if (m_Prefab == Entity.Null)
            {
                m_Prefab = GetSingleton<EnemyAuthoringComponent>().Prefab;
                return;
            }
            var settings = GetSingleton<GameSettings>();
            var commandBuffer = m_BeginSimECB.CreateCommandBuffer();
            var count = m_EnemyQuery.CalculateEntityCountWithoutFiltering();
            var enemyPrefab = m_Prefab;
            var rand = new Unity.Mathematics.Random((uint)Stopwatch.GetTimestamp() * 2);

            Job
            .WithCode(() =>
            {
                for (int i = count; i < settings.numEnemy; ++i)
                {
                var padding = 0.1f;
                var xPosition = rand.NextFloat(-1f * ((settings.levelWidth) / 2 - padding), (settings.levelWidth) / 2 - padding * 2);
                var yPosition = rand.NextFloat(-1f * ((settings.levelHeight) / 2 - padding), (settings.levelHeight) / 2 - padding * 3);
                var zPosition = 0;

                xPosition = (settings.levelWidth) / 3 - padding;
                yPosition = (settings.levelHeight) / 3 - padding;

                var pos = new Translation { Value = new float3(xPosition, yPosition, zPosition) };
                var e = commandBuffer.Instantiate(enemyPrefab);

                commandBuffer.SetComponent(e, pos);

                var randomVel = new Vector3(rand.NextFloat(-1f, 1f), rand.NextFloat(-1f, 1f), 0);
                randomVel.Normalize();
                randomVel = randomVel * settings.enemyVelocity;

                var vel = new PhysicsVelocity { Linear = new float3(randomVel.x, randomVel.y, randomVel.z) };
                commandBuffer.SetComponent(e, vel);

                }
            }).Schedule();

            m_BeginSimECB.AddJobHandleForProducer(Dependency);
        }
    }
}