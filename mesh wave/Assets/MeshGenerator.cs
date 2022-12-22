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

    public int xSize = 2;
    public int zSize = 2;
    public float waveFrequency;
    public float waveSpeed;
    public float waveAmplitude;
    private void Awake()
    {
        _mesh = new Mesh();
        _meshFilter = GetComponent<MeshFilter>();
        _meshFilter.mesh = _mesh;
    }
    // Start is called before the first frame update
    void Start()
    {
        GenerateShape();
    }

    private void Update()
    {
        WaveMotion(Time.timeSinceLevelLoad);
        UpdateShape();

    }
    private void GenerateShape()
    {
        float rand;
        _vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        for ( int z = 0, i = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                rand = Random.Range(0f, 1f);
                _vertices[i] = new Vector3(x, rand, z);
                i++;
            }
        }
        _trianglePoints = new int[xSize * zSize * 6];
        int l_Vertex = 0;
        int l_triPointPosition = 0;
        for (int j = 0; j < zSize; j++)
        {
            for (int i = 0; i < xSize; i++)
            {
                _trianglePoints[l_triPointPosition] = l_Vertex;
                _trianglePoints[l_triPointPosition + 1] = l_Vertex + xSize + 1;
                _trianglePoints[l_triPointPosition + 2] = l_Vertex + 1;
                _trianglePoints[l_triPointPosition + 3] = l_Vertex + 1;
                _trianglePoints[l_triPointPosition + 4] = l_Vertex + xSize + 1;
                _trianglePoints[l_triPointPosition + 5] = l_Vertex + xSize + 2;
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
    private void WaveMotion(float time)
    {
        for (int i = 0; i < _vertices.Length; i++)
        {
            _vertices[i].y = Mathf.Sin(time + _vertices[i].x * waveSpeed) * waveAmplitude;
        }
    }
}
