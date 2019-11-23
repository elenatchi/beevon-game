using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{

    PlanetGravity planet;
    Rigidbody rigidbodyObject;

    void Awake()
    {
        planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<PlanetGravity>();
        rigidbodyObject = GetComponent<Rigidbody>();

        // Disable rigidbody gravity and rotation as this is simulated in GravityAttractor script
        rigidbodyObject.useGravity = false;
        rigidbodyObject.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        // Allow this body to be influenced by planet's gravity
        planet.Attract(rigidbodyObject);
    }
}