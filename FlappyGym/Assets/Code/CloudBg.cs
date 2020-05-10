using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudBg : MonoBehaviour, IReset
{

    [SerializeField]
    private float _cloudMoveSpeed = 1f;
    [SerializeField]
    private Vector3 _cloudDirection;
    [SerializeField]
    private float _heightRange = 2f;

    private float _moveSpeed = 0f;



    void Start()
    {
        InitCloud();
    }

    public void Reset()
    {
        InitCloud();
    }

    void Update()
    {
        UpdateCloud();
    }

    private void InitCloud()
    {
        _moveSpeed = Random.Range(_cloudMoveSpeed * 0.1f, _cloudMoveSpeed * 1.25f);
        this.transform.localEulerAngles = new Vector3(Random.Range(0f, 180f), Random.Range(0f, 180f), Random.Range(0f, 180f));
        float newScale = Random.Range(0.25f, 1.5f);
        this.transform.localScale = Vector3.one * newScale;
        Vector3 pos = this.transform.position;
        pos.y += Random.Range(_heightRange * -1f, _heightRange * 1f);
        pos.z += Random.Range(_heightRange * -1f, _heightRange * 1f);
        this.transform.position = pos;
    }

    private void UpdateCloud()
    {
        this.transform.position += _cloudDirection.normalized * (Time.deltaTime * _moveSpeed);
    }
}
