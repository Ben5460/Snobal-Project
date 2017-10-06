using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using VRTK;
//=================================================================================================//
//This script controls the main sequence of the tutorial along with the audio and text instructions//
//=================================================================================================//
public class GameControl : MonoBehaviour {

    //Game objects
    public Material Arma;
    public GameObject PlayerEye;
    public GameObject Zombie;
    public GameObject HelloWorld;
    public GameObject objectToolTip;
    public GameObject TransparentBall;
	public GameObject controller;
	public GameObject trigger;
	public GameObject trackpad;
    public GameObject GameControlObj;
    public GameObject Ring;
    public Transform player;
    public GameObject destPoint;
    public Transform AI;
    public GameObject Ai;
    public AIbehaviour dialogcontrol;
    public GameObject light;
    public Material NightBox;
    //AI eye prefabs
    public GameObject normalEye;
    public GameObject happyEye;
    public GameObject angryEye;

    //Scripts
    public VRTK_BasicTeleport VRTK_BasicTeleporscript;
    public TransparentBallScript transparentBallExit;
    public TransparentBallMovement MoveTransparentBall;
    private Vector3 targetAngles;
    public Transform eyePosition;//AI's eye position to instantiate prefabs
    public enum OperationTypes
    {
        Ignore,
        Include
    }
    //boolean  
    public bool triggerHint;
    public bool trackpadHint;
    public bool gripHint;
    private bool check;
    public bool hasteleported;
    public bool objectExitedSphere;
    public bool bballExited = false;

    //numbers
    public float bballExitedCounter = 0;
    public float Timer;
    public  int stage;    
    public float LocalTimer = 0;
	private int loopCounter=0;
    public float smooth = 1f;//Controller rotaion speed
    public float fontVariable = 1;
    public float toolTipLength = 1;
    public float toolTipwidth = 1;
    float distancePlayer = 1;
    public int DualDialogCounter = 0;
    public float audioTime1 = 0;
    public float audioTime2 = 0;
    public float audTime3 = 0;
    public float audTime4 = 0;
    public float audTime5 = 0;
    int DualAudioClipTeleportVariable = 0;
    int objectExitedSphereCounter = 0;
    int localTimerReset = 0;
    float initTimer = -11f;//adjust based on preference
    int teleportEyeCounter = 0;
    float ArmagedonTimer = 180.0f;
    // Use this for initialization
    void Start () {
        //TransparentBall.SetActive(false);<---------------disables checkteleport if set to false
        targetAngles = transform.eulerAngles + 90f * Vector3.forward;
        destPoint.SetActive(false);
        HelloWorld.SetActive(false); //<-------------------Set to false for experimentation
		controller.SetActive(false);
		trigger.SetActive(false); 
		trackpad.SetActive (false);
        check = false;
        trackpadHint = false;
        triggerHint = false;
        gripHint = false;
        hasteleported = false;
        objectExitedSphere = false;
        objectToolTip.SetActive(false);
        Zombie.SetActive(false);
        Timer = 0f;
        
        stage = 1;

        //Eyes of AI
        happyEye.SetActive(false);
        angryEye.SetActive(false);
     
    }
	
	// Update is called once per frame
	void Update ()
    {
        dialogcontrol = Ai.GetComponent<AIbehaviour>();

        distancePlayer = Vector3.Distance(player.transform.position, AI.transform.position);
        initTimer += Time.deltaTime;
        print(initTimer);

        if (initTimer > 0 )
        {
            Timer += Time.deltaTime;
            LocalTimer += Time.deltaTime;
            stageControl();
           
            //if (initTimer > 30)
            //{
            //    RenderSettings.skybox = NightBox;
            //    light.SetActive(false);
            //    Zombie.SetActive(true);
            //}
        }       
    }

