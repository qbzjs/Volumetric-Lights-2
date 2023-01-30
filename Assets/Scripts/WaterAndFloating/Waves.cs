using System;
using System.Collections.Generic;
using Singleton;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Waves : MonoBehaviour
{
    //Properties
    [SerializeField] private int _dimension = 10;
    [SerializeField] private float _UVScale = 2f;
    [SerializeField] private Transform _waterPosition;
    [SerializeField] private Octave[] _octaves;

    //Mesh
    protected MeshFilter MeshFilter;
    protected Mesh Mesh;
    private List<Vector3> _vertices = new List<Vector3>();

    private void Start()
    {
        //mesh generation
        MeshFilter = GetComponent<MeshFilter>();
        Mesh = MeshFilter.mesh;
        Mesh.name = gameObject.name;
        Mesh.vertices = GenerateVertices();
        Mesh.triangles = GenerateTriangles();
        Mesh.uv = GenerateUVs();
        Mesh.RecalculateNormals();
        Mesh.RecalculateBounds();

        //position & scale
        transform.position = _waterPosition.position;
        transform.localScale = _waterPosition.localScale;

        Mesh.GetVertices(_vertices);
        WaveGeneration();
    }

    private void Update()
    {
        WaveGeneration();
    }

    public float GetHeight(Vector3 position)
    {
        //scale factor and position in local space
        var scale = new Vector3(1 / transform.lossyScale.x, 0, 1 / transform.lossyScale.z);
        var localPos = Vector3.Scale((position - transform.position), scale);

        //get edge points
        var p1 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Floor(localPos.z));
        var p2 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Ceil(localPos.z));
        var p3 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Floor(localPos.z));
        var p4 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Ceil(localPos.z));

        //clamp if the position is outside the plane
        p1.x = Mathf.Clamp(p1.x, 0, _dimension);
        p1.z = Mathf.Clamp(p1.z, 0, _dimension);
        p2.x = Mathf.Clamp(p2.x, 0, _dimension);
        p2.z = Mathf.Clamp(p2.z, 0, _dimension);
        p3.x = Mathf.Clamp(p3.x, 0, _dimension);
        p3.z = Mathf.Clamp(p3.z, 0, _dimension);
        p4.x = Mathf.Clamp(p4.x, 0, _dimension);
        p4.z = Mathf.Clamp(p4.z, 0, _dimension);

        //get the max distance to one of the edges and take that to compute max - dist
        var max = Mathf.Max(Vector3.Distance(p1, localPos), Vector3.Distance(p2, localPos),
            Vector3.Distance(p3, localPos), Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        var dist = (max - Vector3.Distance(p1, localPos))
                   + (max - Vector3.Distance(p2, localPos))
                   + (max - Vector3.Distance(p3, localPos))
                   + (max - Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        //weighted sum


        var height = _vertices[Index(p1.x, p1.z)].y * (max - Vector3.Distance(p1, localPos))
                     + _vertices[Index(p2.x, p2.z)].y * (max - Vector3.Distance(p2, localPos))
                     + _vertices[Index(p3.x, p3.z)].y * (max - Vector3.Distance(p3, localPos))
                     + _vertices[Index(p4.x, p4.z)].y * (max - Vector3.Distance(p4, localPos));

        //scale
        return height * transform.lossyScale.y / dist;
    }

    #region Mesh Generation

    private Vector3[] GenerateVertices()
    {
        Vector3[] verts = new Vector3[(_dimension + 1) * (_dimension + 1)];

        //equaly distributed verts
        for (int x = 0; x <= _dimension; x++)
        for (int z = 0; z <= _dimension; z++)
            verts[Index(x, z)] = new Vector3(x, 0, z);

        return verts;
    }

    private int[] GenerateTriangles()
    {
        int[] tries = new int[Mesh.vertices.Length * 6];

        //two triangles are one tile
        for (int x = 0; x < _dimension; x++)
        {
            for (int z = 0; z < _dimension; z++)
            {
                tries[Index(x, z) * 6 + 0] = Index(x, z);
                tries[Index(x, z) * 6 + 1] = Index(x + 1, z + 1);
                tries[Index(x, z) * 6 + 2] = Index(x + 1, z);
                tries[Index(x, z) * 6 + 3] = Index(x, z);
                tries[Index(x, z) * 6 + 4] = Index(x, z + 1);
                tries[Index(x, z) * 6 + 5] = Index(x + 1, z + 1);
            }
        }

        return tries;
    }

    private Vector2[] GenerateUVs()
    {
        Vector2[] uvs = new Vector2[Mesh.vertices.Length];

        //always set one uv over n tiles than flip the uv and set it again
        for (int x = 0; x <= _dimension; x++)
        {
            for (int z = 0; z <= _dimension; z++)
            {
                Vector2 vec = new Vector2((x / _UVScale) % 2, (z / _UVScale) % 2);
                uvs[Index(x, z)] = new Vector2(vec.x <= 1 ? vec.x : 2 - vec.x, vec.y <= 1 ? vec.y : 2 - vec.y);
            }
        }

        return uvs;
    }

    #endregion

    #region Index

    private int Index(int x, int z)
    {
        return x * (_dimension + 1) + z;
    }

    private int Index(float x, float z)
    {
        return Index((int)x, (int)z);
    }

    #endregion

    private void WaveGeneration()
    {
        float time = Time.time;
        Octave octave = _octaves[0];
        float octaveScaleX = octave.scale.x;
        float octaveScaleY = octave.scale.y;
        float octaveSpeedX = octave.speed.x * time;
        float octaveSpeedY = octave.speed.y * time;
        float octaveHeight = octave.height;

        for (int x = 0; x <= _dimension; x++)
        {
            for (int z = 0; z <= _dimension; z++)
            {
                float y = 0f;
     
                float perl = Mathf.PerlinNoise(
                    (x * octaveScaleX +  octaveSpeedX) / _dimension, 
                    (z * octaveScaleY +  octaveSpeedY) / _dimension) - 0.5f;
                
                y += perl * octaveHeight;

                _vertices[ x * (_dimension + 1) + z] = new Vector3(x, y, z);
            }
        }

        Mesh.SetVertices(_vertices);
        Mesh.RecalculateNormals();
    }
}

[Serializable]
public struct Octave
{
    public Vector2 speed;
    public Vector2 scale;
    public float height;
    public bool alternate;
}