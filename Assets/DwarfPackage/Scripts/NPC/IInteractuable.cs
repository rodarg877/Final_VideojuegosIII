using System;
using UnityEngine;

public interface IInteractuable
{
    void OnInteraction();
    void OffInteraction(Action completion);
    Transform PositionToFace();
}
