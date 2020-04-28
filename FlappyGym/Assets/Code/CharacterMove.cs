using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMove : MonoBehaviour, ISubscribe
{
    [SerializeField]
    private Vector3 _speed = new Vector3();
    [SerializeField]
    private Vector3 _jump = new Vector3();
    [SerializeField]
    private Vector3 _fall = new Vector3();

    [SerializeField]
    private float _speedDuration = 1f;
    [SerializeField]
    public AnimationCurve _speedVel;
    [SerializeField]
    private float _jumpDuration = 0.3f;
    [SerializeField]
    public AnimationCurve _jumpVel;
    [SerializeField]
    private float _fallDuration = 0.3f;
    [SerializeField]
    public AnimationCurve _fallVel;


    [SerializeField]
    private bool _slowWhenJumping = true;
    [SerializeField]
    private float _jumpInterrupt = 0.3f;
    [SerializeField]
    private float _ground = 0f;



    private Vector3 _localPos;
    private bool _gameInPlay = false;


    private Vector3 _speedVar = new Vector3();
    private Vector3 _jumpVar = new Vector3();
    private Vector3 _fallVar = new Vector3();
    private Vector3 _hitVar = new Vector3();
    private Vector3 _inertiaVar = new Vector3();


    private bool _isJump = false;
    private bool _isFall = false;
    private bool _isHit = false;
    private bool _isGround = false;

    private float _speedTimer = 0f;
    private float _jumpTimer = 0f;
    private float _fallTimer = 0f;
    private float _hitTimer = 0f;
    private float _groundTimer = 0f;




    void Start()
    {
        ServiceLocator.Resolve<GameState>().Subscribe(this);
    }

    public void Reset ()
    {
        _speedVar = new Vector3();
        _hitVar = new Vector3();
        _jumpVar = new Vector3();
        _fallVar = new Vector3();

        _speedTimer = 0f;
        _jumpTimer = 0f;
        _fallTimer = 0f;
        _hitTimer = 5f;
        _groundTimer = 0f;

        _isJump = false;
        _isFall = false;
        _isHit = false;
        _isGround = false;
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

        this.updateGround();
        this.updateSpeed();
        this.updateJump();
        this.updateFall();
        this.updateHit();
        this.updateInertia();

        this.Move();
    }


    void getLocalPos()
    {
        _localPos = transform.position;
    }

    void Move()
    {
        this.transform.position = _localPos +
            (_speedVar * Time.deltaTime) +
            (_inertiaVar * Time.deltaTime) +
            (_hitVar * Time.deltaTime);
    }



    void updateGround()
    {
        bool grndTest = (_localPos.y < _ground + 0.05f);

        if (grndTest && grndTest != _isGround)
        {
            _groundTimer = 0f;
        }

        _isGround = grndTest;

        if (_isGround)
        {
            _localPos = new Vector3(_localPos.x, Mathf.Lerp(_localPos.y, _ground, Time.deltaTime * 10f), _localPos.z);

        }

        if (_isGround && _groundTimer < 5f)
        {
            _groundTimer += Time.deltaTime;
        }
    }

    void updateSpeed()
    {
        if (_isJump && _slowWhenJumping)
        {
            _speedVar = Vector3.Lerp(_speedVar, Vector3.zero, Time.deltaTime);
            return;
        }

        _speedVar = Vector3.Lerp(Vector3.zero, _speed, _speedVel.Evaluate(_speedTimer/_speedDuration));

        if (_speedTimer < 5f)
        {
            _speedTimer += Time.deltaTime;
        }
    }

    void updateJump()
    {

        bool jmpTest = (!_isGround && _jumpTimer < _jumpDuration);

        if (jmpTest && jmpTest != _isJump)
        {
            _jumpTimer = 0f;
        }

        _isJump = jmpTest;

        if (_isJump)
        {
            _jumpVar = Vector3.Lerp(Vector3.down, _jump, _jumpVel.Evaluate(_jumpTimer/_jumpDuration));
        }

        if (_jumpTimer < 5f)
        {
            _jumpTimer += Time.deltaTime;
        }
    }

    void updateFall()
    {

        bool fallTest = (!_isJump && !_isGround);

        if (fallTest && fallTest != _isFall)
        {
            _fallTimer = 0f;
        }

        _isFall = fallTest;

        if (_isFall)
        {
            _fallVar = Vector3.Lerp(Vector3.zero, _fall, _fallVel.Evaluate(_fallTimer/_fallDuration));
        }

        if (_isFall && _fallTimer < 5f)
        {
            _fallTimer += Time.deltaTime;
        }
    }

    void updateHit()
    {

        _isHit = _hitTimer > 0f;

        if (_isHit)
        {
            _hitTimer -= Time.deltaTime;
            _hitVar = Vector3.Lerp(_hitVar, Vector3.zero, Time.deltaTime * 5f);
        }
    }

    void updateInertia()
    {
        Vector3 _target = Vector3.zero;

        if (_isFall)
        {
            _target = _fallVar;
        }
        if (_isJump)
        {
            _target = _jumpVar;
        }

        _inertiaVar = Vector3.Lerp(_inertiaVar, _target, Time.deltaTime * 15f);
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

        if (!isBounce)
        {
            return;
        }

        _isGround = false;
        _isJump = false;
        _isFall = true;
        _hitTimer = 1.5f;
        _hitVar = (this.transform.position - hitBy.transform.position) * force;
    }

    void Jump()
    {
        if (_jumpTimer < _jumpInterrupt)
        {
            return;
        }

        _jumpTimer = 0f;
        _localPos += Vector3.up * 0.25f;
    }

    //void updateHit()
    //{
    //    _hitVel = Vector3.Lerp(_hitVel, Vector3.zero, Time.deltaTime);
    //}

    //void updateJump()
    //{
    //    if (_jumpTimer > 0f)
    //    {
    //        _jumpTimer -= Time.deltaTime;
    //    }

    //    _canInteruptJump = _jumpTimer > 0f;

    //    Vector3 newGoal = Vector3.Lerp(_gravity, _jump, _jumpVel.Evaluate(_jumpTimer));

    //    _inertiaVel = Vector3.Lerp(_inertiaVel, newGoal, Time.deltaTime);
    //}

    public void React(GameStateObj state) {
        _gameInPlay = state.state == GameStateObj.gameStates.Play;
    }
}
