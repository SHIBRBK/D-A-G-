using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace SelectCharacter
{
    public class ChooseMode : MonoBehaviour
    {

        private void Start()
        {
        }
        //　キャラクターを選択した時に実行しキャラクターデータをMyGameManagerDataにセット
        public void OnSelectMode()
        {
            //コントローラー使用の場合はここを消す
            //　ボタンの選択状態を解除して選択したボタンのハイライト表示を可能にする為に実行
            EventSystem.current.SetSelectedGameObject(null);

        }
        //　キャラクターを選択した時に背景をオンにする
        public void SwitchButtonBackground(int buttonNumber)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (i == buttonNumber - 1)
                {
                    transform.GetChild(i).Find("Background").gameObject.SetActive(true);
                }
                else
                {
                    transform.GetChild(i).Find("Background").gameObject.SetActive(false);
                }
            }
        }
    }
}
