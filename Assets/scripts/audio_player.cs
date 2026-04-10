using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class audio_player : MonoBehaviour {

	public bool paused;

	public AudioMixer audioMixer;
	public AudioSource sfx;
	public AudioSource music1;
	public AudioSource music2;
	public AudioSource music3;
	public AudioSource music4;
	public AudioClip[] audioClips;
	public AudioMixerSnapshot menuSnapshot;
	public AudioMixerSnapshot[] snapshots;
	public AudioMixerSnapshot noSoundSnapshot;
	public float fadeTime = 20; // 20 seconds
	public int snapshotPlaying;

	[Header("timer")]
	public float timeIntival = 20; // 20 seconds
	public float timeRemaining = 0;

	[Header("Shuffle ints for click pitches")]
	public int[] clickPitchesShuffled;
	public int shuffleLength;

	// Use this for initialization
	void Start () {
		
		timeRemaining = timeIntival;

		clickPitchesShuffled = shuffleInts(clickPitchesShuffled);
		shuffleLength = clickPitchesShuffled.Length - 1;
	}
	
	// Update is called once per frame
	void Update () {

		timeRemaining -= Time.deltaTime;
		if(timeRemaining <= 0f){

			// play another snapshot
			randSnapshot();

			// reset timer
			timeRemaining = timeIntival;
		}
	}

	int rand = 0;
	public void playAudioClip(int soundNum, float pitch = 1){

		if(!paused){
			sfx.pitch = pitch;
				
			switch (soundNum) {
			case 0:
				if(shuffleLength > 0){
					rand = clickPitchesShuffled[shuffleLength];
					shuffleLength--;
				}else{
					// 0 click
					rand = clickPitchesShuffled[shuffleLength];

					// reset
					shuffleLength = clickPitchesShuffled.Length -1;
					clickPitchesShuffled = shuffleInts(clickPitchesShuffled);
				}

				switch (rand) {
				case 1:
					sfx.pitch = 1.0595f;
					break;
				case 2:
					sfx.pitch = 1.1892f;
					break;
				case 3:
					sfx.pitch = 1.3348f;
					break;
				case 4:
					sfx.pitch = 1.5874f;
					break;
				default:
					sfx.pitch = 1;
					break;
				}
				// tile click sound
				sfx.PlayOneShot(audioClips[0], 0.4f);
				break;
			case 1:
				// start level sound
				sfx.PlayOneShot(audioClips[1], 0.5f);
				break;
			case 2:
				// restart button sound
				sfx.PlayOneShot(audioClips[2], 0.7f);
				break;
			case 3:
				// level win sound
				sfx.PlayOneShot(audioClips[3], 0.9f);
				break;
			case 4:
				// general button sound
				sfx.PlayOneShot(audioClips[4], 0.6f);
				break;
			case 5:
				// general button sound
				sfx.PlayOneShot(audioClips[5], 0.9f);
				break;
			case 6:
				// general button sound
				sfx.PlayOneShot(audioClips[6], 0.9f);
				break;
			case 7:
				// general button sound
				sfx.PlayOneShot(audioClips[7], 0.9f);
				break;
			case 8:
				// general button sound
				sfx.PlayOneShot(audioClips[8], 0.9f);
				break;
			default:
				break;
			}
		}
	}

	public void pauseAudio(){

		noSoundSnapshot.TransitionTo(0.3f);

		StartCoroutine(pauseAudioSources());

		paused = true;
	}

	IEnumerator pauseAudioSources() {

		yield return new WaitForSeconds(0.3f);

		music1.Pause();
		music2.Pause();
		music3.Pause();
		music4.Pause();
	}

	public void playAudio(){
		
		music1.Play();
		music2.Play();
		music3.Play();
		music4.Play();

//		noSoundSnapshot.TransitionTo(0f);
		randSnapshot(1f);

		paused = false;
	}

	public void randSnapshot(float t = 0){

		if(t == 0 ) t = fadeTime;

		int rand = Random.Range(0, snapshots.Length);
		snapshots[rand].TransitionTo(t);

		snapshotPlaying = rand;
	}

	// transition to menu audio mixer snapshot
	public void lowpassFilterEnable(){
		menuSnapshot.TransitionTo(2);
	}


	// set random object
	// fisher-yates shuffle with ints
	public System.Random ran = new System.Random();
	int[] shuffleInts(int[] ints)
	{
		for (int i = ints.Length - 1; i > 0; i--)
		{
			int j = ran.Next(i + 1);

			int temp = ints[i];
			ints[i] = ints[j];
			ints[j] = temp;
		}
		return ints;
	}
}
