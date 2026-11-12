using System.Collections.Generic;
using _TevLib.SoundSystem;
using _TevLib.SoundSystem.Runtime;
using UnityEngine;

namespace _TevLib.ServiceLocatorSystem.AudioService
{
    public class AudioService : MonoBehaviour , IAudioService
    {
        [SerializeField] private SoundPlayer soundPlayerPrefab;

        private Dictionary<int, SoundPlayer> _playerDict = new();

        private SoundPlayer _bgmPlayer;

        private void Awake()
        {
            ServiceLocator.RegisterService<IAudioService>(this);
            _bgmPlayer = Instantiate(soundPlayerPrefab, transform);
        }

        private void Start()
        {
            _bgmPlayer.name = "BgmPlayer";
        }

        private void OnDestroy()
        {
            ServiceLocator.RegisterService<IAudioService>(new NullAudioService());
        }

        public void PlaySfx(SoundClipSO clipData, int channel = 0)
        {
            SoundPlayer sfxPlayer = Instantiate(soundPlayerPrefab, transform);
            sfxPlayer.PlaySound(clipData);

            sfxPlayer.OnSoundFinished += HandleSoundFinish;

            if (channel > 0)
            {
                if (_playerDict.TryGetValue(channel, out SoundPlayer oldPlayer))
                {
                    oldPlayer.ForceSoundStop();
                    SetDisableSoundPlayer(oldPlayer);
                    _playerDict.Remove(channel);
                }
                
                _playerDict[channel] = sfxPlayer;
            }
        }

        private void SetDisableSoundPlayer(SoundPlayer player)
            => Destroy(player.gameObject);

        private void HandleSoundFinish(SoundPlayer player)
        {
            player.OnSoundFinished -= HandleSoundFinish;
            SetDisableSoundPlayer(player);
        }

        public void StopSfx(int channel = 0)
        {
            if (_playerDict.TryGetValue(channel, out SoundPlayer player))
            {
                player.ForceSoundStop();
                SetDisableSoundPlayer(player);
            }
        }

        public void PlayBgm(SoundClipSO bgmSound)
        {
            _bgmPlayer.ForceSoundStop();
            _bgmPlayer.PlaySound(bgmSound);
        }

        public void StopBgm()
        {
            _bgmPlayer.ForceSoundStop();
        }
    }
}