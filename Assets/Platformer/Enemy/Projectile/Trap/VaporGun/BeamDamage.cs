using UnityEngine;
using System.Collections;   
public class BeamDamage : MonoBehaviour
{
    public VaporGun owner;    // assign via inspector
    private void OnTriggerStay2D(Collider2D other)
    {
        owner.HandleTriggerStay(other);
    }
}
