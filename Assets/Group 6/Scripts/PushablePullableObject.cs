using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public interface PushablePullableObject
{
    public void push(Vector3 raycastIntersectionPoint, Vector3 playerVelocity, Vector3 raycastDirection);

    public void pull(Vector3 raycastIntersectionPoint, Vector3 playerVelocity, Vector3 raycastDirection);
}
