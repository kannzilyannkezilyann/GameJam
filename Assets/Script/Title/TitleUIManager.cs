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
    //音楽関係
    //決定時のSE
    [SerializeField] private AudioClip m_choiceSE;
    //選択時の効果音
    [SerializeField] private AudioClip m_selectSE;
    //選択できなかった時の効果音
    [SerializeField] private AudioClip m_notSelectSE;
    //SEを鳴らす為のコンポーネント
    private AudioSource m_audioSource;
    //現在のセレクト番号
    int m_select = 0;
    //遷移先のシーン
    [SerializeField] private string m_scene;
    //選択肢のオブジェクト
    [SerializeField] private TitleUIMoveScript[] m_selectScripts;
    //ロゴのスクリプト
    [SerializeField] private TitleLogoScript m_logoScript;
    //選択されたかどうか
    private bool m_selected = false;   

    /**
    * @brief 初期化処理
    *
    * @param[in] なし
    *
    * @return なし
    */
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
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
        if(m_selectSE == null)
        {
            Debug.Log("m_selectSE = null");
            return;
        } 
        if(m_choiceSE == null)
        {
            Debug.Log("m_choiceSE = null");
            return;
        }
        if (m_notSelectSE == null)
        {
            Debug.Log("m_notSelectSE = null");
            return;
        }
        if (m_audioSource == null)
        {
            Debug.Log("m_audioSource = null");
            return;
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
        if(!m_selected)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                m_select++;
                if (m_select > m_selectScripts.Length - 1)
                {
                    m_audioSource.PlayOneShot(m_notSelectSE);
                    m_select = m_selectScripts.Length - 1;
                }
                else
                {
                    m_audioSource.PlayOneShot(m_selectSE);
                    m_selectScripts[m_select].ResetTimer();
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                m_select--;
                if (m_select < 0)
                {
                    m_audioSource.PlayOneShot(m_notSelectSE);
                    m_select = 0;
                }
                else
                {
                    m_audioSource.PlayOneShot(m_selectSE);
                    m_selectScripts[m_select].ResetTimer();
                }

            }
        }

        //エンターキーが押されたタイミングで現在選択されている番号に応じた処理をする
        if (Input.GetKeyDown(KeyCode.Return))
        {
            m_selected = true;
            StartCoroutine(ChoiceCoroutine());
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

   /**
   * @brief 選択されたときの処理
   *
   * @param[in] なし
   *
   * @return なし
   */
    IEnumerator ChoiceCoroutine()
    {
        // 決定SE再生
        m_audioSource.PlayOneShot(m_choiceSE);

        // SEが鳴り終わるまで待つ
        yield return new WaitForSeconds(m_choiceSE.length);

        switch (m_select)
        {
            case 0:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
                break;

            case 1:
                SceneManager.LoadScene(m_scene, LoadSceneMode.Single);
                break;
        }
    }

}
