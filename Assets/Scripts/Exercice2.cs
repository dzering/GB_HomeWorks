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

    private void Start()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken ct = cancellationTokenSource.Token;

        Task_1(ct);
        Task_2(ct);
    }


    private async void Task_1(CancellationToken ct)
    {
        await Task.Delay(1000);
        Debug.Log("Delay 1s");
    }

    private async void Task_2(CancellationToken ct)
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
