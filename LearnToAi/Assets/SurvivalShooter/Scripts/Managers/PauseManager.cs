using UnityEngine;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseManager : MonoBehaviour 
{
	public AudioMixerSnapshot paused;
	public AudioMixerSnapshot unpaused;
	private Canvas _canvas;

    private void Start()
	{
		_canvas = GetComponent<Canvas>();
	}

    private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			_canvas.enabled = !_canvas.enabled;
			Pause();
		}
	}
	
	public void Pause()
	{
		Time.timeScale = Time.timeScale == 0 ? 1 : 0;
		Lowpass();
	}

    private void Lowpass()
	{
		if (Time.timeScale == 0)
		{
			paused.TransitionTo(.01f);
		}
		else
		{
			unpaused.TransitionTo(.01f);
		}
	}
	
	public void Quit()
	{
		#if UNITY_EDITOR 
		EditorApplication.isPlaying = false;
		#else 
		Application.Quit();
		#endif
	}
}
