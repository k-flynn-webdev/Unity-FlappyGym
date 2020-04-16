using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Level: MonoBehaviour
{
    [SerializeField]
    public int Id = 0;

    public virtual void Setup()
    {
        Debug.Log("Setup");
        // do things
        // now go title ..
        this.Title();
    }

    public virtual void Title()
    {
        Debug.Log("Title");
    }

    public virtual void Play()
    {
        Debug.Log("Play");
    }

    public virtual void Reset()
    {
        Debug.Log("Reset");
    }

    public virtual void Pause()
    {
        Debug.Log("Pause");
    }

    // win / fail scenario
    public virtual void Over()
    {
        Debug.Log("Over");
    }

    public virtual void UnLoad()
    {
        Debug.Log("Unload");
        // kills self and removes GO
    }
}
