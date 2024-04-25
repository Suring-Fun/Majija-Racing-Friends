using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartCondition : MonoBehaviour
{
    public delegate void LabelChangedHandler(string label, bool isHidden);

    [field: SerializeField]
    public float SuspendTime { get; private set; } = 1f;

    [field: SerializeField]
    public int SecondsToWait { get; private set; } = 3;

    [field: SerializeField]
    public LocalizedString GoString { get; private set; }

    [field: SerializeField]
    public float LabelSuspendTime { get; private set; } = 0.5f;

    public event LabelChangedHandler LabelChanged;

    public event System.Action GameStarted;

    public event System.Action CountDownStarted;

    private void Start()
    {
        IEnumerator Coroutine()
        {
            LabelChanged?.Invoke(null, true);

            Movenment[] cars = FindObjectsOfType<Movenment>();

            foreach (var car in cars)
                car.FreeFly++;

            yield return new WaitForSeconds(SuspendTime);

            CountDownStarted?.Invoke();

            while (SecondsToWait > 0)
            {
                LabelChanged?.Invoke(SecondsToWait.ToString(), false);
                yield return new WaitForSeconds(1);
                SecondsToWait--;
            }

            foreach (var car in cars)
            {
                car.FreeFly--;
                car.GetComponent<SafeEffect>().RunSafeEffect();
            }

            GameStarted?.Invoke();
            LabelChanged?.Invoke(GoString, false);

            yield return new WaitForSeconds(LabelSuspendTime);

            LabelChanged?.Invoke(null, true);
        }

        StartCoroutine(Coroutine());
    }
}
