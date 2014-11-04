using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    public AudioMixer master;
    public bool GraphValue = false;
    public bool DisplaySphere = true;
    public AudioSource music, sfx, pellets, sfxBypassed;
    public static AudioPlayer instance;
    public AmplifyColorEffect colorAmp;
    GlowEffect glow;

    public int sampleSize = 2048;
    float[] samples;
    float visualizedAmplitude = 0;
    float amplitude = 0;

    public float defaultHighPass = 10, setHighPass = 850, defaultLowPass = 22000, setLowPass = 800;
    float targetHighPass, targetLowPass;
    float currentHighPass, currentLowPass;

    public AudioClip pellet, powerup, jump, land, explosion, died;

	private HappyHippo Charlie;

    void Awake()
    {
        //Util.Settings.DisableUnitility();
        //Util.Settings.DrawLog = false;
        glow = GameObject.FindObjectOfType<GlowEffect>();
        targetLowPass = setLowPass;
        currentLowPass = setLowPass;
        targetHighPass = defaultHighPass;
        currentHighPass = defaultHighPass;

        instance = this;
        samples = new float[1024];

		Charlie = GameObject.FindObjectOfType<HappyHippo> ();
    }
    // Update is called once per frame
    void Update()
    {
        if (sampleSize != samples.Length)
            samples = new float[1024];

        music.GetOutputData(samples, 0);

        visualizedAmplitude = Mathf.Lerp(visualizedAmplitude, 0, Util.deltaTime * 10);

        amplitude = Util.Math.AverageAbs(samples);
        if (amplitude > visualizedAmplitude)
            visualizedAmplitude = amplitude;

        colorAmp.BlendAmount = visualizedAmplitude*2f;

        currentHighPass = Mathf.Lerp(currentHighPass, targetHighPass, Util.deltaTime * 5);
        currentLowPass = Mathf.Lerp(currentLowPass, targetLowPass, Util.deltaTime * 5);
        glow.glowTint = Color.Lerp(Color.black, new Color(0.5f, 0.5f, 0.5f, 0), Mathf.InverseLerp(defaultHighPass, setHighPass, currentHighPass));

        master.SetFloat("HighPass", currentHighPass);
        master.SetFloat("LowPass", currentLowPass);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            EnterHighPass();
        if (Input.GetKeyDown(KeyCode.DownArrow))
            ExitHighPass();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfx.PlayOneShot(clip);
    }

    public void PlaySFXBypassed(AudioClip clip)
    {
        sfxBypassed.PlayOneShot(clip);
    }

    public void EnterHighPass()
    {
        targetHighPass = setHighPass;
    }

    public void ExitHighPass()
    {
        targetHighPass = defaultHighPass;
    }

    public void EnterLowPass()
    {
        targetLowPass = setLowPass;
    }

    public void ExitLowPass()
    {
        targetLowPass = defaultLowPass;
    }

    float lastPickUp;
    float pickUpPitch = 0.6f;
    float combotTime = 1;
    public void PickUp()
    {
        if (Time.time - lastPickUp < combotTime)
            pickUpPitch += 0.1f;
        else
            pickUpPitch = 0.6f;
        pellets.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", pickUpPitch);
        pellets.PlayOneShot(pellet);
        lastPickUp = Time.time;

		Charlie.Poke ((int)((pickUpPitch-0.6f)*10f));
    }

    public void PowerUp()
    {
        PlaySFX(powerup);
		Charlie.Poke (10);
    }

    public void Explosion()
    {
        PlaySFXBypassed(explosion);
		Charlie.Poke (20);
    }

    public void Jump()
    {
        PlaySFX(jump);
    }

    public void Land()
    {
        PlaySFX(land);
    }

    public void YouLose()
    {
        PlaySFXBypassed(died);
    }
}
