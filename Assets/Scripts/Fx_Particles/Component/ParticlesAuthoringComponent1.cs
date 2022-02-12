using Unity.Entities;

[GenerateAuthoringComponent]
public struct EnemyAuthoringComponent : IComponentData
{
    public Entity Prefab;
}