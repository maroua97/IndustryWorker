using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace WasaaMP
{
    public class InteractiveHeavy : MonoBehaviourPun
    {

        private List<Transform> knobsStart = new List<Transform>();
        private List<Transform> knobsGrabbed = new List<Transform>();
        private List<Transform> oldParents = new List<Transform>();
        private Vector3 newPosition = Vector3.zero;
        private Transform knob;
        private IDictionary<Transform, Vector3> positions = new Dictionary<Transform, Vector3>();

        void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                knob = transform.GetChild(i);
                knobsStart.Add(knob);
                positions.Add(knob, knob.position);
                print(knob.name);
            }
        }

        [PunRPC] public void SetParentAtHeavy()
        {
            knob.parent = transform;
        }


        void Update()
        {
            if (KnobsLifted())
            {
                foreach (Transform knob in knobsGrabbed)
                {
                    if (!(knob.IsChildOf(transform)))
                    {
                        positions[knob] = knob.parent.position;
                        knob.parent = transform;
                        photonView.RPC("SetParentAtHeavy", RpcTarget.All);                        
                    }
                }               
                newPosition = (oldParents[0].position + oldParents[1].position) / 2;
                print(newPosition);
                transform.position = newPosition;

            }
            else
            {
                foreach (Transform knob in knobsStart)
                {
                    knob.position = positions[knob];
                    print(knob.name);
                    print(positions[knob]);
                }
            }

        }


        public bool KnobsLifted()
        {
            foreach (Transform knob in knobsStart)
            {
                if (!(knob.IsChildOf(transform)))
                {
                    if (!(knobsGrabbed.Contains(knob)))
                    {                     
                        oldParents.Add(knob.parent);
                        knobsGrabbed.Add(knob);                       
                       
                    }
                }
                else
                {
                    oldParents.Clear();
                    knobsGrabbed.Clear();
                    print("Not all knobs grabbed");
                    return false;
                }
            }

            return true;
        }
    }
}
