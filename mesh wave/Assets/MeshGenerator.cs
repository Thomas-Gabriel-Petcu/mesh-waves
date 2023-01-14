using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeshGenerator : MonoBehaviour
{
    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private Vector3[] _vertices;
    private int[] _trianglePoints;
    public int quadxSize = 2;
    public int quadzSize = 2;
    public float vertexSpacing;
    public int numberOfWaves;

    [Header("wave settings")]
    public float amp = 1;
    [Range(0.0001f, int.MaxValue)]
    public float lmbd;

    private float flowSpeed;
    const float g = 9.28f;
    private float k;
    private float[] xOrigins;

    private void Awake()
    {
        _mesh = new Mesh();
        _meshFilter = GetComponent<MeshFilter>();
        _meshFilter.mesh = _mesh;
        _mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
    }
    void Start()
    {
        GenerateShape();
    }

    private void Update()
    {
        if (lmbd < amp * 10)
        {
            lmbd = amp * 10;
        }
        Wave();
        UpdateShape();

    }
    private void GenerateShape()
    {
        float l_currentX = 0, l_currentZ = 0;
        _vertices = new Vector3[(quadxSize + 1) * (quadzSize + 1)];
        xOrigins = new float[_vertices.Length];
        for ( int z = 0, i = 0; z <= quadzSize; z++)
        {
            for (int x = 0; x <= quadxSize; x++)
            {
                _vertices[i] = new Vector3(l_currentX, 0, l_currentZ);
                xOrigins[i] = l_currentX;
                l_currentX += vertexSpacing;
                i++;
            }
            l_currentX = 0;
            l_currentZ += vertexSpacing;
        }
        _trianglePoints = new int[(quadxSize+1) * (quadzSize+1) * 6];
        int l_Vertex = 0;
        int l_triPointPosition = 0;
        for (int j = 0; j < quadzSize; j++)
        {
            for (int i = 0; i < quadxSize; i++)
            {
                _trianglePoints[l_triPointPosition] = l_Vertex;
                _trianglePoints[l_triPointPosition + 1] = l_Vertex + quadxSize + 1;
                _trianglePoints[l_triPointPosition + 2] = l_Vertex + 1;
                _trianglePoints[l_triPointPosition + 3] = l_Vertex + 1;
                _trianglePoints[l_triPointPosition + 4] = l_Vertex + quadxSize + 1;
                _trianglePoints[l_triPointPosition + 5] = l_Vertex + quadxSize + 2;
                l_Vertex++;
                l_triPointPosition += 6;
            }
            l_Vertex++;
        }
    }
    private void UpdateShape()
    {
        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = _trianglePoints;
        _mesh.RecalculateNormals();
    }
    private void Wave()
    {
        k = (2 * Mathf.PI) / lmbd;
        for (int i = 0; i < _vertices.Length; i++)
        {
            _vertices[i].x = xOrigins[i] + amp * 1 - Mathf.Cos(G(_vertices[i].x));
            _vertices[i].y = 1 - Mathf.Abs(amp * Mathf.Sin(G(_vertices[i].x)));
        }
    }

    private float G(float x)
    {
        flowSpeed = Mathf.Sqrt(g/k);
        return k *(x - flowSpeed * Time.time);
    }
}
