using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMove : MonoBehaviour, ISubscribe
{
    [SerializeField]
    private Vector3 _speed = new Vector3();
    [SerializeField]
    private float _speedMax = 2f;
    [SerializeField]
    private bool _slowWhenJumping = true;
    [SerializeField]
    private Vector3 _jump = new Vector3();
    [SerializeField]
    private float _jumpDelay = 0.3f;
    [SerializeField]
    private Vector3 _gravity = new Vector3();

    [SerializeField]
    private float _weight = 4f;

    private bool _gameInPlay = false;

    private Vector3 _localPos;
    [SerializeField]
    private float _ground = 0f;
    private bool _isJumping = false;
    private float _jumpTimer = 0f;

    private Vector3 _hitVel = new Vector3();
    private Vector3 _inertiaVel = new Vector3();
    private Vector3 _speedVel = new Vector3();


    void Start()
    {
        ServiceLocator.Resolve<GameState>().Subscribe(this);
    }

    public void Reset ()
    {
        _jumpTimer = 0f;
        _hitVel = new Vector3();
        _inertiaVel = new Vector3();
        _speedVel = new Vector3();
    }

    void Update()
    {
        if (!_gameInPlay)
        {
            return;
        }

        this.getLocalPos();

        if (Input.GetButtonDown("Fire1") &&
            !EventSystem.current.IsPointerOverGameObject())
        {
            this.Jump();
        }

        this.updateHit();
        this.updateJump();
        this.updateSpeed();

        this.Move();
    }


    void getLocalPos()
    {
        _localPos = transform.position;
    }

    void Move()
    {
        var xPos = this._localPos.x;
        var yPos = this._localPos.y;

        xPos += Time.deltaTime * _hitVel.x;
        xPos += Time.deltaTime * _inertiaVel.x;
        xPos += Time.deltaTime * _speedVel.x;

        yPos += Time.deltaTime * _hitVel.y;
        yPos += Time.deltaTime * _inertiaVel.y;
        yPos += Time.deltaTime * _speedVel.y;

        if (yPos < this._ground)
        {
            yPos = this._ground;
        }

        this.transform.position = new Vector3(xPos, yPos, 0f);
    }



    void updateSpeed()
    {
        if (_isJumping && _slowWhenJumping)
        {
            _speedVel = Vector3.Lerp(_speedVel, Vector3.zero, Time.deltaTime);
            return;
        }

        _speedVel = Vector3.Lerp(_speedVel, _speed, Time.deltaTime * _speedMax);
    }

    void OnTriggerEnter(Collider hitBy) { this.TriggerCol(hitBy); }

    void TriggerCol(Collider hitBy)
    {
        bool isBounce = false;
        float force = 2f;

        if (hitBy.CompareTag("Bounce"))
        {
            force = 5f;
            isBounce = true;
        }

        if (hitBy.tag == "Death")
        {
            force = 13f;
            isBounce = true;
        }

        if (!isBounce)
        {
            return;
        }

        _isJumping = true;
        _inertiaVel = Vector3.zero;
        _jumpTimer = _jumpDelay;
        _hitVel = (this.transform.position - hitBy.transform.position) * force;
    }


    void Jump()
    {
        if (_isJumping)
        {
            return;
        }

        _isJumping = true;
        _inertiaVel = _jump * _weight;
        _jumpTimer = _jumpDelay;
    }

    void updateHit()
    {
        _hitVel = Vector3.Lerp(_hitVel, Vector3.zero, Time.deltaTime);
    }

    void updateJump()
    {
        if (_jumpTimer > 0f)
        {
            _jumpTimer -= Time.deltaTime;
        }

        _isJumping = _jumpTimer > 0f;

        Vector3 newGoal = Vector3.Lerp(_gravity, _jump, (_jumpTimer / _jumpDelay));

        _inertiaVel = Vector3.Lerp(_inertiaVel, newGoal, Time.deltaTime);
    }

    public void React(GameStateObj state) {
        _gameInPlay = state.state == GameStateObj.gameStates.Play;
    }
}
