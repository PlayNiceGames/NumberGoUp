using Sirenix.OdinInspector.Editor.Drawers;
using UnityEngine;

namespace Tiles.Data.Editor
{
    public class TileDataArrayEditor : TwoDimensionalArrayDrawer<TileData[,], TileData>
    {
        protected override TileData DrawElement(Rect rect, TileData tile)
        {
            DrawTile(tile, rect);

            return tile;
        }

        private void DrawTile(TileData tile, Rect rect)
        {
            if (tile == null)
            {
                DrawName("null", rect);

                return;
            }

            string typeName = tile.Type.ToString();
            DrawName(typeName, rect);

            /*switch (tile)
            {
                case EmptyTileData emptyTileData:
                    SetColor(Color.white);
                    break;
                case RegularTileData regularTileData:
                    SetColor(Color.cyan);
                    break;
                case MixedTileData mixedTileData:
                    SetColor(Color.yellow);
                    break;
            }*/
        }

        private void DrawName(string name, Rect rect)
        {
            GUI.Label(rect, name);
        }

        private void SetColor(Color color)
        {
            GUI.backgroundColor = color;
        }
    }
}