using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class NetworkManagerTempUI : MonoBehaviour
{
    [SerializeField] Button HostBtn, JoinBtn;
    [SerializeField] GameObject panel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        HostBtn.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartHost())
            {
                Debug.Log("Host started");
                panel.SetActive(false);
            }
            else
            {
                Debug.Log("Failed to start host");
            }
        });

        JoinBtn.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartClient())
            {
                Debug.Log("Client started");
                panel.SetActive(false);
            }
            else
            {
                Debug.Log("Failed to start client");
            }
        });
    }

    
}
