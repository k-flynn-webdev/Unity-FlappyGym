using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMove : MonoBehaviour, IObservable
{
    [SerializeField]
    private float _speed = 1f;
    [SerializeField]
    private float _jump = 4f;
    [SerializeField]
    private float _jumpDelay = 0.33f;
    [SerializeField]
    private float _gravityMS = 2.3f;
    [SerializeField]
    private float _gravityMaxSpeed = -12f;

    private Vector3 _localPos;
    private float _xInertia = 0f;
    private float _gravtyInertia = 0f;
    private bool _gameInPlay = false;

    private float _ground = -1f;

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
        this.ResetInertia();
    }


    void getLocalPos()
    {
        _localPos = transform.position;
    }

    void Move()
    {
        var xPos = 0f;
        var yPos = this._localPos.y;

        if (this._jumpDelayVal > 0f) {
            yPos += this._jumpDelayVal;
        }

        yPos += this._gravtyInertia * Time.deltaTime;

        if (yPos < this._ground)
        {
            yPos = this._ground;
        }

        this.transform.position = new Vector3(xPos, yPos, 0f);
    }

    void ResetInertia()
    {
        if (this._jumpDelayVal > 0f)
        {
            this._jumpDelayVal -= Time.deltaTime;
            return;
        }

        this._gravtyInertia = Mathf.Lerp(this._gravtyInertia, this._gravityMaxSpeed, Time.deltaTime * this._gravityMS);
    }

    private float _jumpDelayVal = 0f;

    void Jump()
    {
        if (this._gravtyInertia > this._jump * 0.5f)
        {
            return;
        }

        this._gravtyInertia = this._jump;
        this._jumpDelayVal = this._jumpDelay;
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
