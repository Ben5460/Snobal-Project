using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
//==========================================================================================//
//                   This script controls AI's movement behaviour and audio                 //
//                                    -Ralph Dagomboy-                                      //
//==========================================================================================//
public class AIbehaviour : MonoBehaviour {
    public Transform player;            //camera eye
    public Transform target;            //AI initial position    
    
    public Transform cube5;
    public GameObject Cube5;
    public GameObject gameControl;
    public Vector3 AI;
    public GameObject SpecialSphere;    //Sphere at target position
    public GameObject ActiveCamera;

    public GameObject normalEyes;       //AI eye emotions
    public GameObject happyEyes;
    public GameObject angryEyes;

    Vector3 SpeiclSpherePos;            //Special sphere position
    Vector3 CamPos;
    
    public float Timer;
    public float speed;   
    public int dialogueNumber;
    public float intTimer;
    int intTimerCounter = 0;
    //Audio clips of AI dialogues to be played by audio source - Ralph
    int audioControl = 1;
    public AudioClip d1GreetingsUser;
    public AudioClip d2DearUser;
    public AudioClip d3MoveTowards;
    public AudioClip d5remindMoveOver;
    public AudioClip d6BeCareful;
    public AudioClip d7PhysicalWall;
    public AudioClip d8teleport;
    public AudioClip d9FollowInstructions;
    public AudioClip d10holdPressAim;
    public AudioClip d11WellDone;
    public AudioClip d13RemindTeleport;
    public AudioClip d14TeleportSafty;
    public AudioClip d15UserBubbleOut;
    public AudioClip d16TryItOut;
    public AudioClip d17approachBubble;
    public AudioClip d19AgainifyouMay;
    public AudioClip d20GreatJob;
    public AudioClip d25Hatsoff;
    public AudioClip d32MoveOutOfSphere;
    public AudioClip d33ShowoffSkills;

    // Use this for initialization
    void Start ()
    {
        SpeiclSpherePos = SpecialSphere.transform.position;
        Timer = 0f;
        intTimer = -11f;
        dialogueNumber = 1;        
    }
	
    void faceUser()                     //AI constantly faces the user
    {
        Vector3 direction = player.position - this.transform.position;
        direction.y = 0;

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
    }
    void followUserView()               //AI follows user view
    {
        float step = speed * Time.deltaTime;
        CamPos = ActiveCamera.transform.position + ActiveCamera.transform.forward * 5;
        CamPos.y =0.2f;
        transform.position = Vector3.MoveTowards(this.transform.position, CamPos, step);
    }
    void returnToOriginalPosition()     //AI goes back to original position
    {
        Vector3 direction2 = cube5.position - this.transform.position;

        float distancePlayer = Vector3.Distance(player.transform.position, cube5.transform.position);//measure distance of player from cube5
        float distanceAI = Vector3.Distance(this.transform.position, cube5.transform.position);//measuer distance of AI from cube5
        float step = speed * Time.deltaTime;

        if (distanceAI > distancePlayer)//If AI is further than the player from the AI, uses a circular movement to avoid player incase he is on AI's way
        {
            direction2.x = 0;
            if (direction2.z > 5)
                direction2.z = 5;
            //direction2.z = 0;
            transform.position = Vector3.Slerp(this.transform.position, direction2, 0.010f);
        }
        else
            transform.position = Vector3.MoveTowards(this.transform.position, target.position, step);                       
    }
    void initialAI()
    {        
        if (Timer < 16)
            followUserView();           //Follow user view
        else
            returnToOriginalPosition(); //Return to initial position       
    }
	// Update is called once per frame
	void Update ()
    {
        intTimer += Time.deltaTime;     
        Timer += Time.deltaTime;        //Start Timer
        if (intTimer > 0 && intTimerCounter == 0)
        {
            GetComponent<Animator>().enabled = true;
            faceUser();                     //Set AI to face user for entire prgram life
            initialAI();
        }
    }
    public void waitPlaySound(AudioClip dialogue)//With for current audio to finish before playing -Ralph
    {
        AudioSource audio = GetComponent<AudioSource>();
        
        audio.clip = dialogue;
        audio.Play();
        //waitSound();
    }
    public void playSound(AudioClip dialogue) //Stop current audio being played and play the next audio - Ralph
    {
        audioControl = 1;
        AudioSource audio = GetComponent<AudioSource>();
       
        if (audio.isPlaying == true)
        {
            audio.Stop();
            audio.clip = dialogue;
            audio.Play();

        }
       else
        {
            audio.clip = dialogue;
            audio.Play();
        }
            
                  
    }
    //Change AI's eye emotions
    void EyeEmotions(GameObject CurrentEmotion, GameObject other, GameObject other1, GameObject other2)
    {
        other.SetActive(false);
        other1.SetActive(false);
        other2.SetActive(false);
        CurrentEmotion.SetActive(true);
    }   
}