    void stageControl()//Control diffirent stages of the game
    {
        switch (stage)
        {
            case 1:
                greetUser();
                break;
            case 2:
                askAttention();//  ask user's attention
                break;
            case 3:
                boundaryChecker();
                break;
            case 4:
                teleportChecker();
                break;
            case 5:       
                objectInteractIntro();
                break;
            case 6:
                ArmagedonCountDown();
                checkBaskeballExited();             
                break;
        }
    }

  
    void greetUser()
    {
        //AI eyes game object - ralplh
        normalEye.SetActive(false);
        happyEye.SetActive(true);
        if (check == false)
        {
           
            objectToolTip.SetActive(true);
            objectToolTip.GetComponent<VRTK_ObjectTooltip>().fontSize = 6;
            objectToolTip.GetComponent<VRTK_ObjectTooltip>().containerSize = new Vector2(70f*toolTipLength, 20f*toolTipwidth);
            objectToolTip.GetComponent<VRTK_ObjectTooltip>().UpdateText("Greetings user!");//AI caption -ralph
            dialogcontrol.playSound(dialogcontrol.d1GreetingsUser);//AI voice greeting user - ralph
            
            check = true;
        }
        if (Timer >= 5)
        {
            
            stage++;
            check = false;
        }
        
        
    }
    void askAttention()
    {
        //AI eyes game object - ralph
        happyEye.SetActive(false);
        normalEye.SetActive(true);
        if (check == false)
        {
            //objectToolTip.GetComponent<VRTK_ObjectTooltip>().containerSize = new Vector2(110f, 30f);
            objectToolTip.GetComponent<VRTK_ObjectTooltip>().UpdateText("I'll instruct you");//AI caption - ralph           
            dialogcontrol.playSound(dialogcontrol.d2DearUser);//AI voice asking user attention -ralph
            check = true;
        }
        if (Timer >= 12)
        {
            stage++;
            check = false;
            LocalTimer = 0;
        }
    }
    void boundaryChecker()
    {
        normalEye.SetActive(true);//Normal eye enable -ralph
        var chaperone = OpenVR.Chaperone;
        
        if (check == false)
        {           
            //objectToolTip.GetComponent<VRTK_ObjectTooltip>().containerSize = new Vector2(70f, 20f);
            objectToolTip.GetComponent<VRTK_ObjectTooltip>().UpdateText("Walk towards me");//AI caption -ralph
            dialogcontrol.waitPlaySound(dialogcontrol.d3MoveTowards);//AI voice instructing user to move forward -ralph
            check = true;
        }
        if (chaperone.AreBoundsVisible() == false && LocalTimer > 10)
        {
            //AI eyes game object -ralph
            normalEye.SetActive(false);
            angryEye.SetActive(true); 
            if (check == true)
            {
                //objectToolTip.GetComponent<VRTK_ObjectTooltip>().containerSize = new Vector2(70f, 25f);                
                objectToolTip.GetComponent<VRTK_ObjectTooltip>().UpdateText("Again please... ");//AI caption -ralph
                if(DualDialogCounter == 0)//AI's voice reminding user to move forward if user did not after 10 sec -ralph
                {
                    audioTime1 = dialogcontrol.d5remindMoveOver.length + LocalTimer;
                    dialogcontrol.waitPlaySound(dialogcontrol.d5remindMoveOver);
                    DualDialogCounter = 1;
                   
                }
                if (DualDialogCounter == 1 && LocalTimer > audioTime1)
                {
                    LocalTimer = 0;
                    DualDialogCounter = 0;
                }                    
            }           
        }
        else if (chaperone.AreBoundsVisible() == true)
        {
            objectToolTip.GetComponent<VRTK_ObjectTooltip>().UpdateText("Blue lines indicate wall");
            //AI eyes game object -ralph
            normalEye.SetActive(false);//disable normal eye
            angryEye.SetActive(false);//angreye disabled
            happyEye.SetActive(true);//happy eye enabled
            //objectToolTip.GetComponent<VRTK_ObjectTooltip>().containerSize = new Vector2(110f, 25f);
            if(DualDialogCounter == 0)//AI voice telling user to be careful when boudary is seen -ralph
            {
                audioTime1 = dialogcontrol.d6BeCareful.length + LocalTimer;
                
                dialogcontrol.playSound(dialogcontrol.d6BeCareful);
                DualDialogCounter = 1;
            }
            if(DualDialogCounter == 1 && LocalTimer> audioTime1)//AI voice explaing that boundary represent wall in the real world -ralph
            {                
                dialogcontrol.waitPlaySound(dialogcontrol.d7PhysicalWall);
                stage++;
                check = false;
                LocalTimer = 0;
            }                                                                             
        }
    }
  
