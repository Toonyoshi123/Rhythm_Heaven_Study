using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    // The conductor keeps the game in sync with the music.
    // By using the song-position as a measurement for everything.

    public float songBPM;
    public float secPerBeat;

    public float songPosition;
    public float songPositionInBeats;
    private float dspSongTime;

    public float firstBeatOffset;

    public float beatsPerLoop;
    public int completedLoops = 0;
    public float loopPositionInBeats;
    public float loopPositionInAnalog;

    public static Conductor instance;
    public AudioSource musicSource;

    private void Awake()
    {
        instance = this;
    }

    // Setup.
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        secPerBeat = 60f / songBPM;
        dspSongTime = (float)AudioSettings.dspTime;
        musicSource.Play();
    }

    // Updates song position, and beat and loop variables.
    void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);
        songPositionInBeats = songPosition / secPerBeat;

        // Loops are necessary for looping animations.
        if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
        {
            completedLoops++;
        }
        loopPositionInBeats = songPositionInBeats - completedLoops * beatsPerLoop;
        loopPositionInAnalog = loopPositionInBeats / beatsPerLoop;
    }
}
