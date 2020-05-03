using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSpin : MonoBehaviour, IReset
{
    [SerializeField]
    private bool _active = true;

    [SerializeField]
    private Quaternion _goal;
    private Quaternion _normal;

    [SerializeField]
    private float _length = 1f;
    private float _progress = 0f;

    [SerializeField]
    private AnimationCurve _animCurve;

    public void Reset()
    {
        _active = true;
        _progress = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        _normal = this.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpin();
    }


    void UpdateSpin()
    {
        if (!this.gameObject.activeSelf)
        {
            return;
        }

        _progress += Time.deltaTime;
        this.transform.localRotation = Quaternion.Lerp(_normal, _goal, _animCurve.Evaluate(_progress / _length));

        if (_progress / _length > 1f)
        {
            _progress = 0f;
        }
    }
}
