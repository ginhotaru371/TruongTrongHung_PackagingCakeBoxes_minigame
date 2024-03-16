using UnityEngine;

public class LoseUI : MonoBehaviour
{
    public static LoseUI instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
