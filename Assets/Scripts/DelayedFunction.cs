using System.Threading.Tasks;
using UnityEngine;

public class DelayedFunction
{
    private System.Action function;

    public DelayedFunction(System.Action function, int delayInMs)
    {
        this.function = function;
        DoFunction(delayInMs);
    }

    private async void DoFunction(int delayInMs)
    {
        await Task.Delay(delayInMs);
        function();
    }
}
