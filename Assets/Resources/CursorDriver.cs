using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace WasaaMP {
    public class CursorDriver : MonoBehaviourPun
    {
        private bool active;
        private CursorTool cursor;
        private Camera theCamera;

        void Start()
        {
            if (photonView.IsMine || !PhotonNetwork.IsConnected)
            {
                // get the camera
                //theCamera = (Camera)GameObject.FindObjectOfType (typeof(Camera)) ;
                theCamera = (Camera)transform.parent.GetComponentInChildren(typeof(Camera));
                active = false;
                cursor = GetComponent<CursorTool>();
            }
        }

        void Update()
        {
            if (photonView.IsMine || !PhotonNetwork.IsConnected)
            {
                // ajouter aussi votre éventuel code qui gère les autres événements
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    active = true;
                }
                if (Input.GetKeyUp(KeyCode.Z))
                {
                    active = false;
                }
                if ((Input.mousePosition != null) && (active))
                {
                    Vector3 point = new Vector3();
                    Vector3 mousePos = Input.mousePosition;
                    float deltaZ = Input.mouseScrollDelta.y / 10.0f;
                    cursor.transform.Translate(0, 0, deltaZ);
                    // Note that the y position from Event should be inverted, but maybe it is not true any longer...
                    // mousePos.y = myCamera.pixelHeight - mousePos.y ;
                    point = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cursor.transform.localPosition.z));
                    cursor.transform.position = point;
                }
                if (Input.GetButtonDown("Fire1"))
                {
                    cursor.photonView.RPC("Catch", RpcTarget.All);
                }
                if (Input.GetButtonUp("Fire1"))
                {
                    cursor.photonView.RPC("Release", RpcTarget.All);
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    cursor.photonView.RPC("Catch", RpcTarget.All);
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    cursor.photonView.RPC("Release", RpcTarget.All);
                }
            }

        }
    }
}