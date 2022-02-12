using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using TMPro;
public class SetGameSettingsSystem : UnityEngine.MonoBehaviour, IConvertGameObjectToEntity
{

    public float asteroidVelocity = 10f;

    public float enemyVelocity = 10f;

    public float playerForce = 50f;
    public float bulletVelocity = 500f;
    public int numAsteroids = 200;
    public int numEnemy = 50;
    public int levelWidth = 2048;
    public int levelHeight = 2048;
    public int levelDepth = 2048;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {

        var settings = default(GameSettings);

        settings.asteroidVelocity = asteroidVelocity;
        settings.enemyVelocity = enemyVelocity;

        settings.playerForce = playerForce;
        settings.bulletVelocity = bulletVelocity;
        settings.numAsteroids = numAsteroids;
        settings.numEnemy = numEnemy;
        settings.levelWidth = levelWidth;
        settings.levelHeight = levelHeight;
        settings.levelDepth = levelDepth;
        

        dstManager.AddComponentData(entity, settings);
    }
}
