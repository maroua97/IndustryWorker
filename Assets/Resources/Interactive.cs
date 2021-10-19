using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace WasaaMP {
    public class Interactive : MonoBehaviourPun {

        private Color colorBeforeHighlight;
        private Color oldColor;
        private float oldAlpha;
        private Vector3 initialPosition;
        private bool catchable = false;
        private bool caught = false;
        private Transform oldParent = null;
        private bool knob = false;

        void Start () {
            initialPosition = transform.position;
            oldParent = transform.parent;
            if (oldParent != null && oldParent.GetComponent<InteractiveHeavy>() != null)
            {
                knob = true;
            }
        }

        void Update () {
            if (knob && transform.parent != oldParent)
            {             
                transform.position = initialPosition;
            }
        }

        [PunRPC] public void ShowCaught () {
            if (! caught) {
                var rb = GetComponent<Rigidbody> () ;
                if (knob)
                {
                    rb.isKinematic = true;
                }
                Renderer renderer = GetComponentInChildren <Renderer> () ;
                oldColor = renderer.material.color ;
                renderer.material.color = Color.yellow ;
                caught = true ;
            }
        }

        [PunRPC] public void ShowReleased () {
            if (caught) {
                var rb = GetComponent<Rigidbody> () ;
                if (knob)
                {
                    rb.isKinematic = false;
                }
                Renderer renderer = GetComponentInChildren <Renderer> () ;
                renderer.material.color = oldColor ;
                caught = false ;
            }
        }

        [PunRPC] public void ShowCatchable () {
            if (! caught) {
                if (! catchable) {
                    Renderer renderer = GetComponentInChildren <Renderer> () ;
                    oldAlpha = renderer.material.color.a ;
                    colorBeforeHighlight = renderer.material.color ;
                    //Color c = renderer.material.color ;
                   
                    Color c = Color.cyan ;
                    renderer.material.color = new Color (c.r, c.g, c.b, 0.5f) ;
                    catchable = true ;
                }
            }
        }
        
        [PunRPC] public void HideCatchable () {
            if (! caught) {
                if (catchable) {
                    Renderer renderer = GetComponentInChildren <Renderer> () ;
                    //Color c = renderer.material.color ;
                    Color c = colorBeforeHighlight ;
                    renderer.material.color = new Color (c.r, c.g, c.b, oldAlpha) ;
                    catchable = false ;
                }
            }
        }

    }

}