    void teleportChecker()//Instruct user to teleport and checks if user has indeed teleport
    {
        //AI eye game objects -ralph
        happyEye.SetActive(false);//disable happy eye
        if(teleportEyeCounter == 0)
        {
            normalEye.SetActive(true);//enable normal eye
            teleportEyeCounter = 1;
        }
        

        VRTK_BasicTeleporscript = GameControlObj.GetComponent<VRTK_BasicTeleport>();
        hasteleported = VRTK_BasicTeleporscript.IfTeleported;
        if (LocalTimer > 4 && LocalTimer<=19&&hasteleported==false)
        {
            if (check == false)
            {               
               // objectToolTip.GetComponent<VRTK_ObjectTooltip>().containerSize = new Vector2(120f, 40f);                               
               
                //Plays 3 audio clips after one another instructing user to teleport -ralph
                if (DualDialogCounter == 1)
                {
                    audioTime1 = dialogcontrol.d8teleport.length + LocalTimer;
                    audioTime2 = dialogcontrol.d9FollowInstructions.length + LocalTimer;
                    audTime3 = audioTime1 + audioTime2;
                    dialogcontrol.playSound(dialogcontrol.d8teleport);
                    DualDialogCounter = 2;
                    objectToolTip.GetComponent<VRTK_ObjectTooltip>().UpdateText("You can also teleport");//AI caption -ralph

                }
                if (DualDialogCounter == 2 && LocalTimer > audioTime1)
                {
                    dialogcontrol.playSound(dialogcontrol.d9FollowInstructions);
                    DualDialogCounter = 3;
                    objectToolTip.GetComponent<VRTK_ObjectTooltip>().UpdateText("Follow the instructions");//AI caption -ralph

                }
                if (DualDialogCounter == 3 && LocalTimer > 14)
                {
                    dialogcontrol.playSound(dialogcontrol.d10holdPressAim);
                    DualDialogCounter = 4;
                    objectToolTip.GetComponent<VRTK_ObjectTooltip>().UpdateText("Teleport: Press, Hold, \nAim, and release");
                    check = true;
                }              
            }
            controller.SetActive(true);
            trackpad.SetActive(true);
            destPoint.SetActive(true);
            trackpadHint = true;
        }
        else if(LocalTimer >30 && hasteleported == false)
        {
            
            DualDialogCounter = 2;
            if (check==true)
            {
                //objectToolTip.GetComponent<VRTK_ObjectTooltip>().containerSize = new Vector2(140f, 35f);
                dialogcontrol.playSound(dialogcontrol.d13RemindTeleport);//AI voice remainding user to teleport -ralph
                //AI eyes game objects -ralph
                normalEye.SetActive(false);//disable normal eye           
                angryEye.SetActive(true);//enable angry eye
                objectToolTip.GetComponent<VRTK_ObjectTooltip>().UpdateText("Teleport to proceed");
                				
				check = false;
            }
            LocalTimer = -4;
        }
        if(hasteleported == true)
        {
            //AI eyes game objectss -ralph
            normalEye.SetActive(false);//disable normal eye
            angryEye.SetActive(false);//disble angry eye
            happyEye.SetActive(true);//enable happy eye
            if (DualAudioClipTeleportVariable == 0)
            {
                DualDialogCounter = 5;
                DualAudioClipTeleportVariable = 1;
            }
            destPoint.SetActive(false);
            GameControlObj.GetComponent<VRTK_PolicyList>().operation = VRTK_PolicyList.OperationTypes.Ignore;
            GameControlObj.GetComponent<VRTK_PolicyList>().checkType = VRTK_PolicyList.CheckTypes.Tag;

            //objectToolTip.GetComponent<VRTK_ObjectTooltip>().containerSize = new Vector2(140f, 30f);
            if(DualDialogCounter == 5)//Plays two audio clips after one another -ralph
            {
                objectToolTip.GetComponent<VRTK_ObjectTooltip>().UpdateText("Well done!");
                audioTime1 = dialogcontrol.d11WellDone.length + LocalTimer + 0.5f;
                audioTime2 = dialogcontrol.d14TeleportSafty.length;
                audTime3 = audioTime1 + audioTime2;
                dialogcontrol.waitPlaySound(dialogcontrol.d11WellDone);
                DualDialogCounter = 6;
                
            }
            if(DualDialogCounter == 6 && LocalTimer > audioTime1)
            {
                objectToolTip.GetComponent<VRTK_ObjectTooltip>().UpdateText("Teleport Reduces Injury");
                dialogcontrol.waitPlaySound(dialogcontrol.d14TeleportSafty);
                DualDialogCounter = 7;
                
            }
            if(DualDialogCounter== 7 && LocalTimer > audTime3 )
            {
                stage++;
                check = true;
                LocalTimer = 0;
                DualDialogCounter = 8;
            }                                               
            trackpadHint = false;
			controller.SetActive (false);
			trackpad.SetActive (false);           
        }        
    }
    void objectInteractIntro()
    {
        //AI eyes game object -ralph
        happyEye.SetActive(false);//disable happy eye
        normalEye.SetActive(true);//enable normal eye
        
        controller.SetActive(true);
        trigger.SetActive(true);
        controller.transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetAngles, smooth * Time.deltaTime);
        transparentBallExit = TransparentBall.GetComponent<TransparentBallScript>();       
        objectExitedSphere = transparentBallExit.ExitTransparetSphere;

