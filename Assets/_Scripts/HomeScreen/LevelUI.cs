using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    public static LevelUI instance;

    private Level _level;

    [SerializeField] private Sprite starWin;
    [SerializeField] private Sprite starLose;

    [SerializeField] private Image star1, star2, star3;
    [SerializeField] private Image levelLock;

    [SerializeField] private TMP_Text levelNumber;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void OnClicked()
    {
        SelecLevelUI.instance.Hide();
        GamePlayUI.instance.Show();
        GameManager.instance.GenerateGrid(_level);
    }

    public void SetLevelDetail(Level level)
    {
        _level = level;
        CheckLevel(level.index);
        CheckStat(level.stat);
        CheckStars(level.stars);
    }

    private void CheckStars(int star)
    {
        switch (star)
        {
            case 0:
                star1.sprite = starLose;
                star2.sprite = starLose;
                star3.sprite = starLose;
                break;
            case 1:
                star1.sprite = starWin;
                break;
            case 2:
                star1.sprite = starWin;
                star2.sprite = starWin;
                break;
            case 3:
                star1.sprite = starWin;
                star2.sprite = starWin;
                star3.sprite = starWin;
                break;
        }
    }

    private void CheckStat(bool stat)
    {
        if (stat)
        {
            levelLock.gameObject.SetActive(false);
            levelNumber.gameObject.SetActive(true);
            this.gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            levelLock.gameObject.SetActive(true);
            levelNumber.gameObject.SetActive(false);
            this.gameObject.GetComponent<Button>().interactable = false;
        }
    }

    private void CheckLevel(int index) {
        levelNumber.text = index.ToString();
    }
}
