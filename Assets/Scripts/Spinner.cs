using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField]
    private Transform _spinner;
    [SerializeField]
    private float RotatePerSecond = -10.0f;

    private float _angle;

    private void Update()
    {
        _angle += RotatePerSecond * Time.deltaTime;
        _angle = _angle <= -360.0f ? 0f : _angle;
        _spinner.localEulerAngles = new Vector3(0f, 0f, _angle);
    }
}