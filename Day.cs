namespace VRTK.Examples
{
    using DigitalRuby.RainMaker;
    using UnityEngine;
    using UnityEventHelper;

    public class Day : MonoBehaviour
    {
        public GameObject unRainButton;
        public GameObject NightButton;
        public Material DayOriginalColor;
        public Material Green;
        public Material DayBox;
        private VRTK_Button_UnityEvents buttonEvents;
        public Material DayWithRainBox;
        public Night NightMaterial;
        public UnRain UnrainMaterial;
        public GameObject light;
    
        public GameObject cricketSound;
        public GameObject cricketSound2;
        private void Start()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Sphere")
            {
                //Uncomment if you want to return unrain button material to original when day button is toggled
                //UnrainMaterial = unRainButton.GetComponent<UnRain>();
                //UnrainMaterial.ReturnOrigTexture();

                //Change night button material to original 
                light.SetActive(true);
                NightMaterial = NightButton.GetComponent<Night>();
                NightMaterial.ReturnOrigTexture();
                GetComponent<Renderer>().material = Green;
                if (GameObject.Find("RainPrefab(Clone)") == null)
                {
                    Debug.Log("Pushed");
                    RenderSettings.skybox = DayBox;
                }
                else
                {
                    Debug.Log("Pushed");
                    RenderSettings.skybox = DayWithRainBox;
                }

                cricketSound.GetComponent<AudioSource>().enabled = false;
                cricketSound2.GetComponent<AudioSource>().enabled = false;
            }

        }
        public void ReturnOrigTexture()
        {
            GetComponent<Renderer>().material = DayOriginalColor;
        }
    }
}