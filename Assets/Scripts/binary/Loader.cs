using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace binary
{
    public class Loader : MonoBehaviour
    {
        // public GameObject cube;

        private const string ResourcesPath = "Assets/Resources/";
        private const string SourcesPath = "Assets/gfx/bin/";
        private const string ImgDstPath = "images";


        public static byte[] LoadBinary(string fileName)
        {
            try
            {
                var bytes = File.ReadAllBytes(ResourcesPath + fileName);
                return bytes;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        void Start()
        {

            return;
            var conv = new Converter();
            conv.BinToPng(SourcesPath, ImgDstPath);
            var name = "wasp48x40.bin";
            var bytes = LoadBinary(name);
            var size = Utils.ExtractNumbers(name);
            var texture = conv.GetTexture2D(bytes, size[0], size[1]);
            conv.CreatePng(ImgDstPath, name + ".png", texture);
            GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, 48, 40), Vector2.zero);

            return;


            // BuildCubeSprite();

            var list = Utils.ExtractNumbers("wasp48x40.bin");
            Debug.Log(string.Join(", ", list));


            foreach (var s in conv.GetFilesList(SourcesPath))
            {
                Debug.Log(s);
            }
        }

        public Mesh cubeMesh; // Меш куба.
        public Material cubeMaterial; // Материал с поддержкой GPU Instancing.
        public int numInstances = 10; // Количество экземпляров.

        private Matrix4x4[] matrices; // Массив матриц позиций и масштабов для экземпляров.

        private void BuildCubeSprite()
        {
            matrices = new Matrix4x4[numInstances];
            for (int i = 0; i < numInstances; i++)
            {
                Vector3 position = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f),
                    Random.Range(-10f, 10f));
                Vector3 scale = new Vector3(Random.Range(0.5f, 2.0f), Random.Range(0.5f, 2.0f),
                    Random.Range(0.5f, 2.0f));
                matrices[i] = Matrix4x4.TRS(position, Quaternion.identity, scale);
            }

            Graphics.DrawMeshInstanced(cubeMesh, 0, cubeMaterial, matrices);
        }

        private void Update()
        {
            // Graphics.DrawMeshInstanced(cubeMesh, 0, cubeMaterial, matrices);
        }
    }
}