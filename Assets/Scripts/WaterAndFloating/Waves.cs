using System;
using System.Collections.Generic;
using System.Linq;
using Singleton;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Waves : MonoBehaviour
{
    //Properties
    [SerializeField] private int _dimension = 10;
    [SerializeField] private float _UVScale = 2f;
    [SerializeField] private Transform _waterPosition;
    [SerializeField] private Octave _octave;

    [Header("VFX"), SerializeField] private ParticleSystem _waveBurstParticlePrefab;

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

        //index wave values setup
        _verticeCount = _vertices.Count;
        Vector3 pos = _waterPosition.position;
        _positionDifference = new Vector2(pos.x, pos.z);
        Vector3 scale = _waterPosition.localScale;
        _scaleDifference = new Vector2(scale.x, scale.z);
        for (int i = 0; i < _vertices.Count; i++)
        {
            _indexVerticesDictionary.Add(new Vector2(_vertices[i].x,_vertices[i].z),i);
        }
    }

    private void Update()
    {
        WaveGeneration();
        ManageCircularWavesTimer();
    }

    public float GetHeight(Vector3 position)
    {
        //scale factor and position in local space
        Vector3 scale = new Vector3(1 / transform.lossyScale.x, 0, 1 / transform.lossyScale.z);
        Vector3 localPos = Vector3.Scale((position - transform.position), scale);

        //get edge points
        Vector3 p1 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Floor(localPos.z));
        Vector3 p2 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Ceil(localPos.z));
        Vector3 p3 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Floor(localPos.z));
        Vector3 p4 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Ceil(localPos.z));

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
        float max = Mathf.Max(Vector3.Distance(p1, localPos), Vector3.Distance(p2, localPos),
            Vector3.Distance(p3, localPos), Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        float dist = (max - Vector3.Distance(p1, localPos))
                   + (max - Vector3.Distance(p2, localPos))
                   + (max - Vector3.Distance(p3, localPos))
                   + (max - Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        //weighted sum


        float height = _vertices[Index(p1.x, p1.z)].y * (max - Vector3.Distance(p1, localPos))
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
        Octave octave = _octave;
        float octaveScaleX = octave.Scale.x;
        float octaveScaleY = octave.Scale.y;
        float octaveSpeedX = octave.Speed.x * time;
        float octaveSpeedY = octave.Speed.y * time;
        float octaveHeight = octave.Height;

        for (int x = 0; x <= _dimension; x++)
        {
            for (int z = 0; z <= _dimension; z++)
            {
                float y = 0f;

                if (octave.Height == 0)
                {
                    _vertices[ x * (_dimension + 1) + z] = new Vector3(x, y, z);
                    continue;
                }
                
                float perlinNoiseValue = Mathf.PerlinNoise(
                    (x * octaveScaleX +  octaveSpeedX) / _dimension, 
                    (z * octaveScaleY +  octaveSpeedY) / _dimension) 
                                         - 0.5f;
                
                y += perlinNoiseValue * octaveHeight;

                _vertices[ x * (_dimension + 1) + z] = new Vector3(x, y, z);
            }
        }
        
        ManageCircularWaves();
        ManageVerticesGrowingAmplitude();
        ManageVerticesReducingAmplitude();

        Mesh.SetVertices(_vertices);
        Mesh.RecalculateNormals();
    }

    #region CircularWaves

    private List<CircularWave> _circularWavesList = new List<CircularWave>();
    private List<float> _circularWavesDurationList = new List<float>();
<<<<<<< HEAD
    public List<float> CircularWavesDurationList => _circularWavesDurationList;
=======
    private int _verticeCount;
    private Vector2 _positionDifference;
    private Vector2 _scaleDifference;
    private Dictionary<Vector2, int> _indexVerticesDictionary = new Dictionary<Vector2, int>();
>>>>>>> 89466ff0f0b8a2dfb7a96585b54385e20cfaa398

    public void LaunchCircularWave(CircularWave circularWave)
    {
        _circularWavesList.Add(circularWave);
        _circularWavesDurationList.Add(circularWave.Duration);
    }
    
    private void ManageCircularWaves()
    {
        for (int i = 0; i < _circularWavesList.Count; i++)
        {
            //calculate the values
            CircularWave waveData = _circularWavesList[i];
            Vector3 center = new Vector3(waveData.Center.x, 0, waveData.Center.y);
            float currentTime = _circularWavesDurationList[i];
            float percent = currentTime / waveData.Duration;
            float distance = (1 - percent) * waveData.Distance;
            float amplitude = percent * waveData.Amplitude;

            //set vertex
            float angleDifference = 360 / (float)waveData.NumberOfPoints;
            for (int j = 1; j <= waveData.NumberOfPoints; j++)
            {
                float angle = j * angleDifference;
                Vector3 point = GetPointFromAngleAndDistance(center, angle, distance);
                int index = FindIndexOfVerticeAt(new Vector2(point.x,point.z), true);
                SetupVerticesAmplitudeDictionary(new Vector2(_vertices[index].x,_vertices[index].z), amplitude);

                if (j % 3 == 0)
                {
                    Instantiate(_waveBurstParticlePrefab, point+new Vector3(0,amplitude/2,0), Quaternion.identity);
                }
            }
        }
    }

    private void ManageCircularWavesTimer()
    {
        for (int i = 0; i < _circularWavesDurationList.Count; i++)
        {
            _circularWavesDurationList[i] -= Time.deltaTime;
            
            if (_circularWavesDurationList[i] <= 0)
            {
                _circularWavesList.Remove(_circularWavesList[i]);
                _circularWavesDurationList.Remove(_circularWavesDurationList[i]);
            }
        }
    }
    
    private int FindIndexOfVerticeAt(Vector2 worldPosition, bool applyWaveTransform)
    {
        Vector2 rounded = new Vector2(Mathf.Round(worldPosition.x), Mathf.Round(worldPosition.y));
        if (applyWaveTransform)
        {
            rounded = new Vector2(Mathf.Round((worldPosition.x-_positionDifference.x)/_scaleDifference.x), 
                                  Mathf.Round((worldPosition.y-_positionDifference.y)/_scaleDifference.y));
        }
        int closestIndex = _indexVerticesDictionary[rounded];
        
        return closestIndex;
    }
    
    public static Vector3 GetPointFromAngleAndDistance(Vector3 startingPoint, float yAngleDegrees, float distance)
    {
        // Convert the Y angle to radians
        float yAngleRadians = yAngleDegrees * Mathf.Deg2Rad;

        // Calculate the X and Z offsets using trigonometry
        float xOffset = distance * Mathf.Sin(yAngleRadians);
        float zOffset = distance * Mathf.Cos(yAngleRadians);

        // Create a new Vector3 with the calculated offsets and the same Y position as the starting point
        Vector3 newPoint = new Vector3(startingPoint.x + xOffset, startingPoint.y, startingPoint.z + zOffset);

        return newPoint;
    }

    #endregion

    #region Amplitude
    
    private Dictionary<Vector2, float> _verticesAmplitudeGrowingDictionary = new Dictionary<Vector2, float>();
    private Dictionary<Vector2, float> _verticesAmplitudeCurrentGrowingDictionary = new Dictionary<Vector2, float>();
    
    private Dictionary<Vector2, float> _verticesAmplitudeReducingDictionary = new Dictionary<Vector2, float>();

    private void SetupVerticesAmplitudeDictionary(Vector2 vertice, float amplitude)
    {
        Vector2 coordinate = new Vector2(vertice.x, vertice.y);
        if (_verticesAmplitudeGrowingDictionary.ContainsKey(coordinate) == false &&
            _verticesAmplitudeCurrentGrowingDictionary.ContainsKey(coordinate) == false)
        {
            _verticesAmplitudeGrowingDictionary.Add(coordinate,amplitude);
            _verticesAmplitudeCurrentGrowingDictionary.Add(coordinate,0);
        }
    }

    private void ManageVerticesGrowingAmplitude()
    {
        //create lists from dictionaries
        List<KeyValuePair<Vector2, float>> amplitudeList =        _verticesAmplitudeGrowingDictionary.ToList();
        List<KeyValuePair<Vector2, float>> currentAmplitudeList = _verticesAmplitudeCurrentGrowingDictionary.ToList();
        
        //apply amplitude to vertices
        for(int i = 0; i < currentAmplitudeList.Count; i++)
        {
            int index = FindIndexOfVerticeAt(currentAmplitudeList[i].Key, false);
            
            float amplitude = Mathf.Lerp(currentAmplitudeList[i].Value, amplitudeList[i].Value, 0.2f);
            _vertices[index] = new Vector3(_vertices[index].x, amplitude, _vertices[index].z);
            
            currentAmplitudeList[i] = new KeyValuePair<Vector2, float>(currentAmplitudeList[i].Key, amplitude);
        }

        //check for vertices on max amplitude 
        for (int i = 0; i < currentAmplitudeList.Count; i++)
        {
            if (currentAmplitudeList[i].Value >= amplitudeList[i].Value - 0.3f)
            {
                Vector2 key = new Vector2(currentAmplitudeList[i].Key.x, currentAmplitudeList[i].Key.y);
                if (_verticesAmplitudeReducingDictionary.ContainsKey(key) == false)
                {
                    _verticesAmplitudeReducingDictionary.Add(key,amplitudeList[i].Value);
                } 
                
                currentAmplitudeList.Remove(currentAmplitudeList[i]);
                amplitudeList.Remove(amplitudeList[i]);
            }
        }
        
        //apply list to dictionaries
        _verticesAmplitudeGrowingDictionary = amplitudeList.ToDictionary(pair => pair.Key, pair => pair.Value);
        _verticesAmplitudeCurrentGrowingDictionary = currentAmplitudeList.ToDictionary(pair => pair.Key, pair => pair.Value);
    }
    
    private void ManageVerticesReducingAmplitude() 
    {
        //create lists from dictionaries
        List<KeyValuePair<Vector2, float>> amplitudeList = _verticesAmplitudeReducingDictionary.ToList();

        //apply amplitude to vertices
        for(int i = 0; i < amplitudeList.Count; i++)
        {
            int index = FindIndexOfVerticeAt(amplitudeList[i].Key, false);
            
            float amplitude = Mathf.Lerp(amplitudeList[i].Value, _vertices[index].y, 0.2f);
            _vertices[index] = new Vector3(_vertices[index].x, amplitude, _vertices[index].z);
            
            amplitudeList[i] = new KeyValuePair<Vector2, float>(amplitudeList[i].Key, amplitude);
        }

        //check for vertices below 0
        for (int i = 0; i < amplitudeList.Count; i++)
        {
            if (amplitudeList[i].Value <= 0.2f)
            {
                amplitudeList.Remove(amplitudeList[i]);
            }
        }
        
        //apply list to dictionaries
        _verticesAmplitudeReducingDictionary = amplitudeList.ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    #endregion

    #region GUI
    
    #if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (KeyValuePair<Vector2,float> vertice in _verticesAmplitudeGrowingDictionary)
        {
            Gizmos.DrawSphere(new Vector3(vertice.Key.x*_scaleDifference.x+_positionDifference.x,0,vertice.Key.y*_scaleDifference.y+_positionDifference.y), 0.5f);
        }
        Gizmos.color = Color.green;
        foreach (KeyValuePair<Vector2,float> vertice in _verticesAmplitudeReducingDictionary)
        {
            Gizmos.DrawSphere(new Vector3(vertice.Key.x*_scaleDifference.x+_positionDifference.x,0,vertice.Key.y*_scaleDifference.y+_positionDifference.y), 0.5f);
        }
    }

#endif

    #endregion
}

[Serializable]
public struct CircularWave
{
    [Tooltip("The center of the wave, the point where it start")]
    public Vector2 Center { get; set; }

    [Tooltip("The duration of the wave in seconds")]
    public float Duration;

    [Tooltip("The height of the waves")]
    public float Amplitude;
    public float CurrentAmplitude { get; set; }

    [Tooltip("The distance it runs")]
    public float Distance;

    [Tooltip("Number of circular vertices points the wave will manage")]
    public int NumberOfPoints;
}

[Serializable]
public struct Octave
{
    public Vector2 Speed;
    public Vector2 Scale;
    public float Height;
}