using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Level: MonoBehaviour
{
    [SerializeField]
    public int Id = 0;
    [SerializeField]
    public bool _isPlaying = false;

    public virtual void Setup()
    {
        Debug.Log("Setup");
        // do things
        // now go title ..
        Title();
        _isPlaying = false;
    }

    public virtual void Title()
    {
        Debug.Log("Title");
        _isPlaying = false;
    }

    public virtual void Play()
    {
        _isPlaying = true;
        Debug.Log("Play");
    }

    public virtual void Reset()
    {
        Debug.Log("Reset");
    }

    public virtual void Pause()
    {
        _isPlaying = false;
        Debug.Log("Pause");
    }

    // win / fail scenario
    public virtual void Over()
    {
        _isPlaying = false;
        Debug.Log("Over");
    }

    public virtual void UnLoad()
    {
        _isPlaying = false;
        Debug.Log("Unload");
        Destroy(this.gameObject, 5f);
        // kills self and removes GO
    }
}
