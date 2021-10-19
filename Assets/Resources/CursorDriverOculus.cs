using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

namespace WasaaMP {
	public class CursorDriverOculus : MonoBehaviourPun {
		private bool active ;
		private CursorToolOculus cursor ;
		private Camera theCamera ;
        private bool pressed = false;
        private Renderer cursorRenderer;
        private UnityEngine.XR.InputDevice device;
        public Text text;

        void Start () {
			if (photonView.IsMine || ! PhotonNetwork.IsConnected) {
				cursor = GetComponent<CursorToolOculus> () ;
                device = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.RightHand);
                text = (Text)GameObject.FindObjectOfType(typeof(Text));
                //cursorRenderer = GetComponentInChildren<Renderer>();
            }
		}
		
		void Update () {
			if (photonView.IsMine  || ! PhotonNetwork.IsConnected) {
                if (pressed)
                {
                    device.IsPressed(InputHelpers.Button.Trigger, out pressed);
                    if (!pressed)
                    {
                        //cursorRenderer.material.color = Color.blue;
                        cursor.photonView.RPC("Release", RpcTarget.All);
                    }
                }
                if (!pressed)
                {
                    device.IsPressed(InputHelpers.Button.Trigger, out pressed);
                    if (pressed)
                    {
                        //cursorRenderer.material.color = Color.red;
                        //cursor.Catch();
                        cursor.photonView.RPC("Catch", RpcTarget.All);
                        
                    }
                }
			}
		}

	}

}