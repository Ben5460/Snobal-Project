namespace VRTK.Examples
{
    using UnityEngine;
    using UnityEventHelper;

    public class Night : MonoBehaviour
    {

        public Material NightOriginalColor;
        public Material Green;
        public Material NightBox;
        public GameObject DayButton;
        public Day dayMaterial;
        public GameObject light;
        public GameObject CricketSound;
        public GameObject CricketSound2;
        public GameObject zombie;
        //public AudioClip night;
        //public void playSound(AudioClip dialogue) //Stop current audio being played and play the next audio - Ralph
        //{
        //    AudioSource audio = GetComponent<AudioSource>();
        //    audio.Stop();
        //    audio.clip = dialogue;
        //    audio.volume = 2.0f;
        //    audio.Play();
        //    audio.loop = true;
        //}
        //private void Start()
        //{
        //    playSound(night);
        //}

        void OnTriggerEnter(Collider other)
        {


            if (other.gameObject.name == "Sphere")
            {
                dayMaterial = DayButton.GetComponent<Day>();
                dayMaterial.ReturnOrigTexture();
                GetComponent<Renderer>().material = Green;
                RenderSettings.skybox = NightBox;
                light.SetActive(false);

                CricketSound.GetComponent<AudioSource>().enabled = true;
                CricketSound2.GetComponent<AudioSource>().enabled = true;

                zombie.GetComponent<ZombieController>().enabled = true;

            }
        }
        public void ReturnOrigTexture()
        {
            GetComponent<Renderer>().material = NightOriginalColor;
        }

    }
}