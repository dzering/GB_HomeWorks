using Unity.Jobs;
using Unity.Collections;
using UnityEngine;
using System.Collections;


namespace Assets.Scripts
{
    public class Lesson2 : MonoBehaviour
    {
        private NativeArray<int> _arr;

        private void Start()
        {
            _arr = new NativeArray<int>(
                 new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9,10,11,12,13,14 },
                 Allocator.Persistent);

            MyJob myJob = new MyJob();
            myJob.Array = _arr;

            JobHandle jobHandle = myJob.Schedule();
            jobHandle.Complete();

            Print(_arr);
        }

        private void OnDestroy()
        {
            _arr.Dispose();
        }

        private void Print(IEnumerable objects)
        {
            foreach (var item in objects)
            {
                Debug.Log(item);
            }
        }
    }

    public struct MyJob : IJob
    {
        public NativeArray<int> Array;
        public void Execute()
        {
            if(Array.Length <= 10)
            {
                return;
            }
            for (int i = 10; i < Array.Length; i++)
            {
                Array[i] = 0;
            }
        }
    }
}
