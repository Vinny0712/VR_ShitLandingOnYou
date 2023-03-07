using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poop : MonoBehaviour
{
    [SerializeField] float vForce;
    [SerializeField] AudioSource audioSource;
    Rigidbody rb;

    //Use to switch between Force Modes
    enum ModeSwitching { Start, Impulse, Acceleration, Force, VelocityChange };
    //ModeSwitching m_ModeSwitching;
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb=GetComponent<Rigidbody>();
        //m_ModeSwitching = ModeSwitching.Start;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(new Vector3(0 , vForce, 0), ForceMode.Force);
    }
    void OnCollisionEnter(Collision collision){
        //add shit sound
        //audioSource.Play();
       Destroy(gameObject, 5);
       //print("shit velocity"+rb.velocity.y);
    }
}