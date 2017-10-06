namespace VRTK.Examples
{

    using UnityEngine;
    using UnityEventHelper;
    using VRTK;
    public class UnRain : MonoBehaviour
    {
        public GameObject RainButton;
        public Material UnrainOriginalColor;
        public Material Green;
        public RainButton RainButtonMaterial;

        public GameObject Cam_Head;//For audio source rain ambient purpose
        private void Start()
        {
            
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Sphere")
            {
                RainButtonMaterial=RainButton.GetComponent<RainButton>();
                RainButtonMaterial.ReturnOrigTexture();
                GetComponent<Renderer>().material = Green;


                if (GameObject.Find("RainPrefab(Clone)") != null)
                {
                    Debug.Log("Pushed");
                    Destroy(GameObject.Find("RainPrefab(Clone)"));
                }

                Cam_Head.GetComponent<AudioSource>().enabled = false;

                //rain.SetActive(false);
            }

        }
        public void ReturnOrigTexture()
        {
            GetComponent<Renderer>().material = UnrainOriginalColor;
        }

    }
}