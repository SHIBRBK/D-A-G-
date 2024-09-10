using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SelectCharacter
{
    public class SelectModeScene : MonoBehaviour
    {

        private void Start()
        {
            
        }

        public void GoToVSScene()
        {
            SceneManager.LoadScene("SelectStageScene");
        }

        public void GoToSoloScene()
        {
            SceneManager.LoadScene("SoloGameScene");
            SoundManager.Instance.PlayBGM("wanderer");
        }
    }
}
