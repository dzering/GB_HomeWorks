using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Jobs;

public class Galaxy : MonoBehaviour
{
    [SerializeField] private int _numberOfEntities;
    [SerializeField] private GameObject _celestialBodyPrefab;

    [Header("Starting parameters")]
    [SerializeField] private float _startDistance;
    [SerializeField] private float _startVelocity;
    [SerializeField] private float _startMass;

    [SerializeField] private float _gravityModificator;

    private TransformAccessArray _transformAccessArray;

    private NativeArray<Vector3> _positions;
    private NativeArray<Vector3> _velocities;
    private NativeArray<Vector3> _accelerations;
    private NativeArray<float> _masses;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        GravityJob gravityJob = new GravityJob()
        {
            Positions = _positions,
            Velocities = _velocities,
            Accelerations = _accelerations,
            Masses = _masses,
            GravityModificator = _gravityModificator,
            DeltaTime = Time.deltaTime
        };

        JobHandle gravitationHandle = gravityJob.Schedule(_numberOfEntities, 0);

        MoveJob moveJob = new MoveJob()
        {
            Accelerations = _accelerations,
            Positions = _positions,
            Velocities = _velocities,
            DeltaTime = Time.deltaTime
        };

        JobHandle moveHandle = moveJob.Schedule(_transformAccessArray, gravitationHandle);
        moveHandle.Complete();
    }

    private void Init()
    {
        Transform[] transforms = new Transform[_numberOfEntities];

        _positions = new NativeArray<Vector3>(_numberOfEntities,Allocator.Persistent);
        _velocities = new NativeArray<Vector3>(_numberOfEntities, Allocator.Persistent);
        _accelerations = new NativeArray<Vector3>(_numberOfEntities, Allocator.Persistent);
        _masses = new NativeArray<float>(_numberOfEntities, Allocator.Persistent);

        for (int i = 0; i < _numberOfEntities; i++)
        {
            _positions[i] = RandomVector(_startDistance);
            _velocities[i] = RandomVector(_startVelocity);
            _accelerations[i] = new Vector3();
            _masses[i] = Random.Range(0,_startMass);

            GameObject gameObject = Instantiate(_celestialBodyPrefab);
            gameObject.GetComponent<Renderer>().material.color = RandomColor();
            Transform transform = gameObject.transform;


            transform.localScale = Vector3.one * _masses[i];
            transform.position = _positions[i];
            transforms[i] = transform;
        }
        _transformAccessArray = new TransformAccessArray(transforms);
    }

    private Color RandomColor()
    {
        return Random.ColorHSV();
    }

    private Vector3 RandomVector(float maxValue)
    {
        Vector3 vector = Random.insideUnitSphere * Random.Range(0, maxValue);
        return vector;
    }
    private void OnDestroy()
    {
        _positions.Dispose();
        _velocities.Dispose();
        _accelerations.Dispose();
        _masses.Dispose();
        _transformAccessArray.Dispose();
    }
}

public struct MoveJob : IJobParallelForTransform
{
    public NativeArray<Vector3> Positions;
    public NativeArray<Vector3> Accelerations;
    public NativeArray<Vector3> Velocities;
    [ReadOnly]
    public float DeltaTime;
    public void Execute(int index, TransformAccess transform)
    {
        Vector3 velocity = Accelerations[index] + Velocities[index];
        transform.position += velocity * DeltaTime;

        Positions[index] = transform.position;
        Velocities[index] = velocity;
        Accelerations[index] = Vector3.zero;
    }
}

public struct GravityJob : IJobParallelFor
{
    [ReadOnly]
    public NativeArray<Vector3> Positions;
    [ReadOnly]
    public NativeArray<Vector3> Velocities;
    public NativeArray<Vector3> Accelerations;
    [ReadOnly]
    public NativeArray<float> Masses;
    [ReadOnly]
    public float GravityModificator;
    public float DeltaTime;

    public void Execute(int index)
    {
        for (int i = 0; i < Positions.Length; i++)
        {
            if (i == index) continue;

            float distance = Vector3.Distance(Positions[i], Positions[index]);
            Vector3 direction = Positions[i] - Positions[index];
            Vector3 gravity = (direction * Masses[i] * GravityModificator) /
                (Masses[index] * Mathf.Pow(distance, 2));
            Accelerations[index] += gravity * DeltaTime;

        }
    }
}
