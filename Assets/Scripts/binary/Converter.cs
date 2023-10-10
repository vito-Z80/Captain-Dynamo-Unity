using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace binary
{
    public class Converter
    {
        private readonly Color _pixelColor = Color.white;
        private readonly Color _clearColor = Color.clear;


        public void BinToPng(string srcPath, string dstPath)
        {
            var list = GetFilesList(srcPath);

            foreach (var fileName in list)
            {
                var bin = LoadBinary(srcPath, fileName);
                var size = Utils.ExtractNumbers(fileName);
                if (size.Count != 2)
                    throw new Exception(
                        "The file name must contain the image dimensions in pixels. (tmp32x48.bin). First number = width, Second = height");
                var texture = GetTexture2D(bin, size[0], size[1]);
                CreatePng(dstPath, fileName + ".png", texture);
            }
        }


        public  byte[] LoadBinary(string path, string fileName)
        {
            try
            {
                var bytes = File.ReadAllBytes(path + fileName);
                return bytes;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private List<bool> GetBoolPixels(byte[] bytes)
        {
            var pixels = new List<bool>();
            foreach (var b in bytes)
            {
                for (var i = 0; i < 8; i++)
                {
                    pixels.Add((b << i & 0x80) != 0);
                }
            }

            return pixels;
        }

        public Texture2D GetTexture2D(byte[] bytes, int width, int height)
        {
            var count = 0;
            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point
            };
            var pixels = GetBoolPixels(bytes);
            for (var y = height - 1; y >= 0; y--)
            {
                for (var x = 0; x < width; x++)
                {
                    texture.SetPixel(x, y, pixels[count++] ? _pixelColor : _clearColor);
                }
            }

            texture.Apply();
            return texture;
        }

        public void CreatePng(string path, string name, Texture2D texture2D)
        {
            Directory.CreateDirectory(path);
            var filePath = Path.Combine(path, name);
            var bytes = texture2D.EncodeToPNG();
            File.WriteAllBytes(filePath, bytes);
        }

        public List<string> GetFilesList(string directoryName)
        {
            try
            {
                return Directory.GetFiles(directoryName).Where(s => !s.Contains(".meta")).Select(Path.GetFileName)
                    .ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}