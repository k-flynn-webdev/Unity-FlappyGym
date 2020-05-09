using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Effect_Die : MonoBehaviour, ISubscribeEvent, ISubscribeState
{

    public GameStateObj State { get; set; }

    private bool active = false;
    private float _whiteOutProgress = 0f;
    private Image _imageEffect;

    private Color _noEffect;

    [SerializeField]
    public Color _whiteEffect;

    private Color _tmpProgress;

    [SerializeField]
    private AnimationCurve _whiteAnim;

    void Start()
    {
        _imageEffect = GetComponent<Image>();
        _noEffect = _imageEffect.color;
        ServiceLocator.Resolve<GameEvent>().SubscribeEvent(this);
        TurnOff();
    }

    void Update()
    {
        UpdateWhiteout();
    }


    private void UpdateWhiteout()
    {
        if (!active)
        {
            return;
        }

        _whiteOutProgress += Time.deltaTime;

        _tmpProgress = Color.Lerp(_noEffect, _whiteEffect, _whiteAnim.Evaluate(_whiteOutProgress));
        _imageEffect.color = _tmpProgress;

        if (_whiteOutProgress > 5f)
        {
            TurnOff();
        }

    }

    private void TurnOn()
    {
        active = true;
        this.gameObject.SetActive(true);
        _whiteOutProgress = 0f;
    }

    private void TurnOff()
    {
        active = false;
        this.gameObject.SetActive(false);
        _imageEffect.color = _noEffect;
        _whiteOutProgress = 0f;
    }

    public void ReactEvent(string state)
    {
        if (state == "Died" && !active)
        {
            TurnOn();
        }
    }

    public void ReactState(GameStateObj state)
    {
        State = state;

        if (active && state.state != GameStateObj.gameStates.Over)
        {
            TurnOff();
        }
    }
}
