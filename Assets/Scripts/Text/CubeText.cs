using System.Collections.Generic;
using UnityEngine;

namespace Text
{
    internal record Letter
    {
        public Vector3[] Points;

        public Letter(Vector3[] points)
        {
            Points = points;
        }
    }


    public class CubeText : MonoBehaviour
    {
        private Texture2D _fontAtlas;


        public Mesh cubeMesh; // Меш куба.
        public Material cubeMaterial; // Материал с поддержкой GPU Instancing.
        private Matrix4x4[] matrices; // Массив матриц позиций и масштабов для экземпляров.


        private string text = "!!!Hello world[] '";
        private Letter[] _letters;
        private Vector3 _scale = new Vector3(1f, 1f, 1f);

        private void Start()
        {
            _fontAtlas = Resources.Load<Texture2D>("Font/CaptainDynamoFontZxSpectrum");
            CreateAlphabet();

            var mat = new List<Matrix4x4>();
            var cubes = 0;
            var x = 0.0f;

            foreach (var c in text.ToUpper())
            {
                var id = c - '!';
                if (id >= 0)
                {
                    var letter = _letters[id];
                    foreach (var point in letter.Points)
                    {
                        var position = point + Vector3.right * x + transform.position;
                        mat.Add(Matrix4x4.TRS(position, Quaternion.identity, _scale));
                        cubes++;
                    }
                }

                x += 9;
            }

            Debug.Log("Text Cubes: " + cubes);
            matrices = mat.ToArray();
        }


        private float _time = 0.0f;
        private void Update()
        {
            Graphics.DrawMeshInstanced(cubeMesh, 0, cubeMaterial, matrices);

            var shift = Mathf.Sin(_time) / 8f;
            Vector3 pos;
            for (int i = 0; i < matrices.Length; i++)
            {
                pos = matrices[i].GetPosition() + Vector3.left* shift;
                matrices[i] = Matrix4x4.TRS(pos, Quaternion.identity, _scale);
            }

            _time += Time.deltaTime;
        }


        private void CreateAlphabet()
        {
            var font = new List<Letter>();
            for (int i = '!'; i < 96; i++)
            {
                var points = CreateLetter((char)i);
                font.Add(new Letter(points));
            }

            _letters = font.ToArray();
        }


        private Vector3[] CreateLetter(char c)
        {
            var list = new List<Vector3>();
            var topY = _fontAtlas.height - 8 - (c - '!') * 8;
            for (int y = topY; y < topY + 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    var pixel = _fontAtlas.GetPixel(x, y);
                    if (pixel.a > 0.0f) list.Add(new Vector3(x, (y & 7), 0.0f));
                }
            }

            return list.ToArray();
        }
    }
}