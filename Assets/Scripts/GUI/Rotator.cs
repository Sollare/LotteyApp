using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

    private Transform _transform;

    protected Transform cachedTransform
    {
        get
        {
            if (_transform) return transform;
            else return (_transform = transform);
        }
    }

    public float Speed = 50f;

    void Update()
    {
        cachedTransform.Rotate(0,0,Speed * Time.deltaTime);
    }
}
