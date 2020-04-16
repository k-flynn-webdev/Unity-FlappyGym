using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Level: MonoBehaviour
{
    [SerializeField]
    public int Id = 0;

    public virtual void Setup()
    {
        // do things
        // now go title ..
        this.Title();
    }

    public virtual void Title()
    {

    }

    public virtual void Play()
    {

    }

    public virtual void Reset()
    {

    }

    public virtual void Pause()
    {

    }

    // win / fail scenario
    public virtual void Over()
    {

    }

    public virtual void UnLoad()
    {

    }
}
