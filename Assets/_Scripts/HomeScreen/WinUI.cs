using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    public static WinUI instance;

    [SerializeField] private Sprite starWin;
    [SerializeField] private Sprite starLose;

    [SerializeField] private Image star1, star2, star3;

    private void Awake()
    {
        if (instance == null) instance = this;
        Hide();
    }

    public int OnLevelCompleted(int timeLeft)
    {
        var stars = CheckStars(timeLeft);
        Show();

        return stars;
    }

    private int CheckStars(int timeLeft)
    {
        switch (timeLeft)
        {
            case >= 31:
                star1.sprite = starWin;
                star2.sprite = starWin;
                star3.sprite = starWin;

                return 3;
            case >= 16:
                star1.sprite = starWin;
                star2.sprite = starWin;

                return 2;
            case < 16:
                star1.sprite = starWin;

                return 1;
        }
    }

    private void ClearStars()
    {
        star1.sprite = starLose;
        star2.sprite = starLose;
        star3.sprite = starLose;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        ClearStars();
        gameObject.SetActive(false);
    }
}
