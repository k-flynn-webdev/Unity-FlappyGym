using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour, IObservable
{
    [SerializeField]
    private float _speed = 1f;
    [SerializeField]
    private float _jump = 4f;
    [SerializeField]
    private float _jumpDelay = 0.33f;
    [SerializeField]
    private float _gravity = -9f;


    private Vector3 _localPos;
    private float _yInertia = 0f;
    private float _xInertia = 0f;
    private bool _gameInPlay = false;



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

        if (Input.GetButtonDown("Fire1"))
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
        //var xPos = this._localPos.x + (this._speed * Time.deltaTime);
        var xPos = 0f;
        //var yPos = this._localPos.y + (Time.deltaTime * (_yInertia + (_gravity * 2f)));
        var yPos = _yInertia;
        this.transform.position = new Vector3(xPos, yPos, 0f);

        this.ResetInertia();
    }

    void ResetInertia()
    {
        if (this._jumpDelayVal > 0f)
        {
            this._jumpDelayVal -= Time.deltaTime;
            return;
        }

        if (this._yInertia >= 0f)
        {
            this._jumpDecayVal += Time.deltaTime;
            this._yInertia = this._yInertia - (this._jumpDecayVal * this._jumpDecayVal);
        }
    }

    private float _jumpDecayVal = 0f;
    private float _jumpDelayVal = 0f;

    void Jump()
    {
        if (this._yInertia > this._jump * 0.75f)
        {
            return;
        }

        this._jumpDecayVal = 0f;
        this._jumpDelayVal = this._jumpDelay;
        this._yInertia = this._jump;
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
