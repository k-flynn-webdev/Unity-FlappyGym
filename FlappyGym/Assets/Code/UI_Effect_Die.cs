using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Effect_Die : MonoBehaviour, ISubscribeEvent, ISubscribeState
{
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

        if (_whiteOutProgress > 5f)
        {
            TurnOff();
        }

        _whiteOutProgress += Time.deltaTime;

        _tmpProgress = Color.Lerp(_noEffect, _whiteEffect, _whiteAnim.Evaluate(_whiteOutProgress));
        _imageEffect.color = _tmpProgress;
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
        if (active && state.state != GameStateObj.gameStates.Over)
        {
            TurnOff();
        }
    }
}
