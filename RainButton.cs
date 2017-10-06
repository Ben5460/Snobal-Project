namespace VRTK.Examples
{
    using DigitalRuby.RainMaker;
    using UnityEngine;
    using UnityEventHelper;

    public class RainButton : MonoBehaviour
    {
        public Material green;
        public Material RainOriginalColor;

        public GameObject UnrainButton;
        public UnRain unRainMaterial;
        //public AudioClip RainSound;

        public GameObject rain;
        public Material RainBox;

        public GameObject Cam_Head;//for rain audio source purposes

        private void Start()
        {
       

        }
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Sphere")
            {
               

                if (GameObject.Find("RainPrefab(Clone)") == null)
                {
                    //Debug.Log("Pushed");
                    Instantiate(rain);
                    if (RenderSettings.skybox.name != "StarSkyBox")
                    {
                        RenderSettings.skybox = RainBox;
                    }

                }
                unRainMaterial = UnrainButton.GetComponent<UnRain>();
                unRainMaterial.ReturnOrigTexture();
                GetComponent<Renderer>().material = green;

                //enable rain ambient sound attached to the camera head
                Cam_Head.GetComponent<AudioSource>().enabled = true;

            }

        }
        public void ReturnOrigTexture()
        {
            GetComponent<Renderer>().material = RainOriginalColor;
        }

    }
}