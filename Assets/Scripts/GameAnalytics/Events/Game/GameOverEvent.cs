﻿using System.Collections.Generic;
using Analytics.Events;
using Analytics.Parameters;

namespace GameAnalytics.Events.Game
{
    public class GameOverEvent : AnalyticsEvent
    {
        public override string EventName => "game_over";
        public override string EventCategory => "game";

        private int _score;
        private int _highScore;
        private int _highestTileValue;
        private bool _canContinue;

        public GameOverEvent(int score, int highScore, int highestTileValue, bool canContinue)
        {
            _score = score;
            _highScore = highScore;
            _highestTileValue = highestTileValue;
            _canContinue = canContinue;
        }

        public override IEnumerable<AbstractEventParameter> GetParameters()
        {
            yield return new IntegerEventParameter("score", _score);
            yield return new IntegerEventParameter("high_score", _highScore);
            yield return new IntegerEventParameter("highest_tile_value", _highestTileValue);
            yield return new BooleanEventParameter("can_continue", _canContinue);
        }
    }
}