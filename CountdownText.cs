using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))] //Depends on TextComponent
public class CountdownText : MonoBehaviour
{
    public delegate void CountdownFinished();
    public static event CountdownFinished OnCountdownFinished;

    Text countdown;

    void OnEnable() //OnEnable() Gets called everytime we set this page to Active
    {
        countdown = GetComponent<Text>();
        countdown.text = "3";
        StartCoroutine("Countdown"); //Coroutine is IEnumberable Countdown()
    }

    IEnumerator Countdown()
    {
        int count = 3;
        for (int i = 0; i < count; i++)
        {
            countdown.text = (count - i).ToString();
            yield return new WaitForSeconds(1);
        }
        OnCountdownFinished();
    }
}
