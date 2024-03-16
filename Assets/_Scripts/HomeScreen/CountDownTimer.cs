using System.Collections;
using TMPro;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    public static CountDownTimer instance;
    
    [SerializeField] private TMP_Text timer;
    
    private const int TimeLeft = 45;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void StartCountDownTimer()
    {
        StopAllCoroutines();
        StartCoroutine(CountDown(TimeLeft));
    }

    public int CheckTimeLeft()
    {
        StopAllCoroutines();
        return int.Parse(timer.text);
    }
    
    private IEnumerator CountDown(int deltaTime)
    {
        while (deltaTime >= 0)
        {
            timer.text = deltaTime.ToString();
            yield return new WaitForSecondsRealtime(1);
            deltaTime -= 1;
        }
        
        GameManager.instance.ChangeState(GameState.Lose);
    }
}
