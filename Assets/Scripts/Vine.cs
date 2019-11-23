using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour
{
    private ScoreManager manager;
    private BeevonMovement player;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //Get manager
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<ScoreManager>();

        //Connect to player
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<BeevonMovement>();

        //Get audio Source
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HurtBeevon()
    {
        manager.RemovePollen();

        //Play hurt Animation
        player.HurtBeevon();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            HurtBeevon();
            audioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Play hurt Animation
        player.UnhurtBeevon();
    }
}
