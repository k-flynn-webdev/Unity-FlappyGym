using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSpin : MonoBehaviour, ISubscribeState
{
    public GameStateObj State { get; set; }

    [SerializeField]
    private float _speed = 1f;

    private bool _allowRot = false;

    private Vector3 _localRot = new Vector3();

    void Start()
    {
        ServiceLocator.Resolve<GameState>().SubscribeState(this);
        _localRot = Vector3.zero;
    }

    void Update()
    {
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        if (_allowRot)
        {
            _localRot.y -= Time.deltaTime * _speed;
            this.transform.localEulerAngles = _localRot;
        }
    }

    public void ReactState(GameStateObj state)
    {
        _allowRot = (state.state == GameStateObj.gameStates.Title ||
            state.state == GameStateObj.gameStates.Settings ||
            state.state == GameStateObj.gameStates.Play);

        if (state.last == GameStateObj.gameStates.Over &&
            state.state != GameStateObj.gameStates.Over)
        {
            _localRot = Vector3.zero;
        }
    }
}
