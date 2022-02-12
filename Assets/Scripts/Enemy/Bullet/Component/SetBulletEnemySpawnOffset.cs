using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Enemy.Bullet.Component
{
    public class SetBulletEnemySpawnOffset : UnityEngine.MonoBehaviour, IConvertGameObjectToEntity
    {
        public GameObject bulletEnemySpawn;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var bulletOffset = default(BulletEnemySpawnOffsetComponent);

            var offsetVector = bulletEnemySpawn.transform.position;
            bulletOffset.Value = new float3(offsetVector.x, offsetVector.y, offsetVector.z);

            dstManager.AddComponentData(entity, bulletOffset);
        }
    }
}