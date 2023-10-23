﻿using System;
using Camera;
using JetBrains.Annotations;
using UnityEngine;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        [HideInInspector] [CanBeNull] public LevelController levelController = null;

        public bool debugLevel;

        public GameData data;
        public CameraController cameraController;


        private AudioSource _audioSource;
        private const string LevelPrefPath = "Level/Level_";

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            NextLevel();
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M)) _audioSource.mute = !_audioSource.mute;
        }

        private void NextLevel()
        {
            _audioSource.Play();
            var levelNumber = data.currentLevel;
            if (levelController is not null) Destroy(levelController.gameObject);
            // data.Reset();
            data.currentLevel = levelNumber;
            var levelName = LevelPrefPath + levelNumber;
            var levelPref = Resources.Load<GameObject>(levelName);
            levelController = Instantiate(levelPref).GetComponent<LevelController>();
        }
    }
}