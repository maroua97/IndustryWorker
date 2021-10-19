using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace WasaaMP
{
    public class CursorToolOculus : MonoBehaviourPun
    {
        private bool caught;
        public Interactive interactiveObjectToInstanciate;
        private InteractiveBox target;
        private MonoBehaviourPun targetParent;
        private Transform oldParent = null;

        void Start()
        {
            caught = false;
        }

        [PunRPC]
        public void SetParent()
        {
            target.transform.parent = transform;
        }

        [PunRPC]
        public void SetOldParent()
        {
            target.transform.parent = oldParent;
        }

        

        [PunRPC]
        public void Catch()
        {
            print("Catch ?");
            if (target != null)
            {
                if ((!caught) && (transform != target.transform))
                { // pour ne pas prendre 2 fois l'objet et lui faire perdre son parent
                    oldParent = target.transform.parent;
                    //photonView.
                    target.transform.parent = transform;
                    //photonView.RPC("SetParent", RpcTarget.All);
                    if (photonView.IsMine)
                    {
                        target.photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                    }
                    //target.photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                    target.photonView.RPC("ShowCaught", RpcTarget.All);
                    //text.text = target.transform.parent.name.ToString();
                    PhotonNetwork.SendAllOutgoingCommands();
                    caught = true;
                }
                print("Catch !");
            }
            else
            {
                print("Catch failed");
            }           
        }

        [PunRPC]
        public void Release()
        {
            if (caught)
            {
                print("Release :");
                if (target != null)
                {
                    target.transform.parent = oldParent;
                    photonView.RPC("SetOldParent", RpcTarget.All);
                    target.photonView.RPC("ShowReleased", RpcTarget.All);
                    PhotonNetwork.SendAllOutgoingCommands();
                    print("Release !");
                    caught = false;
                }
            }
        }


        public void CreateInteractiveCube()
        {
            var objectToInstanciate = PhotonNetwork.Instantiate(interactiveObjectToInstanciate.name, transform.position, transform.rotation, 0);
        }


        void OnTriggerEnter(Collider other)
        {
            if (!caught)
            {
                print(name + " : CursorTool OnTriggerEnter");
                target = other.gameObject.GetComponent<InteractiveBox>();

                if (target != null)
                {
                    target.photonView.RPC("ShowCatchable", RpcTarget.All);
                    PhotonNetwork.SendAllOutgoingCommands();
                }
            }           
        }


        void OnTriggerExit(Collider other)
        {
            if (!caught)
            {
                print(name + " : CursorTool OnTriggerExit");
                if (target != null)
                {
                    target.photonView.RPC("HideCatchable", RpcTarget.All);
                    PhotonNetwork.SendAllOutgoingCommands();
                    target = null;
                }
            }           
        }

    }

}