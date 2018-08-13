using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Receiver : MonoBehaviour {
	
    public int port = 49000;
    [Header("Otros")]
    public GameObject startButton;
    public GameObject stopButton;

    private UdpClient receiver;
    private string receivedString = string.Empty;

    private void Awake()
    {
        stopButton.SetActive(false);
        startButton.SetActive(true);
        //InitReceiving();
    }

    public void StartReceiving()
    {
        startButton.SetActive(false);
        stopButton.SetActive(true);
        InitReceiving();
    }

    private void InitReceiving()
    {
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
        receiver = new UdpClient(ipep);
        receiver.BeginReceive(new AsyncCallback(Receive), null);

        //IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        //var data = receiver.Receive(ref sender);
        //print(Encoding.ASCII.GetString(data));
    }

    private void Receive(IAsyncResult asyncResult)
    {
        if(receiver == null)
        {
            return;
        }
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        byte[] received = receiver.EndReceive(asyncResult, ref sender);
        receivedString = Encoding.ASCII.GetString(received);
        print(receivedString);
        receiver.BeginReceive(new AsyncCallback(Receive), null);
    }

    public void StopReceiving()
    {
        stopButton.SetActive(false);
        startButton.SetActive(true);
        receiver.Close();
        receiver = null;
    }

}
