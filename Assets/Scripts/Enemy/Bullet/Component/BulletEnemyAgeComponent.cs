using Unity.Entities;

namespace Enemy.Bullet.Component
{
    [GenerateAuthoringComponent]
    public struct BuelletEnemyAgeComponent : IComponentData
    {
        public BuelletEnemyAgeComponent(float maxAge)
        {
            this.maxAge = maxAge;
            age = 0;
        }

        public float age;
        public float maxAge;

    }
}