using UnityEngine;

public class UISFX : MonoBehaviour
{
    public void ClickSound()
    {
        AudioManager.instance.SonidoClickUI();
    }

    public void HoverSound()
    {
        AudioManager.instance.SonidoHoverUI();
    }
}
