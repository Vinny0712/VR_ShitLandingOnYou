using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class human : MonoBehaviour
{
    [SerializeField] public EventSystem eventSystem;
    [SerializeField] public VRControls vRControls;
    // Start is called before the first frame update
    NavMeshAgent NavMeshAgentHuman;
    SkinnedMeshRenderer humanRenderer;
    MeshRenderer foodRenderer;
    AudioSource[] audioSource;
    
    
    public float walkRadius=10;
    
    enum collisionState {none,checkState,collidedHuman, collidedFood,collidedShit};

    private collisionState collided=collisionState.none;
    
    
    
    void Start()
    
    {
        NavMeshAgentHuman = GetComponent<NavMeshAgent>(); 
        humanRenderer=gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        foodRenderer=gameObject.GetComponentInChildren<MeshRenderer>();
        audioSource = GetComponents<AudioSource>();
        
        
        
        InvokeRepeating("navigate",0,5);
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        
    }
    void navigate(){
        Vector3 randomDirection = new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f));
         randomDirection += transform.position;
         //print("current pos"+transform.position);
         //print("suggested pos"+randomDirection);
         
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        Vector3 target = hit.position;
        //print("target pos"+target);
        NavMeshAgentHuman.SetDestination(target);
    }

    void animateHumanAndTriggerFood(){
        collided=collisionState.checkState;
            
            
            GetComponent<Animator>().SetBool("isAttacked",true);
            GetComponent<Animator>().SetBool("isRespawned",false);
            Invoke("checkState",5.0f);// time taken for attack animation to end
    }
     void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Player gull"&& collided ==collisionState.none)//and y velocity>number
        //need get the y velocity via rigid body of capsule of keyboard first
        {
            //add collision sound
            //print("touch gull");
            //CharacterController rbGull=collision.gameObject.GetComponent<CharacterController>();
            //float yVelocity=rbGull.velocity.y;
            //print("y velocity gull on collision"+ yVelocity);
            //float terminalVelocity=-50;
            if(vRControls.isDive){//if (yVelocity<=terminalVelocity){
                //add human screaming sound
            audioSource[1].Play();
            animateHumanAndTriggerFood();
            }
            
            


        }

        //to handle collision w food
        if (collision.gameObject.tag == "Player gull"&& collided ==collisionState.collidedHuman)
        {
            collided=collisionState.collidedFood;
            print("food collided w gull");
            //add eating food sound
            audioSource[2].Play();

            //add food
            eventSystem.Eat();
            //check if the food bar is fully refilled
            if (eventSystem.CurrentFood >= eventSystem.MaxFood) {
                Debug.Log("max food reached");
                eventSystem.scaleup();
                eventSystem.AddShit();
                eventSystem.scaledown();
            }
           //food disappear then respawn later
            foodRenderer.enabled=!foodRenderer.enabled;
            NavMeshAgentHuman.enabled=!NavMeshAgentHuman.enabled;
           Invoke("respawnHuman",10.0f);//to shorten respawn
            


        }
        //if collision is shit and y velocity>number => change state to hit alr then add point, then disappear or some other effect
        if (collision.gameObject.tag == "shit"&& collided==collisionState.none)
        {
            Rigidbody rbShit=collision.gameObject.GetComponent<Rigidbody>();
            float yVelocity=rbShit.velocity.y;
            if(yVelocity>0){
                //collided=collisionState.collidedShit;
                
                print("shit collided w gull");
                
                //add point
                eventSystem.ActivatePoints();
                eventSystem.AddPoints();
                eventSystem.DeactivatePoints();
                //add oh shit sound
                audioSource[0].Play();
                animateHumanAndTriggerFood();

                //NavMeshAgentHuman.enabled=!NavMeshAgentHuman.enabled;//
               // CancelInvoke("navigate");//

                //spin animation
               // GetComponent<Animator>().SetBool("isAttacked",true);//
               // GetComponent<Animator>().SetBool("isRespawned",false);//
                //end
                //generate food
                
               // Invoke("respawnHumanAfterShit",5);//delay bef disappear //time taken for attack animation to end

            }
            
           
            


        }
    
}

/*void respawnHumanAfterShit(){
    humanRenderer.enabled=!humanRenderer.enabled; //disappear
    NavMeshAgentHuman.enabled=!NavMeshAgentHuman.enabled;
    //
    GetComponent<Animator>().SetBool("isRespawned",true);
    GetComponent<Animator>().SetBool("isAttacked",false);
    //
    InvokeRepeating("navigate",0,5);
    Invoke("makeHumanVisibleAfterShit",5);
}
void makeHumanVisibleAfterShit(){
    humanRenderer.enabled=!humanRenderer.enabled;
    collided=collisionState.none;
}*/
void checkState(){
    print("checking state");
    AnimatorStateInfo animationState=GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if( animationState.IsName("faint") )
{
	
        {
            print("time to turn food out!");
            makeFoodAppear();
        
            

        }
}
}

void makeFoodAppear(){

NavMeshAgentHuman.enabled=!NavMeshAgentHuman.enabled;
CancelInvoke("navigate");
humanRenderer.enabled=!humanRenderer.enabled;
foodRenderer.enabled=!foodRenderer.enabled;
collided=collisionState.collidedHuman;



}

public void respawnHuman(){
//NavMeshAgentHuman.enabled=!NavMeshAgentHuman.enabled;
humanRenderer.enabled=!humanRenderer.enabled;
collided=collisionState.none;
GetComponent<Animator>().SetBool("isAttacked",false);
GetComponent<Animator>().SetBool("isRespawned",true);

InvokeRepeating("navigate",0,5);


}

}

