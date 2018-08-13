using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;

public class Receiver : MonoBehaviour {
	
	public UdpClient receiver;
	
	int remotePort = 19784;
	
	string receivedString = "";
	
	float t = 0;
	
	bool received_ = false;
	
	public void Awake(){
		
		t = Time.time;
		
		StartReceivingIP ();
	}
	
	public void StartReceivingIP (){
		try {
			if (receiver == null) {
				receiver = new UdpClient (remotePort);
				receiver.BeginReceive (new AsyncCallback (ReceiveData), null);
			}
		} catch (SocketException e) {
			Debug.Log (e.Message);
		}
	}
	
	void Update(){
		if (received_) {
			t = Time.time;
			received_ = false;
		}
		if (Time.time - t > 1.5f) {//Clean receivedString after 1.5f seconds of inactivity
			receivedString = "";
		}
	}
	
	public void CloseReceiver(){
		if (receiver != null) {
			receiver.Close();
			receiver = null;
		}
		Destroy (gameObject);
	}
	
	private void ReceiveData (IAsyncResult result){
		IPEndPoint receiveIPGroup = new IPEndPoint (IPAddress.Any, remotePort);
		byte[] received;
		if (receiver != null) {
			received = receiver.EndReceive (result, ref receiveIPGroup);
		}else{
			return;
		}
		received_ = true;
		receiver.BeginReceive (new AsyncCallback (ReceiveData), null);
		receivedString = Encoding.ASCII.GetString (received);
		receivedString = receivedString.Trim ();
	}
	
	public string getReceived(){
		return receivedString;
	}
}
