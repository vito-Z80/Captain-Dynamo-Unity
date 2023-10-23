using System;
using System.Collections.Generic;
using System.Linq;
using Game.Platforms;
using UnityEngine;
using AnimationState = Animations.AnimationState;

namespace Game
{
    /// <summary>
    /// 
    /// </summary>
    public class LevelController : MonoBehaviour
    {
        public GameData gameData;


        private GameObject _levelEntryPoint;
        private GameObject _levelExitPoint;

        //
        public GameObject respawnPoints;
        public TeleportController teleportController;
        public GameObject platforms;

        private List<Vector3> _respawnPointsList = new List<Vector3>();
        private Zipline[] _ziplines;


        private HeroController _heroController;

        private void Start()
        {
            // TeleportController.LevelCompleted += () => { Destroy(gameObject); };
            LevelLaunch();
        }


        private void LevelLaunch()
        {
            FindRespawnPoints();
            FindTeleports();
            FindZiplines();
            _heroController ??= FindObjectOfType<HeroController>(true);
            _heroController.gameObject.SetActive(false);
            _heroController.levelController = this;
            teleportController.StartLevel(_heroController);
            gameData.diamondsOnLevel = GetDiamondsOnLevel();
        }

        public void RestoreZiplines()
        {
            foreach (var zipline in _ziplines)
            {
                zipline.RestorePosition();
            }
        }


        public void LevelCompleted()
        {
            _heroController.isActive = false;
            _heroController.animationSprite.SetState(AnimationState.Idle);
            _heroController._rb.velocity = Vector3.zero;
            _heroController.SetStayPosition(teleportController.finishTeleport.transform.position + Vector3.up * 16.0f);
            teleportController.FinishLevel(_heroController);
        }


        private void FindZiplines()
        {
            _ziplines = platforms.GetComponentsInChildren<Zipline>();
        }

        private void FindRespawnPoints()
        {
            _respawnPointsList = respawnPoints.GetComponentsInChildren<Transform>().Select(t => t.position)
                .OrderBy(pos => pos.y).ToList();
            _respawnPointsList.Remove(respawnPoints.transform.position);
            respawnPoints.SetActive(false);
        }

        private void FindTeleports()
        {
            var teleports = GameObject.FindGameObjectsWithTag("Platform");
            _levelEntryPoint =
                teleports.First(o => o.gameObject.name.Contains("entry", StringComparison.OrdinalIgnoreCase));
            _levelExitPoint =
                teleports.First(o => o.gameObject.name.Contains("exit", StringComparison.OrdinalIgnoreCase));
        }

        public Vector3 GetRespawnPosition(Vector3 maxHeroPosition)
        {
            //  TODO если смерть будет после проходе нексольких чекпоинтов то респаун будет с первого. попраить.
            if (_respawnPointsList.Count == 1) return _respawnPointsList[0];

            var count = _respawnPointsList.Count;

            while (--count >= 0)
            {
                if (maxHeroPosition.y > _respawnPointsList[count].y) break;
            }

            _respawnPointsList.RemoveRange(0, count);
            // if (maxHeroPositoin.y > _respawnPointsList[1].y) _respawnPointsList.RemoveAt(0);
            return _respawnPointsList[0];
        }


        private int GetDiamondsOnLevel()
        {
            var count = GetComponentsInChildren<Transform>().Count(o => o.name.Contains(Define.Diamond));
            return count;
        }
    }
}