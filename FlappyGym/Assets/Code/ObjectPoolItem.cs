using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectPoolItem)), CanEditMultipleObjects]
public class ObjectPoolItemEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Object[] monoObjects = targets;

        if (GUILayout.Button("Return"))
        {
            for (int i = 0; i < monoObjects.Length; i++)
            {
                ObjectPoolItem myScript = (ObjectPoolItem)monoObjects[i];
                myScript.IsNotActive();
            }
        }
    }
}

public class ObjectPoolItem : MonoBehaviour
{
    [SerializeField]
    public bool Active
    { get { return this._isActive; } }

    private bool _isActive = false;


    public void IsActive()
    {
        _isActive = true;
        this.gameObject.SetActive(true);
    }

    public void IsNotActive()
    {
        _isActive = false;
        this.gameObject.SetActive(false);
    }

    public ObjectPoolItem CreateItem()
    {
        ObjectPoolItem tmpObj = Instantiate(this, new Vector3(0, 0, 0), Quaternion.identity);
        tmpObj.IsActive();
        return tmpObj;
    }
}
