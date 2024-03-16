using UnityEngine;

public class SelecLevelUI : MonoBehaviour
{
    public static SelecLevelUI instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void OnButtonBackClicked()
    {
        MySceneManager.instance.MyLoadScene("SplashScreen");
    }

    public void Show()
    {
        gameObject.SetActive(true);
        LevelManager.instance.Spawn();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        LevelManager.instance.Despawn();
    }
}
