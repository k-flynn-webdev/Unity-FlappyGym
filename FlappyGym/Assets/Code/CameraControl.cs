﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private Transform _targetDefault;

    [SerializeField]
    private Vector3 _followAxes = new Vector3();

    [SerializeField]
    private Vector3 _followOffset = new Vector3();

    [SerializeField]
    private Vector3 _followSmoothing = new Vector3();

    [SerializeField]
    private Transform _targetLocal;
    private Vector3 _followAxesLocal = new Vector3();
    private Vector3 _followOffsetLocal = new Vector3();
    private Vector3 _followSmoothingLocal = new Vector3();


    void Awake()
    {
        SetTarget();
        _camera = GetComponent<Camera>();
        ServiceLocator.Register<CameraControl>(this);
    }

    public void SetTarget()
    {
        _targetLocal = _targetDefault;
        _followAxesLocal = _followAxes;
        _followOffsetLocal = _followOffset;
        _followSmoothingLocal = _followSmoothing;
    }

    public void SetTarget(Transform target)
    {
        _targetLocal = target;
        _followAxesLocal = _followAxes;
        _followOffsetLocal = _followOffset;
        _followSmoothingLocal = _followSmoothing;
    }

    public void SetTarget(Transform target, Vector3 followAxes)
    {
        _targetLocal = target;
        _followAxesLocal = followAxes;
        _followOffsetLocal = _followOffset;
        _followSmoothingLocal = _followSmoothing;
    }

    public void SetTarget(Transform target, Vector3 followAxes, Vector3 followOffset)
    {
        _targetLocal = target;
        _followAxesLocal = followAxes;
        _followOffsetLocal = followOffset;
        _followSmoothingLocal = _followSmoothing;
    }

    public void SetTarget(Transform target, Vector3 followAxes, Vector3 followOffset, Vector3 followSmooth)
    {
        _targetLocal = target;
        _followAxesLocal = followAxes;
        _followOffsetLocal = followOffset;
        _followSmoothingLocal = followSmooth;
    }

    float GetAxesPos(int axes)
    {
        float pos = Mathf.Lerp(
           this.transform.position[axes],
           _targetLocal.position[axes],
           _followSmoothingLocal[axes] * Time.deltaTime * _followAxesLocal[axes]);

        pos = Mathf.Lerp(pos, _targetLocal.position[axes] + _followOffsetLocal[axes], _followSmoothingLocal[axes] * Time.deltaTime);

        return pos;
    }

    void Update()
    {
        this.transform.position = new Vector3(GetAxesPos(0), GetAxesPos(1), GetAxesPos(2));
    }
}
