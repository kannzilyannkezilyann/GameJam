/**
 * @file   TitleUIManager.cs
 *
 * @brief  タイトルシーンのUIの動きに関するヘッダファイル
 *
 * @author 制作者名　深谷翔太
 *
 * @date   日付　2026/02/03
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{
    //現在のセレクト番号
    int m_select = 0;
    //遷移先のシーン
    [SerializeField] private string m_scene;
    //選択肢のオブジェクト
    [SerializeField] private TitleUIMoveScript[] m_selectScripts;
    //ロゴのスクリプト
    [SerializeField] private TitleLogoScript m_logoScript;

    /**
    * @brief 初期化処理
    *
    * @param[in] なし
    *
    * @return なし
    */
    void Start()
    {
        if (m_logoScript == null)
        {
            Debug.LogError("m_titleLogoScript がアタッチされていません！");
            enabled = false; // Update を止める
            return;
        }
    }

    /**
    * @brief 更新処理
    *
    * @param[in] なし
    *
    * @return なし
    */
    void Update()
    {
        if (m_logoScript == null) return;
        for (int i = 0; i < m_selectScripts.Length; i++)
        {
            if (m_selectScripts[i] == null) return;
        }
        //中央にロゴが到達していなければ更新を行わない
        if (!m_logoScript.GetArrival()) return;
        //透明度変化演出が終了していなければ演出の更新と、それ以降の更新を飛ばす
        for (int i = 0; i < m_selectScripts.Length; i++)
        {
            if (!m_selectScripts[i].GetAlphaEnd())
            {
                m_selectScripts[i].AlphaChange();
            }
        }
        //左右キーが押されたら、現在選択されている番号を変更する
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_select++;
            if (m_select > m_selectScripts.Length - 1)
            {
                m_select = m_selectScripts.Length - 1;
            }
            else
            {
                m_selectScripts[m_select].ResetTimer();
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_select--;
            if (m_select < 0)
            {
                m_select = 0;
            }
            else
            {
                m_selectScripts[m_select].ResetTimer();
            }

        }

        //エンターキーが押されたタイミングで現在選択されている番号に応じた処理をする
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (m_select)
            {
                case 0:
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
                    break;
                case 1:
                    SceneManager.LoadScene(m_scene, LoadSceneMode.Single);
                    break;

            }
        }

        for (int i = 0; i < m_selectScripts.Length; ++i)
        {
            if (m_selectScripts[i] == null) continue;
            if (i == m_select)
            {
                m_selectScripts[m_select].ChoiceMove();
            }
            else
            {
                m_selectScripts[i].MoveReset();
            }
        }

        for (int i = 0; i < m_selectScripts.Length; i++)
        {
            if (m_selectScripts[i] == null)
            {
                Debug.Log(i + "番目はnullです");
            }
            else
            {
                Debug.Log(i + "番目はnullではないです");
            }
        }
        Debug.Log("test");
        Debug.Log("現在の選択 :" + m_select);
    }
}
