using TMPro;
using UnityEngine;

public enum Angle_Camera
{
    menu = 0,
    white = 1,
    black = 2
}


public class GameUI : MonoBehaviour
{
   public static GameUI Instance { set; get; }

    public Server server;
    public Client client;

    [SerializeField] private Animator menuAnimation;
    [SerializeField] private TMP_InputField addressinput;
    [SerializeField] private GameObject[] Anglesofcamera;

    private void Awake()
    {
        Instance = this;

    }

    public void OnLocalButton()
    {
        menuAnimation.SetTrigger("InGameMenu");
        server.Init(38000);
        client.Init("127.0.0.1", 38000);
    }
    public void OnMultiplayerButton()
    {
        menuAnimation.SetTrigger("OnlineMenu");
    }

    public void OnOnlineHostButton()
    {
        server.Init(38000);
        client.Init("127.0.0.1", 38000);
        menuAnimation.SetTrigger("HostMenu");
    }
    public void OnConnectButton()
    {
        client.Init(addressinput.text, 38000);
    }
    public void OnBackButton()
    {
        menuAnimation.SetTrigger("StartMenu");
    }

    public void OnHostBackButton()
    {
        server.Shutdown();
        client.Shutdown();
        menuAnimation.SetTrigger("OnlineMenu");
    }

}
