using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Jobs;
using UnityEngine;

namespace Player.System
{
    public class InputMovementSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireSingletonForUpdate<GameSettings>();
        }

        protected override void OnUpdate()
        {
            var gameSettings = GetSingleton<GameSettings>();
            var deltaTime = Time.DeltaTime;
            byte right, left, thrust, reverseThrust;

            right = left = thrust = reverseThrust = 0;

            float mouseX = 0;

            if (Input.GetKey("d"))
            {
                right = 1;
            }
            if (Input.GetKey("a"))
            {
                left = 1;
            }
            if (Input.GetKey("w"))
            {
                thrust = 1;
            }
            if (Input.GetKey("s"))
            {
                reverseThrust = 1;
            }
            if (Input.GetMouseButton(0))
            {
                mouseX = Input.GetAxis("Mouse X");
            }

            Entities
            .WithAll<CharacterTag>()
            .ForEach((Entity entity, int nativeThreadIndex, ref Rotation rotation, ref PhysicsVelocity velocity) =>
            {
                if (right == 1)
                {  
                velocity.Linear += (math.mul(rotation.Value, new float3(1, 0, 0)).xyz) * gameSettings.playerForce * deltaTime;
                }
                if (left == 1)
                {   
                velocity.Linear += (math.mul(rotation.Value, new float3(-1, 0, 0)).xyz) * gameSettings.playerForce * deltaTime;
                }
                if (thrust == 1)
                {   
                velocity.Linear += (math.mul(rotation.Value, new float3(0, 1, 0)).xyz) * gameSettings.playerForce * deltaTime;
                }
                if (reverseThrust == 1)
                {  
                velocity.Linear += (math.mul(rotation.Value, new float3(0, -1, 0)).xyz) * gameSettings.playerForce * deltaTime;
                }
                if (mouseX != 0)
                {  
                float lookSpeedH = 2f;

                Quaternion currentQuaternion = rotation.Value;
                    float yaw = currentQuaternion.eulerAngles.z;

                yaw += lookSpeedH * -mouseX;
                Quaternion newQuaternion = Quaternion.identity;
                    newQuaternion.eulerAngles = new Vector3(0, 0, yaw);
                    rotation.Value = newQuaternion;
                }
            }).ScheduleParallel();
        }
    }
}