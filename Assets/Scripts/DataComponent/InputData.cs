using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct InputData : IComponentData
{
    public KeyCode Up;
    public KeyCode Down;
    public KeyCode Right;
    public KeyCode Left;
    public KeyCode RotationR;
    public KeyCode RotationL;
    public KeyCode Fire;
}
