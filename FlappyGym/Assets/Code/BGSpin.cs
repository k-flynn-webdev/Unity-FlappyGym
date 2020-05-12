using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSpin : MonoBehaviour, ISubscribeState
{
    public GameStateObj State { get; set; }

    [SerializeField]
    private float _speed = 1f;

    private Vector3 _goal;
    private bool _isTitle = false;

    private float _updateTime = 0f;
    private Vector3 _localRot = new Vector3();

    void Start()
    {
        ServiceLocator.Resolve<GameState>().SubscribeState(this);
    }

    void Update()
    {

        UpdateRotation();
        UpdateGoal();
    }

    private void UpdateRotation()
    {
        if (_isTitle)
        {
            Vector3 _tmp = this.transform.localEulerAngles;
            _tmp.y -= Time.deltaTime * 0.5f;
            this.transform.localEulerAngles = _tmp;
            return;
        }

        _localRot = Vector3.MoveTowards(_localRot, _goal, Time.deltaTime * _speed);
        this.transform.localEulerAngles = _localRot;
    }


    private void UpdateGoal()
    {
        if (_updateTime > 0f)
        {
            _updateTime -= Time.deltaTime;
            return;
        }

        _goal.y = ServiceLocator.Resolve<LevelManager>().GetProgress().x * -1f;
        _updateTime = 0.66f;
    }

    public void ReactState(GameStateObj state)
    {
        _isTitle = (state.state == GameStateObj.gameStates.Title ||
            state.state == GameStateObj.gameStates.Settings);

        if (state.last == GameStateObj.gameStates.Over &&
            state.state == GameStateObj.gameStates.Play)
        {
            this.transform.localEulerAngles = Vector3.zero;
            _localRot = Vector3.zero;
            _updateTime = 1f;
            UpdateGoal();
        }
    }
}
