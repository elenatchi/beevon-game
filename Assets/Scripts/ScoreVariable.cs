using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName  = "Score Variable")]
public class ScoreVariable : ScriptableObject, ISerializationCallbackReceiver
{
    public int initialValue;
    public int runtimeValue;

    public void OnAfterDeserialize()
    {
        runtimeValue = initialValue;
    }

    public void OnBeforeSerialize()
    {

    }
}
