using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wave
{
    public float amp;
    [Range(0.0001f, int.MaxValue)]
    public float lmbd;
    public float flowSpeed;
    [Range(1f, 2f)]
    public float weight;
    public float offset;
}
