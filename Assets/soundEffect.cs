using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundEffect : MonoBehaviour
{
    public AudioSource walk ;
    public AudioSource thunder ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void walkSound(){
        walk.Play();
    }

     void thunderSound(){
        thunder.Play();
    }
}
