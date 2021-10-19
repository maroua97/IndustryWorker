using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace WasaaMP {
    public class NavigationOculus : MonoBehaviourPunCallbacks {
        UnityEngine.XR.InputDevice device;
        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private bool isPressed = false;
        private const float deltaT = 0.05f;
        private const float deltaR = 0.5f;

        #region Public Fields

        // to be able to manage the offset of the camera
        public Vector3 cameraPositionOffset = new Vector3(0, 1.6f, 0);
        public Quaternion cameraOrientationOffset = new Quaternion();

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        #endregion
        void Awake() {
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine) {
                LocalPlayerInstance = this.gameObject;
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            //DontDestroyOnLoad (this.gameObject) ;
        }

        void Start() {
            //if (photonView.IsMine) {
            print ("attachement de la caméra de l'oculus") ;
            // attach the camera to the navigation rig
            Camera theCamera = (Camera)this.gameObject.GetComponentInChildren (typeof(Camera)) ;
            Camera.main.enabled = false ;
            theCamera.enabled = true ;
            device = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.LeftHand);
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            //} 
     
        }

        void Update()
        {
            if (photonView.IsMine)
            {
                device.IsPressed(InputHelpers.Button.PrimaryButton, out isPressed); // X button
                if (isPressed)
                {
                    transform.SetPositionAndRotation(initialPosition, initialRotation);
                    Camera theCamera = (Camera)this.gameObject.GetComponentInChildren(typeof(Camera));
                    theCamera.enabled = true;
                }
                // translation
                device.IsPressed(InputHelpers.Button.PrimaryAxis2DUp, out isPressed, 0.3f); // Joystick
                if (isPressed)
                {
                    transform.Translate(0, 0, deltaT);
                }
                device.IsPressed(InputHelpers.Button.PrimaryAxis2DDown, out isPressed, 0.3f); // Joystick
                if (isPressed)
                {
                    transform.Translate(0, 0, -deltaT);
                }
                // rotation
                device.IsPressed(InputHelpers.Button.PrimaryAxis2DRight, out isPressed, 0.3f); // Joystick
                if (isPressed)
                {
                    transform.Rotate(0, deltaR, 0);
                }
                device.IsPressed(InputHelpers.Button.PrimaryAxis2DLeft, out isPressed, 0.3f); // Joystick
                if (isPressed)
                {
                    transform.Rotate(0, -deltaR, 0);
                }
            }


        }

    }
}