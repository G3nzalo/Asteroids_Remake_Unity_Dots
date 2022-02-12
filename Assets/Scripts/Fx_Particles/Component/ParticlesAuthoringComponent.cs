using Unity.Entities;

[GenerateAuthoringComponent]
public struct ParticlesAuthoringComponent : IComponentData
{
    public Entity Prefab;
}