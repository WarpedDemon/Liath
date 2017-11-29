using UnityEngine;
using UnityEngine.UI;

public class VolumeHandler : MonoBehaviour 
{
    private void Start() 
	{
        if (GameObject.Find("EffectsSlider"))
        {
            GameObject.Find("EffectsSlider").GetComponent<Slider>().onValueChanged.AddListener(SetVolume);
        }
	}

    private void SetVolume(float volume)
	{
		GetComponent<AudioSource>().volume = volume;
	}

    private void OnDestroy()
	{
        if (GameObject.Find("EffectsSlider"))
        {
            GameObject.Find("EffectsSlider").GetComponent<Slider>().onValueChanged.RemoveListener(SetVolume);
        }
	}
}