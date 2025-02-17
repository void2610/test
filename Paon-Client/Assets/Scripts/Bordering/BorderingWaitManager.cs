using System.Collections;
using System.Collections.Generic;
using Paon.NNetwork;
using Paon.NPlayer;
using UnityEngine;
using UnityEngine.UI;

namespace Paon.NBordering
{
    public class BorderingWaitManager : MonoBehaviour
    {
        bool Flag;

        int count;

        Vector3 temp;

        public int NowPeople = 0;

        public int MaxPeople = 2;

        public GameObject[] WaitAreas = new GameObject[3];

        GameObject WaitText;

        int check;

        private GameObject client;

        void Start()
        {
            WaitAreas[0] = GameObject.Find("WaitArea1");
            WaitAreas[1] = GameObject.Find("WaitArea2");
            WaitAreas[2] = GameObject.Find("WaitArea3");
            client = GameObject.Find("GameClient");
            WaitText = GameObject.Find("WaitText");
        }

        public void FlagCheck(bool Flags)
        {
            Flag = Flags;

            //Debug.Log(Flag);
        }

        public void KindCheck(int Count)
        {
            count = Count - 2;
        }

        void Update()
        {
            check = client.GetComponent<BorderingClient>().OfflineMode();

            if (check == 1)
            {
                Flag = true;
            }
            else
            {
                client.GetComponent<BorderingClient>().CheckBorder();
            }
            if (
                WaitAreas[0].GetComponent<WaitAreaScript>().ReadyPlayer !=
                null &&
                !Flag
            )
            {
                WaitText.GetComponent<Text>().text =
                    "じゅんばんまち　ちょっとまってね！";
            }
            if (
                Flag ||
                WaitAreas[0].GetComponent<WaitAreaScript>().ReadyPlayer == null
            )
            {
                WaitText.GetComponent<Text>().text = "";
            }

            if (NowPeople < MaxPeople)
            {
                //人数に空きがあって、待機エリアに人がいる場合、プレイ中にしてテレポートさせる
                if (
                    WaitAreas[0].GetComponent<WaitAreaScript>().ReadyPlayer !=
                    null &&
                    Flag == true
                )
                {
                    WaitAreas[0]
                        .GetComponent<WaitAreaScript>()
                        .ReadyPlayer
                        .GetComponent<PlayerMove>()
                        ._Player
                        .playingBordering = true;

                    WaitAreas[0]
                        .GetComponent<WaitAreaScript>()
                        .TeleportPlayer();
                    NowPeople++;
                    client.GetComponent<BorderingClient>().StartBorder();
                }
            }

            if (
                WaitAreas[0].GetComponent<WaitAreaScript>().ReadyPlayer ==
                GameObject.Find("PlayerBody")
            )
            {
                temp = GameObject.Find("PlayerBody").transform.position;
            }

            //次の待機エリアにテレポートさせる
            if (
                WaitAreas[0].GetComponent<WaitAreaScript>().ReadyPlayer ==
                null &&
                WaitAreas[1].GetComponent<WaitAreaScript>().ReadyPlayer != null
            )
            {
                WaitAreas[1].GetComponent<WaitAreaScript>().TeleportPlayer();
            }
            if (
                WaitAreas[1].GetComponent<WaitAreaScript>().ReadyPlayer ==
                null &&
                WaitAreas[2].GetComponent<WaitAreaScript>().ReadyPlayer != null
            )
            {
                WaitAreas[2].GetComponent<WaitAreaScript>().TeleportPlayer();
            }

            if (
                WaitAreas[0].GetComponent<WaitAreaScript>().ReadyPlayer !=
                null &&
                temp != GameObject.Find("PlayerBody").transform.position
            )
            {
                if (count == 2)
                {
                    PlayerPrefs
                        .SetInt("GiveTurn",
                        PlayerPrefs.GetInt("GiveTurn", 0) + 1);
                }
            }
        }
    }
}
