using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine;

public class Lesson2Task2 : MonoBehaviour
{
    private NativeArray<Vector3> _positions;
    private NativeArray<Vector3> _velocities;
    private NativeArray<Vector3> _finalPosition;

    private void Start()
    {
        _positions = new NativeArray<Vector3>(100, Allocator.Persistent);
        _velocities = new NativeArray<Vector3>(100, Allocator.Persistent);
        _finalPosition = new NativeArray<Vector3>(100, Allocator.Persistent);

    MyJob myJob = new MyJob();
        myJob.Positions = _positions;
        myJob.Velocities = _velocities;
        myJob.FinalPositions = _finalPosition;

        JobHandle jobHandle = myJob.Schedule(_positions.Length, 0);
        jobHandle.Complete();

        foreach (var item in _finalPosition)
        {
            Debug.Log(item);
        }
    }

    private void OnDestroy()
    {
        _positions.Dispose();
        _velocities.Dispose();
        _finalPosition.Dispose();
    }
}

public struct MyJob : IJobParallelFor
{
    [ReadOnly]
    public NativeArray<Vector3> Positions;
    [ReadOnly]
    public NativeArray<Vector3> Velocities;
    [WriteOnly]
    public NativeArray<Vector3> FinalPositions;

    public void Execute(int index)
    {
        FinalPositions[index] = Positions[index] + Velocities[index];
    }
}
