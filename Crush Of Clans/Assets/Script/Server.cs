using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
public class Server : MonoBehaviour {
	static public string serverPORT="25565";
	static Thread th_Listen;
	static public string serverIP="107.167.178.99";//"107.167.178.99";
	static string findThisString = "@@@@@";//chkstring
	static int chkCommand = 0;//chk value != -1 is OK
	static public bool ConnectToServer() {
		IPEndPoint ServerEP = new IPEndPoint(IPAddress.Parse(serverIP), int.Parse(serverPORT));
		State.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		try
		{
			State.clientSocket.Connect(ServerEP);
			th_Listen = new Thread(new ThreadStart(Listen));
			th_Listen.Start();
			Thread.Sleep(200);
			print("Already connect to the Server!");
			return true;
			
		}
		catch (Exception ex) {
			
			print("Can't connect to the Server!");
			return false;
		}
	}
	static private void Listen() {
		EndPoint ClientEP = State.clientSocket.RemoteEndPoint;
		//接收要用的 Byte Array
		Byte[] byteLoad = new byte[1024];
		int loadLen;
		
		String strAll;    //接收到的完整訊息strAll=strCase+strInfo
		String strCase;  //命令碼: 00 ~ 99 (前兩碼)
		String strInfo;     //真正傳達的訊息
		while(State.chkThread)
		{
			try
			{
				
				loadLen = State.clientSocket.ReceiveFrom(byteLoad, 0, byteLoad.Length, SocketFlags.None, ref ClientEP);
				strAll = Encoding.UTF8.GetString (byteLoad, 0, loadLen);
				if (loadLen != 0 && strAll.IndexOf(findThisString) != -1) {
					
					string[] control = strAll.Split(new string[]{"@@@@@"}, StringSplitOptions.RemoveEmptyEntries);
					print (strAll);
					foreach (var item in control) {
						strCase = item.Substring(0, 2);
						switch (strCase) {
						case "21"://Server Send 01PlayID
							State.name = item.Substring(2);
							
							break;
							
						case "10"://If Client Login Success 
							strInfo = item.Substring(2);
							if (strInfo[0] == '0'){
								print("Login Success!!");
								State.LoginSuecess=true;
							}
							else{
								State.LoginSuecess=false;
								
								//failed
							}
							
							break;
						case "20":
							strInfo = item.Substring(2);
							string[] com20= strInfo.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
							State.name=com20[1];
							break;
						case "11"://If Client Login Failed
							
							print("Login Error!!");
							break;
						case "22":
							strInfo=item.Substring(2);
							string[] strxy=strInfo.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
							print (strxy[0]);
							print (strxy[1]);
							State.PosX=int.Parse(strxy[0]);
							State.PosZ=int.Parse(strxy[1]);
							break;
						case "23":
							strInfo=item.Substring(2);
							//string[] strsource= strInfo.Split(new string[] { "@@@@@" }, StringSplitOptions.RemoveEmptyEntries);
							string[] strsources=strInfo.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
							//infomationText(strsources[1]);
							
							print (strsources[1]);
							State.source[int.Parse (strsources[0])-1]=int.Parse (strsources[1]);//0 wood, 1 stone ,2 metal;
							break;
						case "er"://If Client Send Message length < 2 , Server can send "er"
							print ("command Error!!");
							break;
							
						default:
							break;
						}
					}

				}
				
			}
			catch (Exception ex)//產生錯誤時
			{
				print (ex);
				State.clientSocket.Close();//關閉通訊器
				
				print("伺服器斷線了！");//顯示斷線
				
				th_Listen.Abort();//刪除執行緒
			}
		} 
	}
	static public void Send(string strSend)
	{
		byte[] byteSend = new byte[1024];
		
		try
		{
			byteSend = Encoding.UTF8.GetBytes(strSend);
			State.clientSocket.Send (byteSend);  
		}
		catch (Exception ex)
		{
			
			print(" Connection Break!");
		}
	}
}
