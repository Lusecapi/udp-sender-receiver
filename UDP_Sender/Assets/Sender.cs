using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine.UI;

public class Sender : MonoBehaviour {

    [Range(1, 60)]
    public float packagePerSecond = 20;
    public int remotePort = 49000;
    public string remoteIP = "127.0.0.1";//"192.68.0.6";
    [Header("Otros")]
    public GameObject mainPanel;
    public GameObject closeButton;
    public InputField ifPPS;
    public InputField ifPort;
    public InputField ifIP;


    UdpClient sender;
    private readonly int localPort = 48999;//Esta variable no es configurable, el x plane la setea internamente;s

    private float sendRate;
    private int randomNum;
    private bool isSending;

    private void Awake()
    {
        mainPanel.SetActive(true);
        closeButton.SetActive(false);
        //InitSender();
    }

    private void Update()
    {
        if(isSending)
            randomNum = Random.Range(0, 100);
    }

    public void StartSending()
    {
        isSending = true;
        mainPanel.SetActive(false);
        closeButton.SetActive(true);

        packagePerSecond = int.Parse(ifPPS.text);
        remotePort = int.Parse(ifPort.text);
        remoteIP = ifIP.text;

        InitSender();

        InvokeRepeating("Send", 0, sendRate);
    }

    private void InitSender()
    {
        sendRate = (1000 / packagePerSecond) / 1000;

        sender = new UdpClient(localPort, AddressFamily.InterNetwork);
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(remoteIP), remotePort);
        sender.Connect(endPoint);
    }

    private void Send()
    {
        if (sender == null)
        {
            CancelInvoke("Send");
            return;
        }
        //print(Mathf.RoundToInt(Time.time));//Para verificar que si se esta mandando con la frecuancia dada
        print(randomNum);
        if (sender == null)
        {
            Debug.LogError("No Sender");
            return;
        }

        string customMessage = string.Format("randomRum = {0}", randomNum);
        sender.Send(Encoding.ASCII.GetBytes(customMessage), customMessage.Length);
        print("send");
    }

    public void StopSending()
    {
        isSending = false;
        closeButton.SetActive(false);
        mainPanel.SetActive(true);
        sender.Close();
        sender = null;
    }

    //public void CloseSender(){
    //	if (sender != null) {
    //		sender.Close();
    //		sender = null;
    //	}
    //	Destroy (gameObject);
    //}
}