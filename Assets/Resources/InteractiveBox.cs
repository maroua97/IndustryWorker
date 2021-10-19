using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace WasaaMP
{
    public class InteractiveBox : MonoBehaviourPun
    {
       
        public Material catchableMaterial;
        public Material caughtMaterial;
        private Material oldMaterial;
        private Renderer renderer;
        private bool catchable = false;
        private bool caught = false;
        private Transform oldParent = null;
        private Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            renderer = GetComponentInChildren<Renderer>();
            oldMaterial = renderer.material;
        }


        [PunRPC]
        public void ShowCaught()
        {
            if (!caught)
            {
                rb.isKinematic = true;
                renderer.material = caughtMaterial;
                caught = true;
            }
        }

        [PunRPC]
        public void ShowReleased()
        {
            if (caught)
            {
                rb.isKinematic = false;
                renderer.material = oldMaterial;
                caught = false;
            }
        }

        [PunRPC]
        public void ShowCatchable()
        {
            if (!caught)
            {
                if (!catchable)
                {
                    renderer.material = catchableMaterial;
                    catchable = true;
                }
            }
        }

        [PunRPC]
        public void HideCatchable()
        {
            if (!caught)
            {
                if (catchable)
                {
                    renderer.material = oldMaterial;
                    catchable = false;
                }
            }
        }

    }

}
