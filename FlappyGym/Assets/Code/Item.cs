using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    [SerializeField]
    private float _top = 0f;
    [SerializeField]
    private float _bottom = 0f;

    [SerializeField]
    private float _random = 1.5f;

    public void Place(float xPos = 0f)
    {
        this.transform.position = new Vector3(
            Random.Range(xPos - _random, xPos + _random),
            Random.Range(_top, _bottom),
            0f);
    }
}
