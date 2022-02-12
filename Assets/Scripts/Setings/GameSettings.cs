using System;
using Unity.Entities;

[Serializable]
public struct GameSettings : IComponentData
{
    public float asteroidVelocity;
    public float enemyVelocity;
    public float playerForce;
    public float bulletVelocity;
    public int numAsteroids;
    public int numEnemy;
    public int levelWidth;
    public int levelHeight;
    public int levelDepth;

}
