using Cysharp.Threading.Tasks;
using GameAds;
using GameLoop;
using ServiceLocator;
using UnityEngine;

namespace GameConsumables
{
    public class GameRewindButton : GameButton
    {
        private AdsService _adsService;

        protected override void Awake()
        {
            base.Awake();

            _adsService = GlobalServices.Get<AdsService>();
        }

        public async UniTask<bool> ProcessInput()
        {
            if (Application.isEditor)
            {
                await _adsService.ShowAdUnavailableMessage();

                return true;
            }

            RewardedAdShowResult adShowResult = await _adsService.ShowRewardedAd("rewind");

            switch (adShowResult)
            {
                case RewardedAdShowResult.Unavailable:
                    await _adsService.ShowAdUnavailableMessage();
                    return false;
                case RewardedAdShowResult.Rewarded:
                    return true;
            }

            return false;
        }
    }
}