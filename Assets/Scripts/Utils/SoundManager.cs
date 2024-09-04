using UnityEngine;
using System.Collections;

namespace DotsTriangle.Utils
{
    /// <summary>
    /// This persistent singleton handles sound playing
    /// </summary>

    public class SoundManager : Persistent<SoundManager>
    {
        public const string MUSIC_VOLUME = "MUSIC_VOLUME";
        public const string SOUND_VOLUME = "SOUND_VOLUME";
        public const string MUSIC_ON = "MUSIC_ON";
        public const string SOUND_ON = "SOUND_VOLUME";

        public ISaveData data;
        /// true if the music is enabled	
        private bool musicOn = true;
        /// true if the sound fx are enabled
        private bool sfxOn = true;
        /// the music volume
        [Range(0, 1)]
        [SerializeField]
        private float _musicVolume = 0.3f;
        /// the sound fx volume
        [Range(0, 1)]
        [SerializeField]
        private float _sfxVolume = 1f;

        protected AudioSource _backgroundMusic;

        public float MusicVolume
        {
            get
            {
                return _musicVolume;
            }

            set
            {
                _musicVolume = value;
                data.SetKey(MUSIC_VOLUME, _musicVolume);
                if (_backgroundMusic != null)
                    _backgroundMusic.volume = _musicVolume;
            }
        }

        public float SfxVolume
        {
            get
            {
                return _sfxVolume;
            }

            set
            {
                _sfxVolume = value;
                data.SetKey(SOUND_VOLUME, _sfxVolume);
            }
        }

        public bool MusicOn
        {
            get
            {
                return musicOn;
            }

            set
            {
                musicOn = value;
                data.SetKey(MUSIC_ON, musicOn ? 1 : 0);

                if (!musicOn && _backgroundMusic != null)
                    _backgroundMusic.Stop();
                if (musicOn && _backgroundMusic != null)
                    _backgroundMusic.Play();
            }
        }

        public bool SfxOn
        {
            get
            {
                return sfxOn;
            }

            set
            {
                sfxOn = value;
                data.SetKey(SOUND_ON, sfxOn ? 1 : 0);
            }
        }

        public override void Awake()
        {
            base.Awake();

            int mOn = 1, sOn = 1;
            data.GetKey(MUSIC_ON, ref mOn, 1);
            data.GetKey(SOUND_ON, ref sOn, 1);
            MusicOn = mOn == 1;
            SfxOn = sOn == 1;


            data.GetKey(SOUND_VOLUME, ref _sfxVolume, _sfxVolume);
            data.GetKey(MUSIC_VOLUME, ref _musicVolume, _musicVolume);

            //SfxVolume = 0.8f;
        }

        /// <summary>
        /// Plays a background music.
        /// Only one background music can be active at a time.
        /// </summary>
        /// <param name="Clip">Your audio clip.</param>
        public virtual void PlayBackgroundMusic(AudioSource Music)
        {
            if (_backgroundMusic != null && _backgroundMusic != Music)
                Destroy(_backgroundMusic.gameObject);

            // we set the background music clip
            _backgroundMusic = Music;

            // if the music's been turned off, we do nothing and exit
            if (!MusicOn)
                return;
            // if we already had a background music playing, we stop it
            if (_backgroundMusic != null)
            {
                _backgroundMusic.Stop();
            }


            // we set the music's volume
            _backgroundMusic.volume = MusicVolume;
            // we set the loop setting to true, the music will loop forever
            _backgroundMusic.loop = true;
            // we start playing the background music
            _backgroundMusic.Play();
        }

        public virtual void PlayBackgroundMusic(AudioClip music, Vector3 location)
        {
            if (music == null)
            {
                Debug.LogError("No AudioClip, Sir!: SoundManager.PlayBackgroundMusic");
                return;
            }

            // we create a temporary game object to host our audio source
            GameObject temporaryAudioHost = new GameObject("Music");
            // we set the temp audio's position
            temporaryAudioHost.transform.position = location;
            // we add an audio source to that host
            AudioSource audioSource = temporaryAudioHost.AddComponent<AudioSource>() as AudioSource;
            // we set that audio source clip to the one in paramaters
            audioSource.clip = music;
            // we set the audio source volume to the one in parameters
            audioSource.volume = MusicVolume;
            audioSource.spatialBlend = 0;
            // play music
            PlayBackgroundMusic(audioSource);
        }

        /// <summary>
        /// Plays a sound
        /// </summary>
        /// <returns>An audiosource</returns>
        /// <param name="Sfx">The sound clip you want to play.</param>
        /// <param name="Location">The location of the sound.</param>
        /// <param name="Volume">The volume of the sound.</param>
        public virtual AudioSource PlaySound(AudioClip Sfx, Vector3 Location)
        {
            if (!SfxOn)
                return null;
            // we create a temporary game object to host our audio source
            GameObject temporaryAudioHost = new GameObject("TempAudio");
            // we set the temp audio's position
            temporaryAudioHost.transform.position = Location;
            // we add an audio source to that host
            AudioSource audioSource = temporaryAudioHost.AddComponent<AudioSource>() as AudioSource;
            // we set that audio source clip to the one in paramaters
            audioSource.clip = Sfx;
            // we set the audio source volume to the one in parameters
            audioSource.volume = SfxVolume;
            // we start playing the sound
            audioSource.Play();
            // we destroy the host after the clip has played
            Destroy(temporaryAudioHost, Sfx.length);
            // we return the audiosource reference
            return audioSource;
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus)
            {
                if (_backgroundMusic != null)
                {
                    if (MusicOn)
                        _backgroundMusic.Play();
                    else
                        _backgroundMusic.Stop();
                }
                    
                    
            }

        }

        void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                if (_backgroundMusic != null)
                {
                    if (MusicOn)
                        _backgroundMusic.Play();
                    else
                        _backgroundMusic.Stop();
                }
                    
            }
            else
            {
                if (_backgroundMusic != null)
                    _backgroundMusic.Stop();
            }
        }
    }

}