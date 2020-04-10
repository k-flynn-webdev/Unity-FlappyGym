using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMove : MonoBehaviour, IObservable
{
    [SerializeField]
    private float _speed = 1f;
    [SerializeField]
    private float _jump = 3f;
    [SerializeField]
    private float _jumpPeakDiff = 0.5f;
    [SerializeField]
    private float _jumpPeak = 0.5f;
    [SerializeField]
    private float _jumpMS = 2.3f;
    [SerializeField]
    private float _gravityMS = 2.3f;
    [SerializeField]
    private float _gravityMaxSpeed = -12f;

    private Vector3 _localPos;
    //private float _xInertia = 0f;
    //private float _yInertia = 0f;
    private bool _gameInPlay = false;

    private float _ground = 0f;

    // Start is called before the first frame update
    void Start()
    {
        ServiceLocator.Resolve<GameState>().Subscribe(this);
    }

    // Update is called once per frame
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

        this.Move();
    }


    void getLocalPos()
    {
        _localPos = transform.position;
    }

    void Move()
    {
        var xPos = 0f;
        var yPos = this.JumpUpdate();

        if (yPos < this._ground)
        {
            yPos = this._ground;
        }

        this.transform.position = new Vector3(xPos, yPos, 0f);
    }


    private bool _isJumping = false;
    private bool _isPeaked = false;
    private bool _isFalling = false;

    private float _jumpTarget = 0f;
    private float _jumpPeakDelay = 0f;
    private float _jumpMSVel = 0f;
    private float _playerFallVel = 0f;


    void Jump()
    {
        _isJumping = true;
        _isPeaked = false;
        _isFalling = false;

        this._jumpMSVel = this._jumpMS;
        this._playerFallVel = 0f;
        this._jumpPeakDelay = this._jumpPeak;
        this._jumpTarget = this._localPos.y + this._jump;
    }

    float JumpUpdate()
    {
        var yPos = this._localPos.y;

        if (_isJumping || _isPeaked)
        {
            yPos = Mathf.Lerp(yPos, this._jumpTarget, Time.deltaTime * this._jumpMSVel);
        }

        if (_isJumping)
        {
            var diff = Mathf.Abs(yPos - this._jumpTarget);
            if (diff < _jumpPeakDiff)
            {
                _isJumping = false;
                _isPeaked = true;
            }
        }

        if (_isPeaked)
        {
            this._jumpMSVel = Mathf.Lerp(this._jumpMSVel, 0.1f, Time.deltaTime * this._jumpMS);
            _jumpPeakDelay -= Time.deltaTime;

            if (_jumpPeakDelay < 0f)
            {
                _isPeaked = false;
                _isFalling = true;
            }
        }

        if (_isFalling)
        {
            this._playerFallVel = Mathf.Lerp(this._playerFallVel, this._gravityMaxSpeed, Time.deltaTime * this._gravityMS);
        }

        yPos += this._playerFallVel * Time.deltaTime;

        return yPos;
    }


    public List<IObservable> Subscribers
    { get; }

    public void Notify(){}

    public void React(GameStateObj state) {
        _gameInPlay = state.state == GameStateObj.gameStates.Play;
    }

    public void Subscribe(IObservable listener){}
    public void UnSubscribe(IObservable listener){}
}
