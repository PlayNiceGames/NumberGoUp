using Cysharp.Threading.Tasks;
using GameBoard;
using GameBoard.Rules;
using GameOver;
using GameScore;
using UnityEngine;

namespace GameLoop.EndlessMode
{
    public class EndlessModeGameLoop : AbstractGameLoop
    {
        [SerializeField] private GameLoopSettings _settings;
        [SerializeField] private BoardGameLoop _boardLoop;
        [SerializeField] private Board _board;
        [SerializeField] private ScoreSystem _scoreSystem;
        [SerializeField] private GameOverUI _gameOverUI;

        private BoardRules _boardRules;
        private GameOverController _gameOver;

        public override void Setup()
        {
            _boardLoop.Setup();

            _gameOver = new GameOverController(_gameOverUI, _board, _scoreSystem, _settings.GameOverSettings);

            _gameOver.Setup();
        }

        public override async UniTask Run()
        {
            while (true)
            {
                await _boardLoop.Run();

                bool shouldEndGame = await _gameOver.TryProcessGameOver();

                if (shouldEndGame)
                {
                    Debug.LogError("GAME OVER");
                    return;
                }
            }
        }
    }
}