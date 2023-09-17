using System;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Utils
{
    public class ScreenshotMaker : MonoBehaviour
    {
        [SerializeField] private string _screenshotFolder;

        [Button]
        public void Capture()
        {
            string folderName = GetFolderName();

            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            Vector2Int resolution = GetResolution();

            string fileName = GetFilename(folderName, resolution);

            try
            {
                ScreenCapture.CaptureScreenshot(fileName);

                Debug.Log($"Captured screenshot to: {fileName}");

                EditorUtility.RevealInFinder(fileName);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private string GetFolderName()
        {
            string imagesRootDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            return Path.Combine(imagesRootDirectory, _screenshotFolder);
        }

        private string GetFilename(string folderName, Vector2Int resolution)
        {
            DateTime time = DateTime.Now;
            string fileName = $"{resolution} {time:yyyy-dd-M--HH-mm-ss}.png";

            return Path.Combine(folderName, fileName);
        }

        private Vector2Int GetResolution()
        {
            Vector2 size = Handles.GetMainGameViewSize();
            Vector2Int sizeInt = new Vector2Int((int)size.x, (int)size.y);

            return sizeInt;
        }
    }
}