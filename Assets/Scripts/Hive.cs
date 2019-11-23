using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Hive : MonoBehaviour
{
    public int shakeCounter = 0;
    public int maxShake = 30;
    public ParticleSystem pollenEmitter;
    public GameObject lightBeam;
    AudioSource audioSource;
    private bool inRange;
    BeevonMovement player;
    ScoreManager manager;
    UnityEvent convertToScore = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        lightBeam.SetActive(false);
        //Connect to score manager
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<ScoreManager>();
        convertToScore.AddListener(manager.AddHoney);

        //Connect to player
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<BeevonMovement>();

        //Get audio Source
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeCounter >= maxShake && inRange)
        {
            convertToScore.Invoke();
            shakeCounter = 0;
            lightBeam.SetActive(false);
            inRange = false;

            player.StopCollect();
            audioSource.Stop();
        }
    }

    public void EmptyPollen()
    {
        if (inRange)
        {
            if (shakeCounter == 0)
            {
                player.Collect();
                audioSource.Play();
            }
            pollenEmitter.Play();
            shakeCounter += 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (manager.pollen > 0)
            {
                inRange = true;
                lightBeam.SetActive(true);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRange = false;
            lightBeam.SetActive(false);

            player.StopCollect();
            audioSource.Stop();
        }
    }
}
