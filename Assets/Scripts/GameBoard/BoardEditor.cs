using Sirenix.OdinInspector;
using UnityEngine;

namespace GameBoard
{
    public class BoardEditor : MonoBehaviour
    {
        public BoardData Data;

        [SerializeField] private Board _board;

        [Button, DisableInEditorMode]
        public void SerializeBoard()
        {
            Data = _board.GetData();
        }

        [Button, DisableInEditorMode]
        public void DeserializeBoard()
        {
            _board.SetData(Data);
        }
    }
}