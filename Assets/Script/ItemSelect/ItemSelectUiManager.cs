/**
* @file   ItemSelectUiManager.cs
*
* @brief  アイテムセレクトシーンのUIに関するヘッダファイル
*
* @author 制作者名　深谷翔太
*
* @date   日付　2026/02/04
*/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemSelectUiManager : MonoBehaviour
{
    [System.Serializable]
    struct ImageAndItemKinds
    {
        public ItemKinds itemKinds;
        public Image image;
        public string explanation;
    }

    //決定時のSE
    [SerializeField] private AudioClip m_choiceSE;
    //選択時の効果音
    [SerializeField] private AudioClip m_selectSE;
    //SEを鳴らす為のコンポーネント
    private AudioSource m_audioSource;
    //開始地点
    private Vector2 m_start;
    //目標地点
    [SerializeField] private Vector2 m_end;
    //説明用文章
    [SerializeField] TextMeshProUGUI m_text;
    //移動にかける時間
    [SerializeField] float m_moveTime = 1.0f;
    //マップの位置
    [SerializeField] RectTransform m_mapPos;
    //遷移先のシーン名
    [SerializeField] private string m_nextSceneName;
    //吹き出しのアニメーションに関するスクリプト
    [SerializeField] HukidasiScript m_hukidasiScript;
    //上下移動の長さ
    [SerializeField] float m_range = 4.0f;
    //再生速度(指)
    [SerializeField] float m_speed = 2.0f;
    //現在の選択
    int m_select = 0;
    //時間
    float m_timer = 0.0f;
    //動かす時間
    float m_moveTimer = 0.0f;
    //色変更のフラグ
    bool m_colorFlag = false;
    //指画像の初期位置
    float m_fingerPosY;
    //画像とアイテム
    [SerializeField] ImageAndItemKinds[] m_imageAndItemKinds;
    //指の画像
    [SerializeField] Image m_fingerImages;
    //決定したかどうかのフラグ
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
        //すべての画像を透明にする
        UnityEngine.Color color;
        for (int i = 0; i < m_imageAndItemKinds.Length; i++)
        {
            color = m_imageAndItemKinds[i].image.color;
            color.a = 0.0f;
            m_imageAndItemKinds[i].image.color = color;
        }
        color = m_fingerImages.color;
        color.a = 0.0f;
        m_fingerImages.color = color;
        color = m_text.color;
        color.a = 0.0f;
        m_text.color = color;

        m_fingerPosY = m_fingerImages.transform.position.y;

        m_start = m_mapPos.anchoredPosition;

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
        if (m_hukidasiScript == null) return;
        //吹き出しの演出が終わったならすべての画像を色付きにする
        if (m_hukidasiScript.GetAnimationEnd() && !m_colorFlag)
        {
            UnityEngine.Color color;
            for (int i = 0; i < m_imageAndItemKinds.Length; i++)
            {
                color = m_imageAndItemKinds[i].image.color;
                color.a = 1.0f;
                m_imageAndItemKinds[i].image.color = color;
            }
            color = m_fingerImages.color;
            color.a = 1.0f;
            m_fingerImages.color = color;

            color = m_text.color;
            color.a = 1.0f;
            m_text.color = color;
            m_colorFlag = true;
        }
        //色変化が終了していないなら以下の処理を飛ばす
        if (!m_colorFlag) return;

        //時間更新
        m_timer += Time.deltaTime;
        //選択(十字キー)と値のリセット
        if (!m_selected)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                m_select++;
                if (m_select > m_imageAndItemKinds.Length - 1)
                {
                    m_select = 0;
                }
                m_timer = 0.0f;
                m_audioSource.PlayOneShot(m_selectSE);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                m_select--;
                if (m_select < 0)
                {
                    m_select = m_imageAndItemKinds.Length - 1;
                }
                m_timer = 0.0f;
                m_audioSource.PlayOneShot(m_selectSE);
            }
        }
        //指画像を動かす
        Vector2 point;
        point.x = m_imageAndItemKinds[m_select].image.transform.position.x;
        point.y = m_fingerPosY + (m_range * Mathf.Sin(m_timer * m_speed));

        m_fingerImages.transform.position = point;
        m_text.text = m_imageAndItemKinds[m_select].explanation;

        if(m_moveTimer <= m_moveTime)
        {
            m_moveTimer += Time.deltaTime;
            float t = m_moveTimer / m_moveTime;

            m_mapPos.anchoredPosition = Vector2.Lerp(m_start, m_end, t);
        }

        //決定したらゲームマネージャーにセットしてシーン遷移
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("start");
            if (m_selected) return;

            m_selected = true;
            StartCoroutine(ChoiceCoroutine());
            Debug.Log("end");
        }
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
        Debug.Log("start");
        // 決定SE再生
        m_audioSource.PlayOneShot(m_choiceSE);

        // SEが鳴り終わるまで待つ
        yield return new WaitForSeconds(m_choiceSE.length);

        GameManager.instance.SetSelectItem(m_imageAndItemKinds[m_select].itemKinds);
        SceneManager.LoadScene(m_nextSceneName);
    }
}
