using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkConnector : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    #region Pun Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("Sunucuya Başarıyla Bağlanıldı!");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"Sunucuya Bağlantı Başarısız Oldu: {cause}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        PhotonNetwork.CreateRoom("JBR");
    }

    public override void OnPlayerEnteredRoom(Player Oyuncu)
    {
        Debug.Log($"{PhotonNetwork.CurrentRoom.Name} Odasına Katıldınız!");
        if (PhotonNetwork.IsMasterClient){
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2) {
            PhotonNetwork.LoadLevel("OyunSahnesi");
        } 
    }
    }

    #endregion
}
