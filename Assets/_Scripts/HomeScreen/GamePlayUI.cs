using UnityEngine;

public class GamePlayUI : MonoBehaviour
{
    public static GamePlayUI instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        GameManager.instance.Despawn();
    }
}
