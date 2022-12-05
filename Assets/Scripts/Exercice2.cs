using System.Threading.Tasks;
using System.Threading;
using UnityEngine;


public class Exercice2 : MonoBehaviour
{
    [SerializeField] private bool isActive;

    private void Awake()
    {
        if (!isActive)
        {
            this.enabled = false;
        }
    }

    private async void Start()
    {
        using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource())
        {

            CancellationToken ct = cancellationTokenSource.Token;

            Task task1 = Task_1(ct);
            Task task2 = Task_2(ct);

            bool res = await WhatTaskFasterAsync(ct, task1, task2);
            cancellationTokenSource.Cancel();

            Debug.Log(res);
            
        }

    }

    private async Task<bool> WhatTaskFasterAsync(CancellationToken ct, Task task1, Task task2)
    {
        int task1Hash = task1.GetHashCode();
        int task2Hash = task2.GetHashCode();

        Debug.Log($"Task1Hash {task1Hash}\nTask2Hash {task2Hash}");

        Task res = await Task.WhenAny(task1, task2);
        Debug.Log(res.GetHashCode());

        if (res.GetHashCode() == task1.GetHashCode())
            return true;
        else
            return false;
    }

    private async Task Task_1(CancellationToken ct)
    {
        await Task.Delay(1000);
        Debug.Log("Delay 1s");
    }

    private async Task Task_2(CancellationToken ct)
    {
        int count = 0;
        while(count < 60)
        {
            if(ct.IsCancellationRequested)
                return;
            count++;
            await Task.Yield();
        }
        Debug.Log("Delay 60 frame");
    }


}
