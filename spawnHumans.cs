using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class spawnHumans : MonoBehaviour
{
    public GameObject human;
    public float spawnTime;
    private int humanCount=0;
    public int maxHumanCount;
    
    private float timer=0.0f;
    public float walkRadius=20;

    // Start is called before the first frame update
    void Start()
    {
        
            InvokeRepeating("spawnPerson",0,spawnTime);
            print("start");

            
        
    }

    // Update is called once per frame
    void Update()
    {
        timer+=Time.deltaTime;
        if(humanCount<=maxHumanCount && timer>spawnTime*maxHumanCount){
            Invoke("spawnPerson",spawnTime);
            
            
        } 
        if (humanCount>= maxHumanCount){
                CancelInvoke();
        }
    }
    void spawnPerson(){
        humanCount+=1;
        
         Vector3 randomDirection = new Vector3(Random.Range(-40, 40), 15.5f, Random.Range(-40, 40));
         print("random dir" +randomDirection);
         NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
 
    
        //Vector3 randomVector= new Vector3(UnityEngine.Random.Range(minX, maxX), 0, UnityEngine.Random.Range(minZ, maxZ));
        Vector3 spawnLocation=hit.position;
        print("spawnLocation"+spawnLocation);
        Instantiate(human,spawnLocation,Quaternion.identity);

        print("spawn human");
    }
}
