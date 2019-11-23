using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class IntEvent : UnityEvent<int>
{
}


[RequireComponent(typeof(Collider))]
public class FlowerScript : MonoBehaviour
{
    private bool inRange;
    public int pollenCounter;
    public int maxPollen = 60;
    public Light rangeLight;
    public ParticleSystem pollenEmitter;
    public GameObject pollenSprite;

    public enum Flower {White, Blue, Pink};
    public Flower flowerType;

    public GameObject[] flowerMeshList;

    IntEvent addToScore = new IntEvent() ;
    AudioSource audioSource;
    BeevonMovement player;
    ScoreManager manager;
    Vector3 target_pos;
    Image pollenImage;
    GameObject canvas;

    // Start is called before the first frame update
    void Awake()
    {
        // show or hide flower depending on type
        UpdateFlower();
        // Reset pollen counter
        pollenCounter = 0;

        //Connect to score manager
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<ScoreManager>();
        addToScore.AddListener(manager.AddPollen);

        //Connect to player
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<BeevonMovement>();
        player.AddShakeListener(this.gameObject);

        //Get audio Source
        audioSource = GetComponent<AudioSource>();

        //Get Canvas
        canvas = GameObject.FindGameObjectWithTag("Canvas");

        //Get TargetPose
        GameObject icon = GameObject.FindGameObjectWithTag("PollenNumber");
        target_pos = icon.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (pollenCounter>= maxPollen)
        {
            if (flowerType == Flower.White) { addToScore.Invoke(3); }
            else { addToScore.Invoke(1); }

            audioSource.Stop();
            player.StopCollect();

            //Check if this is the last flower
            if (GameObject.FindGameObjectsWithTag("Flower").Length == 1)
            {
                manager.GetComponent<SpawnFlowers>().SpawnFlowerField();
            }
            EmitPollen();
            GameObject.Destroy(this.gameObject);
        }
    }

    public void EmitPollen()
    {
        pollenImage = GameObject.Instantiate(pollenSprite).GetComponent<Image>();
        Vector3 pos = Camera.main.WorldToScreenPoint(rangeLight.transform.position);
        pollenImage.transform.SetParent(canvas.transform);
        pollenImage.rectTransform.position = pos;

        pollenImage.rectTransform.LeanMove(target_pos, 1f).setEaseOutCubic().setOnComplete(DestroyPollen);
    }

    void DestroyPollen()
    {
        GameObject.Destroy(pollenImage.gameObject);
    }

    public void FillUpPollen()
    {
        if (inRange)
        {
            //At first collect start music and anim
            if (pollenCounter == 0 ^ Input.GetKeyDown(KeyCode.Space))
            {
                player.Collect();
                audioSource.Play();
            }

            pollenEmitter.Play();
            pollenCounter += 1;  
        }
    }

    public void UpdateFlower()
    {
        switch (flowerType)
        {
            case Flower.Blue:
                flowerMeshList[0].SetActive(true);
                flowerMeshList[1].SetActive(false);
                flowerMeshList[2].SetActive(false);
                break;
            case Flower.Pink:
                flowerMeshList[0].SetActive(false);
                flowerMeshList[1].SetActive(true);
                flowerMeshList[2].SetActive(false);
                break;
            case Flower.White:
                flowerMeshList[0].SetActive(false);
                flowerMeshList[1].SetActive(false);
                flowerMeshList[2].SetActive(true);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRange = true;
            rangeLight.intensity = 10;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Stop light and fx
            inRange = false;
            rangeLight.intensity = 0;
            pollenEmitter.gameObject.SetActive(false);

            //Stop music and player anim
            player.StopCollect();
            audioSource.Stop();
        }
    }
}
