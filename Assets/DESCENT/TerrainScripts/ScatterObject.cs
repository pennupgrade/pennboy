using System;
using UnityEngine;

public class ScatterObject : MonoBehaviour
{
    private Action<ScatterObject> _killAction;

    public void Init(Action<ScatterObject> killAction, ref Action onChunkDelete)
    {
        _killAction = killAction;
        onChunkDelete += HandleChunkDelete;
    }
    public void HandleChunkDelete()
    {
        _killAction(this);
    }
}
