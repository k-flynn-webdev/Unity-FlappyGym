using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Level: MonoBehaviour
{
    public string ID { get { return this._id; } }
    public Vector3 Progress { get { return this._progress; } }

    [SerializeField]
    private string _id;
    [SerializeField]
    private Vector3 _progress;
    [SerializeField]
    private bool _loaded = false;
    [SerializeField]
    private bool _ready = false;


    public void SetProgress(Vector3 progress)
    {
        _progress = progress;
    }

    public void SetReady(bool isReady)
    {
        _ready = isReady;
    }

    public virtual void Load()
    {
        _loaded = true;
        ServiceLocator.Resolve<GameState>().SetStateTitle();
    }

    // kills self and removes GO
    public virtual void UnLoad()
    {
        _loaded = false;
        Destroy(this.gameObject, 5f);
    }

    public virtual void TitlePre(GameStateObj state)
    {
        SetReady(true);
    }

    public virtual void Title() { }

    public virtual void TitlePost(GameStateObj state)
    {
        SetReady(false);
    }

    public virtual void PlayPre(GameStateObj state)
    {
        SetReady(true);
    }

    public virtual void Play() { }

    public virtual void PlayPost(GameStateObj state)
    {
        SetReady(false);
    }

    public virtual void PausePre(GameStateObj state)
    {
        SetReady(true);
    }

    public virtual void Pause() { }

    public virtual void PausePost(GameStateObj state)
    {
        SetReady(false);
    }

    public virtual void OverPre(GameStateObj state)
    {
        SetReady(true);
    }

    public virtual void Over() { }

    public virtual void OverPost(GameStateObj state)
    {
        SetReady(false);
    }
}
