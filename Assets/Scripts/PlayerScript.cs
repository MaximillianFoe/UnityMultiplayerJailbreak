using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public float speed;
    public int playerScore;

    public static Vector3 tuzakBirPozisyon;
    public static Vector3 tuzakIkiPozisyon;
    public static Vector3 tuzakUcPozisyon;
    public static Vector3 donusTuzakPozisyon;

    public GameObject playerOnePrefab;
    public GameObject playerTwoPrefab;

    Rigidbody rb;
    PhotonView photonView;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PhotonNetwork.IsMasterClient) {
        PhotonNetwork.Instantiate(playerOnePrefab.name, new Vector3(-4, 2, 0), Quaternion.identity);
        PhotonNetwork.NickName = "Gardiyan";
        }  
        else {
        PhotonNetwork.Instantiate(playerTwoPrefab.name, new Vector3(-10, 2, -10), Quaternion.identity);
        PhotonNetwork.NickName = "Score: " + playerTwoPrefab.GetComponent<PlayerScript>().playerScore;
        }
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Move();
        }
    }

    void Update(){
        GetComponentInChildren<TextMeshPro>().text = PhotonNetwork.NickName;
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        Vector3 newVector = new Vector3(horizontal, 0, vertical);

        rb.position += newVector;
    }

    private void OnTriggerStay(Collider TagTarget)
    {
        if(TagTarget.tag == "TuzakBir" && Input.GetKeyDown(KeyCode.E))
          {
            Animator Trap = TagTarget.GetComponentInChildren<Animator>();
            tuzakBirPozisyon = TagTarget.transform.GetChild(0).position;
            Debug.Log("Tuzak 1 Aktiflestirildi! Onceki Pozisyon: " + tuzakBirPozisyon);
            Trap.SetTrigger("TuzakTriggered");        
          }
        if(TagTarget.tag == "TuzakIki" && Input.GetKeyDown(KeyCode.E))
          {
            Animator Trap = TagTarget.GetComponentInChildren<Animator>();
            tuzakIkiPozisyon = TagTarget.transform.GetChild(0).position;
            Debug.Log("Tuzak 2 Aktiflestirildi! Onceki Pozisyon: " + tuzakIkiPozisyon);
            Trap.SetTrigger("TuzakTriggered");         
          }
        if(TagTarget.tag == "TuzakUc" && Input.GetKeyDown(KeyCode.E))
          {
            Animator Trap = TagTarget.GetComponentInChildren<Animator>();
            tuzakUcPozisyon = TagTarget.transform.GetChild(0).position;
            Debug.Log("Tuzak 3 Aktiflestirildi! Onceki Pozisyon: " + tuzakUcPozisyon);
            Trap.SetTrigger("TuzakTriggered");         
          }
        if(TagTarget.tag == "DonusTuzak" && Input.GetKeyDown(KeyCode.E))
          {
            Animator Trap = TagTarget.GetComponentInChildren<Animator>();
            donusTuzakPozisyon = TagTarget.transform.GetChild(0).position;
            Debug.Log("Donus Tuzagi Aktiflestirildi! Onceki Pozisyon: " + donusTuzakPozisyon);
            Trap.SetTrigger("TuzakTriggered");         
          }     
    }

    public void OnCollisionEnter(Collision TrapCol)
    {
            if (TrapCol.gameObject.tag == "BitisCizgisi" && !PhotonNetwork.IsMasterClient)
            {
            photonView.RPC("MahkumKazandi", RpcTarget.All); 
            }
            if (TrapCol.gameObject.tag == "TuzakBir_Temas" && !PhotonNetwork.IsMasterClient)
            {
            Debug.Log("Mahkum Tuzağa Değdi!");
            photonView.RPC("newScore", RpcTarget.All);
            if(playerScore <= 0){
            photonView.RPC("GardiyanKazandi", RpcTarget.All);
            } 
            }
            if (TrapCol.gameObject.tag == "TuzakIki_Temas" && !PhotonNetwork.IsMasterClient)
            {
            Debug.Log("Mahkum Tuzağa Değdi!");
            photonView.RPC("newScore", RpcTarget.All);
            if(playerScore <= 0){
            photonView.RPC("GardiyanKazandi", RpcTarget.All);
            } 
            }
            if (TrapCol.gameObject.tag == "TuzakUc_Temas" && !PhotonNetwork.IsMasterClient)
            {
            Debug.Log("Mahkum Tuzağa Değdi!");
            photonView.RPC("newScore", RpcTarget.All);
            if(playerScore <= 0){
            photonView.RPC("GardiyanKazandi", RpcTarget.All);
            }
            }
            if (TrapCol.gameObject.tag == "DonusTuzak_Temas" && !PhotonNetwork.IsMasterClient)
            {
            Debug.Log("Mahkum Tuzağa Değdi!");
            photonView.RPC("newScore", RpcTarget.All);
            if(playerScore <= 0){
            photonView.RPC("GardiyanKazandi", RpcTarget.All);
            }
            } 
            if (TrapCol.gameObject.tag == "OtomatikTuzak_Temas" && !PhotonNetwork.IsMasterClient)
            {
            Debug.Log("Mahkum Tuzağa Değdi!");
            photonView.RPC("newScore", RpcTarget.All);
            if(playerScore <= 0){
            photonView.RPC("GardiyanKazandi", RpcTarget.All);
            }  
            }
    } 

    [PunRPC]
    void GardiyanKazandi()
    {
        PhotonNetwork.LoadLevel("GardiyanKazandi");
        Debug.Log("Gardiyan Kazandı!");
    }

    [PunRPC]
    void MahkumKazandi()
    {
        PhotonNetwork.LoadLevel("BasariylaKacildi");
        Debug.Log("Mahkum Başarıyla Kaçtı!");
    }

    [PunRPC]
    public void newScore()
    {
        if (!photonView.IsMine) return;
        playerScore = playerScore - 1;
        if(!PhotonNetwork.IsMasterClient) PhotonNetwork.NickName = "Score: " + playerScore;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerScore);
        }
        else
        {
            playerScore = (int)stream.ReceiveNext();
        }
    }
}
