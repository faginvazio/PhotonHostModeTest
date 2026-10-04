using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public const byte MouseButton0 = 1;
    public const byte MouseButton1 = 2;

    public Vector3 Direction;

    public NetworkButtons Buttons;
}
