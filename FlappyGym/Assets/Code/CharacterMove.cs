using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMove : MonoBehaviour, ISubscribeState, ISubscribeEvent, IReset
{

    public GameStateObj State { get; set; }

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
    private float _rotateOnChange = 5f;
    [SerializeField]
    private float _rotateBias = 0f;


    [SerializeField]
    private bool _slowWhenJumping = true;
    [SerializeField]
    private float _jumpInterrupt = 0.3f;
    [SerializeField]
    private float _ground = 0f;

    private Quaternion _rotNormal = new Quaternion();
    [SerializeField]
    private Quaternion _rotJump = new Quaternion();
    [SerializeField]
    private Quaternion _rotFall = new Quaternion();


    private Vector3 _localPos;
    private bool _gameInPlay = false;
    private bool _gameOver = false;
    private bool _allowInput = false;


    private Vector3 _speedVar = new Vector3();
    private Vector3 _jumpVar = new Vector3();
    private Vector3 _fallVar = new Vector3();
    private Vector3 _hitVar = new Vector3();
    private Vector3 _inertiaVar = new Vector3();
    private Vector3 _lastPos = new Vector3();

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
        ServiceLocator.Resolve<GameState>().SubscribeState(this);
        ServiceLocator.Resolve<GameEvent>().SubscribeEvent(this);
        _rotNormal = this.transform.localRotation;
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
        _hitTimer = 0f;
        _groundTimer = 0f;

        _isJump = false;
        _isFall = false;
        _isHit = false;
        _isGround = false;

        _allowInput = false;
    }

    void Update()
    {
        if (_gameInPlay || _gameOver)
        {
            this.getLocalPos();

            if (_allowInput)
            {
                if (_gameInPlay &&
                    Input.GetButtonDown("Fire1") &&
                    !EventSystem.current.IsPointerOverGameObject())
                {
                    this.Jump();
                }
            }

            this.updateGround();
            this.updateSpeed();
            this.updateJump();
            this.updateFall();
            this.updateHit();
            this.updateInertia();

            this.Rotate();
            this.Move();
        }
    }

    void getLocalPos()
    {
        _localPos = transform.position;
    }

    void Move()
    {
        _lastPos = _localPos;

        this.transform.position = _localPos +
            (_speedVar * Time.deltaTime) +
            (_inertiaVar * Time.deltaTime) +
            (_hitVar * Time.deltaTime);
    }


    void Rotate()
    {

        float rotDir = _localPos.y - _lastPos.y;
        Quaternion goal = new Quaternion();

        if (rotDir > _rotateBias)
        {
            goal = Quaternion.Lerp(_rotNormal, _rotJump, rotDir * _rotateOnChange);
        } else
        {
            goal = Quaternion.Lerp(_rotNormal, _rotFall, (rotDir * -1f) * _rotateOnChange);
        }

        this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, goal, Time.deltaTime * 20f);
        return;
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
        if (!_gameInPlay)
        {
            _speedVar = Vector3.Lerp(_speedVar, Vector3.zero, Time.deltaTime * 10f);
            return;
        }

        if (_isHit)
        {
            _speedVar = Vector3.Lerp(_speedVar, Vector3.zero, Time.deltaTime * 10f);
            return;
        }

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
            _jumpVar = Vector3.Lerp(Vector3.zero, _jump, _jumpVel.Evaluate(_jumpTimer/_jumpDuration));
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
            float curveY = _jumpVel.Evaluate(_jumpTimer / _jumpDuration);
            _localPos += (_jumpVar * curveY);
            return;
        }
        if (_isHit)
        {
            _target = _fallVar;
        }

        _inertiaVar = Vector3.Lerp(_inertiaVar, _target, Time.deltaTime * 15f);
    }

    void OnTriggerEnter(Collider hitBy) { this.TriggerCol(hitBy); }

    void TriggerCol(Collider hitBy)
    {
    }

    public void Force(float force, Vector3 point)
    {

        if (_hitTimer > 0.75f)
        {
            return;
        }

        // work out moving same dir..
        Vector3 thisDir = this.transform.position - _lastPos;
        Vector3 otherDir = this.transform.position - point;

        float dotPro = Vector3.Dot(thisDir, otherDir);

        if (dotPro < 0.2f)
        {
            _speedTimer = 0f;
            _speedVar = Vector3.zero;
        }


        _isGround = false;
        _isJump = false;
        _isFall = true;
        _isHit = true;
        _hitTimer = 1.5f;
        _hitVar = otherDir * force;

        _jumpTimer = 5f;
        _fallTimer = 0f;
        _jumpVar = Vector3.zero;
        _fallVar = Vector3.zero;
    }

    void Jump()
    {
        if (_jumpTimer < _jumpInterrupt)
        {
            return;
        }

        _localPos += Vector3.up * 0.1f;
        _jumpTimer = 0f;
        _fallTimer = 0f;
        _fallVar = Vector3.zero;
        _jumpVar = Vector3.zero;
        _inertiaVar = Vector3.zero;
    }

    public void ReactState(GameStateObj state) {
        State = state;
        _gameInPlay = state.state == GameStateObj.gameStates.Play;
        _gameOver = state.state == GameStateObj.gameStates.Over;
    }

    public void ReactEvent(string state)
    {
        if (state.Equals("DisablePlayerInput"))
        {
            _allowInput = false;
            return;
        }

        if (state.Equals("AllowPlayerInput"))
        {
            _allowInput = true;
            return;
        }
    }
}
