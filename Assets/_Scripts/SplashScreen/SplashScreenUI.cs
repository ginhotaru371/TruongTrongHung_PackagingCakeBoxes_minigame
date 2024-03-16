using UnityEngine;

public class SplashScreenUI : MonoBehaviour
{
    public void OnButtonPlayClicked()
    {
        MySceneManager.instance.MyLoadScene("HomeScreen");
    }
}