        if (LocalTimer > 1 && LocalTimer <= 20 && objectExitedSphere != true)
        {
            if (check == true)
            {
                //objectToolTip.GetComponent<VRTK_ObjectTooltip>().containerSize = new Vector2(130f, 35f);
                if (DualDialogCounter == 8)//Plays 3 audio clips after one another instructing user to interact with objects -ralph
                {
                    audioTime1 = dialogcontrol.d15UserBubbleOut.length + LocalTimer;
                    audioTime2 = dialogcontrol.d16TryItOut.length + audioTime1;
                    audTime3 = audioTime2 + dialogcontrol.d17approachBubble.length;
                    audTime4 = audTime3 + dialogcontrol.d32MoveOutOfSphere.length;
                                       
                    if (loopCounter == 0)
                    {
                        objectToolTip.GetComponent<VRTK_ObjectTooltip>().UpdateText("Interact with objects\ninside the bubble");
                        dialogcontrol.waitPlaySound(dialogcontrol.d15UserBubbleOut);
                        loopCounter = 1;
                    }
                    DualDialogCounter = 9;
                }
                if (DualDialogCounter == 9 && LocalTimer > audioTime1)
                {
                    if (loopCounter == 1)
                    {
                        dialogcontrol.waitPlaySound(dialogcontrol.d16TryItOut);
                        objectToolTip.GetComponent<VRTK_ObjectTooltip>().UpdateText("Try it out");
                        loopCounter = 2;
                    }

                    DualDialogCounter = 10;
                }
                if (DualDialogCounter == 10 && LocalTimer > audioTime2)
                {
                    dialogcontrol.waitPlaySound(dialogcontrol.d17approachBubble);
                    objectToolTip.GetComponent<VRTK_ObjectTooltip>().UpdateText("Reach out to the item");
                    DualDialogCounter = 11;
                }
                if (DualDialogCounter == 11&& LocalTimer > audTime3)
                {
                    dialogcontrol.waitPlaySound(dialogcontrol.d32MoveOutOfSphere);
                    objectToolTip.GetComponent<VRTK_ObjectTooltip>().UpdateText("Move out of sphere");
                    DualDialogCounter = 12;
                }
                if (DualDialogCounter == 12 && LocalTimer > audTime4)
                {
                    check = false;
                    DualDialogCounter = 13;
                }
                //AI caption -ralplh                       
            }
            triggerHint = true;            
        }
        else if (LocalTimer > 28 && objectExitedSphere == false)
        {
            //AI eyes game objects -ralph
            normalEye.SetActive(false);//disale normal eye
            angryEye.SetActive(true);//enable angry eye
            if (check == false)
            {
                if(loopCounter>3 && loopCounter%2==0)
                {
                    //////////////////User are you still there?
                }
                //objectToolTip.GetComponent<VRTK_ObjectTooltip>().containerSize = new Vector2(140f, 20f);
                objectToolTip.GetComponent<VRTK_ObjectTooltip>().UpdateText("Again, reach \nout to the items");
                if (DualDialogCounter == 13)//AI voice remainds user to interact with objects -ralph
                {
                    audioTime1 = dialogcontrol.d19AgainifyouMay.length + LocalTimer;
                    audioTime2 = dialogcontrol.d17approachBubble.length + LocalTimer;
                    audTime3 = audioTime1 + audioTime2;
                    dialogcontrol.waitPlaySound(dialogcontrol.d19AgainifyouMay);
                    DualDialogCounter = 14;
                }
                if (DualDialogCounter == 14 && LocalTimer > audioTime1)//AI voice instruct user to approach buble -ralph
                {
                    //objectToolTip.GetComponent<VRTK_ObjectTooltip>().containerSize = new Vector2(135f, 30f);
                    objectToolTip.GetComponent<VRTK_ObjectTooltip>().UpdateText("Press highlighted button");
                    dialogcontrol.waitPlaySound(dialogcontrol.d17approachBubble);
                    DualDialogCounter = 15;
                }
                if (DualDialogCounter == 15 && LocalTimer > audTime3)
                {
                    //DualDialogCounter = 13;
                    check = true;
                    DualDialogCounter = 7;
                    LocalTimer = 0;
                }
              
                loopCounter ++;              
            }			
        }      
        else if(objectExitedSphere == true && objectExitedSphereCounter ==0)
        {
            
            if(bballExitedCounter==0 &&bballExited == false)
            {
                TransparentBallScript x = TransparentBall.GetComponent<TransparentBallScript>();
                bballExited = x.basketBallExit;
                bballExitedCounter = 1;
            }
            //AI eyes game objects -ralph
            angryEye.SetActive(false);//disable angry eye
            normalEye.SetActive(false);//disable angry eye
            happyEye.SetActive(true);//enable angry eye

            if (localTimerReset == 0)
            {
                DualDialogCounter = 16;
                LocalTimer = 0;
                localTimerReset = 1;
            }
            triggerHint = false;
            controller.SetActive(false);
           
            //objectToolTip.GetComponent<VRTK_ObjectTooltip>().containerSize = new Vector2(140f, 40f);            
                if (DualDialogCounter == 16)//AI voice cogratulates user -ralph
                {
                    audioTime1 = dialogcontrol.d20GreatJob.length + LocalTimer;
                    audioTime2 = dialogcontrol.d25Hatsoff.length + audioTime1;
                    audTime3 = audioTime1 + audioTime2;
                    audTime4 = audTime3 + dialogcontrol.d33ShowoffSkills.length;

                    dialogcontrol.waitPlaySound(dialogcontrol.d20GreatJob);
                    objectToolTip.GetComponent<VRTK_ObjectTooltip>().UpdateText("Props to you!");
                    DualDialogCounter = 17;
                }
                if (DualDialogCounter == 17 && LocalTimer > audioTime1)
                {
                    dialogcontrol.waitPlaySound(dialogcontrol.d25Hatsoff);
                    DualDialogCounter = 18;
                    objectToolTip.GetComponent<VRTK_ObjectTooltip>().UpdateText("You've finished the basics!");
                }
                if (DualDialogCounter == 18 && LocalTimer > audTime3 && bballExited == true)
                {
                    dialogcontrol.waitPlaySound(dialogcontrol.d33ShowoffSkills);
                    DualDialogCounter = 19;
                }
                if (DualDialogCounter == 18 && LocalTimer > audTime3 && bballExited == false)
                {
                    objectExitedSphereCounter++;
                    DualDialogCounter = 19;
                    //objectToolTip.SetActive(false);
                    objectExitedSphereCounter = 1;
                    stage++;
                    bballExitedCounter = 0;
                }
                if (DualDialogCounter == 19 && LocalTimer > audTime4 && bballExited == true)
                {
                    objectExitedSphereCounter++;
                    DualDialogCounter = 19;
                    //objectToolTip.SetActive(false);
                    objectExitedSphereCounter = 1;
                    stage++;
                    bballExitedCounter = 0;
                }
        }      
    }

    void ArmagedonCountDown()//Starts a timer that will end the tutorial
    {
        ArmagedonTimer -= Time.deltaTime;
        double timer = Math.Round(ArmagedonTimer, 0);
        int min=(int)timer/60;
       
        objectToolTip.GetComponent<VRTK_ObjectTooltip>().containerColor = new Color(0.5f, 0f, 0f, 1f);
        //objectToolTip.GetComponent<VRTK_ObjectTooltip>().containerSize = new Vector2(140f, 40f);



        if(ArmagedonTimer<= 0)
        {
            RenderSettings.skybox = Arma;
            SteamVR_Fade fade = PlayerEye.GetComponent<SteamVR_Fade>();
            SteamVR_Fade.View(Color.black, 1);
            objectToolTip.GetComponent<VRTK_ObjectTooltip>().UpdateText("Good Bye!");
        }
        else
        {
            if (min < 1)
            {
                objectToolTip.GetComponent<VRTK_ObjectTooltip>().UpdateText("Armageddon Timer :\n" + timer + "sec");
            }
            else
            {
                objectToolTip.GetComponent<VRTK_ObjectTooltip>().UpdateText("Armageddon Timer :\n" + min + " min " + timer % 60 + " sec");
            }
        }
    }
    void checkBaskeballExited()//reminds user about the ring 
    {
        TransparentBallScript x = TransparentBall.GetComponent<TransparentBallScript>();
        bballExited = x.basketBallExit;
        if(bballExited == true && bballExitedCounter == 0)
        {
            dialogcontrol.playSound(dialogcontrol.d33ShowoffSkills);
            bballExitedCounter = 1;
        }

    }
}
