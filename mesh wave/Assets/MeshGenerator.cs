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
    //public Vector3 direction;

    [Header("wave settings")]
    public Wave[] waves;
    private Vector3[] originalPos;


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
        Wave();
        UpdateShape();
    }
    private void GenerateShape()
    {
        float l_currentX = 0, l_currentZ = 0;
        _vertices = new Vector3[(quadxSize + 1) * (quadzSize + 1)];
        originalPos = new Vector3[_vertices.Length];
        for ( int z = 0, i = 0; z <= quadzSize; z++)
        {
            for (int x = 0; x <= quadxSize; x++)
            {
                _vertices[i] = new Vector3(l_currentX, 0, l_currentZ);
                originalPos[i] = _vertices[i];
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
        Vector3 store = new Vector3();
        for (int i = 0; i < _vertices.Length; i++)
        {
            //k = (2 * Mathf.PI) / lmbd;
            //amp = s / k;
            foreach (Wave wave in waves)
            {
                store.x += originalPos[i].x + wave.direction.x * wave.GetAmp() * Mathf.Cos(G(originalPos[i], wave));
                store.y += wave.GetAmp() * Mathf.Sin(G(originalPos[i], wave));
                store.z += originalPos[i].z + wave.direction.z * wave.GetAmp() * Mathf.Cos(G(originalPos[i], wave));
            }
            //store.z = _vertices[i].z;
            _vertices[i] = store;
            store = Vector3.zero;
            //_vertices[i].x = xOrigins[i] + amp * Mathf.Cos(G(xOrigins[i]));
            //_vertices[i].y = amp * Mathf.Sin(G(xOrigins[i]));
            Color color = new Color();
            color = Color.Lerp();
        }
    }
    private float G(Vector3 pos, Wave wave)
    {
        float k;
        k = (2 * Mathf.PI) / wave.lmbd;
        return k * (wave.direction.x * pos.x + wave.direction.z * pos.z - wave.offset - wave.flowSpeed * Time.time);
    }
}
