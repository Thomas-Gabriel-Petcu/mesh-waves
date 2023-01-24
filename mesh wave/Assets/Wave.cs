using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wave
{
    [Range(0.0001f,1f)]
    public float s;
    //public float amp;
    [Range(0.0001f, int.MaxValue)]
    public float lmbd;
    public float flowSpeed;
    [Range(1f, 2f)]
    public float weight;
    public float offset;
    public Vector3 direction;

    public float GetAmp()
    {
        float k = (Mathf.PI * 2) / lmbd;
        return s / k;
    }
}
