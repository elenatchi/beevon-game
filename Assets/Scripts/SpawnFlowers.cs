using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFlowers : MonoBehaviour
{
    public int flowerNumberWhite = 1;
    public int flowerNumberVine = 3;
    public int flowerNumberBlue = 15;
    public int flowerNumberPink = 15;
    public GameObject prefab;
    public GameObject vine_prefab;
    public GameObject planet;

    List<Collider> flowerList = new List<Collider>();
    List<int> flowerNumberList = new List<int>();
    GameObject instantiated_flower;

    // Start is called before the first frame update
    void Start()
    {
        SpawnFlowerField();
    }

    public void SpawnFlowerField()
    {
        //Delete all remaining flowers
        foreach(GameObject flower in GameObject.FindGameObjectsWithTag("Flower"))
        {
            GameObject.Destroy(flower);
        }
        foreach (GameObject vine in GameObject.FindGameObjectsWithTag("Vine"))
        {
            GameObject.Destroy(vine);
        }
        flowerNumberList.Clear();
        flowerList.Clear();

        //Add hive and lake to collider list
        Collider hive = GameObject.FindGameObjectWithTag("Hive").GetComponent<Collider>();
        Collider lake = GameObject.FindGameObjectWithTag("Lake").GetComponent<Collider>();
        flowerList.Add(hive);
        flowerList.Add(lake);

        //Initialize
        int flower_int = 0;
        flowerNumberList.Add(flowerNumberVine);
        flowerNumberList.Add(flowerNumberWhite);
        flowerNumberList.Add(flowerNumberBlue);
        flowerNumberList.Add(flowerNumberPink);

        foreach (int flowerNumber in flowerNumberList)
        {
            int temp_flowerNumber = flowerNumber;

            for (int i = 0; i < temp_flowerNumber; i++)
            {
                if (temp_flowerNumber > flowerNumber * 5) { break; };
                float r = planet.transform.localScale.x / 2;
                Vector3 point = ((Random.onUnitSphere * r) + planet.transform.position); // put the ray randomly around the transform

                if (flower_int == 0)
                {
                    // Instantiation mesh
                    instantiated_flower = Instantiate(vine_prefab, point, Quaternion.identity);

                    //Orient mesh
                    instantiated_flower.transform.LookAt(planet.transform);
                    instantiated_flower.transform.Rotate(-90, 0, 0);
                }
                else
                {
                    // Instantiation mesh
                    instantiated_flower = Instantiate(prefab, point, Quaternion.identity);

                    //Orient mesh
                    instantiated_flower.transform.LookAt(planet.transform);
                    instantiated_flower.transform.Rotate(-90, 0, 0);

                    //Set flower type
                    FlowerScript flower_script = instantiated_flower.GetComponent<FlowerScript>();
                    flower_script.flowerType = (FlowerScript.Flower)flower_int-1;
                    flower_script.UpdateFlower();
                }

                //if intersecting with another flower, replace
                bool isIntersecting = false;
                Collider flowerCollider = instantiated_flower.GetComponent<Collider>();

                foreach (Collider col in flowerList)
                {
                    if (flowerCollider.bounds.Intersects(col.bounds))
                    {
                        GameObject.Destroy(instantiated_flower);
                        temp_flowerNumber += 1;
                        isIntersecting = true;
                        break;
                    }
                }
                // Add to list
                if (!isIntersecting) { flowerList.Add(flowerCollider); }
            }
            flower_int += 1;
        }

    }
}
