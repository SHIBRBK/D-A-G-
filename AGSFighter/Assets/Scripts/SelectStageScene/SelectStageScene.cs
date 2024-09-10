using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SelectCharacter
{
    public class SelectStageScene : MonoBehaviour
    {
        public GameObject loadingSpinner; // ローディングスピナーの参照

        private void Start()
        {
            // ローディングスピナーを初期状態では非表示に設定
            loadingSpinner.SetActive(false);

            if (RoundManager.Instance == null)
            {
                Debug.LogError("RoundManagerのインスタンスが見つかりません。シーンに配置されていることを確認してください。");
            }
        }

        public void GoToOtherScene(string stage)
        {
            // 次のシーンデータをRoundManagerに保存
            if (RoundManager.Instance != null)
            {
                //選ばれたステージをセット
                RoundManager.Instance.SetSelectedStage(stage);
            }
            else
            {
                Debug.LogError("RoundManagerのインスタンスがありません");
            }

            // 非同期読み込みを開始
            StartCoroutine(LoadSceneAsync(stage));
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            // ローディングスピナーを表示
            loadingSpinner.SetActive(true);

            // シーンの非同期読み込みを開始
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

            // 読み込みが完了するまで待つ
            while (!asyncOperation.isDone)
            {
                yield return null;
            }

            // 読み込みが完了したらローディングスピナーを非表示にする
            loadingSpinner.SetActive(false);
        }
    }
}
