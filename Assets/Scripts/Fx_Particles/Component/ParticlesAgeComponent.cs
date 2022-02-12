using Unity.Entities;
namespace Fx_Particles.Component
{
    [GenerateAuthoringComponent]
    public struct ParticlesAgeComponent : IComponentData
    {
        public ParticlesAgeComponent(float maxAge)
        {
            this.maxAge = maxAge;
            age = 0;
        }

        public float age;
        public float maxAge;

    }
}