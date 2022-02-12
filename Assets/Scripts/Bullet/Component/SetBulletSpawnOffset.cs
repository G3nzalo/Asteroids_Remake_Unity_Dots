using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Bullet.Component
{
    public class SetBulletSpawnOffset : UnityEngine.MonoBehaviour, IConvertGameObjectToEntity
    {
        public GameObject bulletSpawn;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var bulletOffset = default(BulletSpawnOffsetComponent);

            var offsetVector = bulletSpawn.transform.position;
            bulletOffset.Value = new float3(offsetVector.x, offsetVector.y, offsetVector.z);

            dstManager.AddComponentData(entity, bulletOffset);
        }
    }
}