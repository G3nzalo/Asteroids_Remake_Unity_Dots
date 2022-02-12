using Unity.Entities;
using Unity.Mathematics;

namespace Enemy.Bullet.Component
{
    public struct BulletEnemySpawnOffsetComponent : IComponentData
    {
        public float3 Value;
    }
}