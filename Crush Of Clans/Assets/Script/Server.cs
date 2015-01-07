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
	static public Thread th_Listen;
	static public string serverIP="107.167.178.99";//"107.167.178.99";
	static string findThisString = "@@@@@";//chkstring
	static int chkCommand = 0;//chk value != -1 is OK

	static public void initial(){
		foreach (string HouseIDKey in State.HouseID.Keys) {
			State.HouseStatus[HouseIDKey]=false;
		}
		foreach (string PlayerIDKey in State.PlayerID.Keys) {
			State.PlayerStatus[PlayerIDKey]=false;
			
		}
		State.mainPlayerStatus=false;

	}
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
			State.isConnect=true;
			//State.chkThread=true;
			initial();
			return true;
			
		}
		catch (Exception ex) {
			State.isConnect=false;
			
			print("Can't connect to the Server!");
			return false;
		}
	}
	static private void Listen() {
		EndPoint ClientEP = State.clientSocket.RemoteEndPoint;
		//接收要用的 Byte Array
		Byte[] byteLoad = new byte[4069];
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
					print(strAll);
					
				//	print (strAll);
					foreach (var item in control) {
						
						strCase = item.Substring(0, 2);
					
						switch (strCase) {
						case "21"://Server Send 01PlayID
							State.PlayerName = item.Substring(2);
							print ("21Player:"+State.PlayerName);
							
							break;
							
						case "10"://If Client Login Success 
							strInfo = item.Substring(2);
							if (strInfo[0] == '0'){

								print("Login Success!!");
								State.LoginSuecess=true;
							}
							else{
								State.LoginSuecess=false;
								print (strInfo[0].ToString());
								//failed
							}
							
							break;
						case "20":
							strInfo = item.Substring(2);
							string[] com20= strInfo.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
							State.PlayerName=com20[1];
							break;
						case "11"://If Client Login Failed
							
							print("Login Error!!");
							break;
						case "22":
							strInfo=item.Substring(2);
							print ("22Player:"+strInfo);
							
							string[] strxy=strInfo.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
							State.PosX=int.Parse(strxy[0]);
							State.PosZ=int.Parse(strxy[1]);
							break;
						case "23":
							strInfo=item.Substring(2);
							print ("23Player:"+strInfo);
							
							string[] strsources=strInfo.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
							
							State.source[int.Parse (strsources[0])-1]=int.Parse (strsources[1]);//0 wood, 1 stone ,2 metal;
							break;
						case "31":
							strInfo=item.Substring(2);
							string[] strPlayers=strInfo.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
							if(strPlayers.Length<3&& strPlayers[2]!="" &&strPlayers[2]!=null){
								continue;
							}
							if(!State.PlayerID.ContainsKey(strPlayers[0])){
							
								State.PlayerID.Add(strPlayers[0],strPlayers[0]);
								State.PlayerPositionX.Add(strPlayers[0],int.Parse(strPlayers[1]));
								State.PlayerPositionZ.Add(strPlayers[0],int.Parse(strPlayers[2]));
								State.PlayerStatus.Add(strPlayers[0],false);
								State.PlayerUpdate.Add (strPlayers[0],5);
								if(!State.AnotherPlayerList.ContainsKey(strPlayers[0])){
									State.AnotherPlayerList.Add (strPlayers[0],strPlayers[0]);
								}
							}else{
								State.PlayerPositionX[strPlayers[0]]=int.Parse(strPlayers[1]);
								State.PlayerPositionZ[strPlayers[0]]=int.Parse(strPlayers[2]);
								State.PlayerUpdate[strPlayers[0]]=5;
								
							}
							break;
						case "32":
							print ("House:"+item);
							
							strInfo=item.Substring(2);
							string[] strHouses=strInfo.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
							if(strHouses.Length<9 && strHouses[8]!="" &&strHouses[8]!=null){
								continue;
							}
							print ("House"+strInfo);
							if(!State.HouseID.ContainsKey(strHouses[0])){

								State.HouseID.Add(strHouses[0],strHouses[0]);
								State.HouseKind.Add(strHouses[0],int.Parse(strHouses[1])-1);
								State.HousePlayerID.Add(strHouses[0],strHouses[2]);
								
								State.HouseHP.Add(strHouses[0],int.Parse(strHouses[5]));
								State.HousePositionX.Add(strHouses[0],int.Parse(strHouses[3]));
								State.HousePositionZ.Add(strHouses[0],int.Parse(strHouses[4]));
								State.HouseLevel.Add(strHouses[0],int.Parse(strHouses[7]));
								State.HouseStatusNow.Add(strHouses[0],int.Parse(strHouses[8]));
								State.HouseStatus.Add(strHouses[0],false);
								State.HouseUpdate.Add (strHouses[0],5);
								if(!State.HouseList.ContainsKey(strHouses[0])){
									State.HouseList.Add (strHouses[0],strHouses[0]);
								}

							}else{

								State.HouseKind[strHouses[0]]=int.Parse(strHouses[1])-1;
								State.HouseLevel[strHouses[0]]=int.Parse(strHouses[7]);
								State.HouseStatusNow[strHouses[0]]=int.Parse(strHouses[8]);
								State.HouseHP[strHouses[0]]=int.Parse(strHouses[5]);
								State.HouseUpdate[strHouses[0]]=5;
								

							}
							break;
						case "33":
							print ("Source:"+item);
							
							strInfo=item.Substring(2);
							string[] strSource=strInfo.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
							if(strSource.Length<2 && strSource[1]!="" &&strSource[1]!=null){

								continue;
							}

							if(int.Parse(strSource[1])==0){
								if(strSource.Length<5 && strSource[4]!="" &&strSource[4]!=null){
									continue;
								}
							}else{
								if(strSource.Length<7 && strSource[6]!="" &&strSource[6]!=null){
									continue;
								}
							}

							if(!State.SourceID.ContainsKey(strSource[0])){
								int[] quatity =new int[3];
								State.SourceID.Add(strSource[0],strSource[0]);
								State.Sourcekind.Add(strSource[0],int.Parse(strSource[1]));

								State.SourcePositionX.Add(strSource[0],int.Parse(strSource[2]));
								State.SourcePositionZ.Add(strSource[0],int.Parse(strSource[3]));
								State.SourceUpdate.Add (strSource[0],5);
								if(!State.SourceList.ContainsKey(strSource[0])){
									State.SourceList.Add (strSource[0],strSource[0]);
								}
								if(int.Parse(strSource[1])==0){
									quatity[0]=int.Parse(strSource[4]);
									quatity[1]=0;
									quatity[2]=0;
									
								}
								else{
									quatity[0]=int.Parse(strSource[4]);
									quatity[1]=int.Parse(strSource[5]);
									quatity[2]=int.Parse(strSource[6]);
								}
								State.SourceQuatity.Add(strSource[0],quatity);
								State.SourceStatus.Add(strSource[0],false);
								
							}else{
								int[] quatity =new int[3];
								
								if(int.Parse(strSource[1])==0){
									quatity[0]=int.Parse(strSource[4]);
									quatity[1]=0;
									quatity[2]=0;
									
								}
								else{
									quatity[0]=int.Parse(strSource[4]);
									quatity[1]=int.Parse(strSource[5]);
									quatity[2]=int.Parse(strSource[6]);
								}
								State.SourceQuatity[strSource[0]]=quatity;
								State.SourceUpdate[strSource[0]]=5;
								
							}
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

				print ("error6");
				print (ex);
				State.clientSocket.Close();//關閉通訊器
				

			//	Player.infomationText("伺服器斷線了！");
				
				print("伺服器斷線了！");//顯示斷線
				
				th_Listen.Abort();//刪除執行緒
				//Application.LoadLevel("Start");
				State.isConnect=false;
				
			}
		} 
	}
	static public void Send(string strSend)
	{
		byte[] byteSend = new byte[1024];
		strSend+="@@@@@";
		try
		{
			byteSend = Encoding.UTF8.GetBytes(strSend);
			State.clientSocket.Send (byteSend);  
		}
		catch (Exception ex)
		{
			print ("error7");
			State.clientSocket.Close();//關閉通訊器
			th_Listen.Abort();//刪除執行緒
			
			State.isConnect=false;

		//	Player.infomationText("伺服器斷線了！");
			
			print(" Connection Break!");
			//	Application.LoadLevel("Start");
			
		//	Server.ConnectToServer();
		//	Server.Send("10"+State.ID+","+State.Password+"@@@@@");
			
		}
	}
}
