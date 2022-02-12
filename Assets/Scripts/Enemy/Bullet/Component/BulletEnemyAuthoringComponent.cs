using Unity.Entities;

[GenerateAuthoringComponent]
public struct BulletEnemyAuthoringComponent : IComponentData
{
    public Entity Prefab;
}