/**
 * @file   TitleUIMoveScript.cs
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
using UnityEngine.UI;

public class TitleUIMoveScript : MonoBehaviour
{
    //最初のサイズ
    Vector2 m_initialScale;
    //どれだけ大きくするか
    [SerializeField] private float m_range = 0.005f;
    //画像の色情報
    Color m_color;
    //透明度調整演出が終了したかどうか
    bool m_endAlpha = false;
    //タイマー(演出に使用)
    float m_timer;

    /**
    * @brief 初期化処理
    *
    * @param[in] なし
    *
    * @return なし
    */
    void Start()
    {
        m_timer = 0.0f;
        m_initialScale = transform.localScale;
        m_color = gameObject.GetComponent<Image>().color;
        m_color.a = 0.0f;
        gameObject.GetComponent<Image>().color = m_color;
    }

    // Update is called once per frame
    void Update()
    {
    }

    /**
    * @brief 選択中の処理
    *
    * @param[in] なし
    *
    * @return なし
    */
    public void ChoiceMove()
    {
        m_timer += Time.deltaTime;
        float sin = (Mathf.Sin(m_timer) + 1.0f) / 2.0f; //0~1の範囲
        float scaleIndex = m_initialScale.x + m_range * sin;
        this.transform.localScale = new Vector2(scaleIndex, scaleIndex);
    }

    /**
    * @brief 選択外のリセット処理
    *
    * @param[in] なし
    *
    * @return なし
    */
    public void MoveReset()
    {
        this.transform.localScale = m_initialScale;
    }

    /**
    * @brief 透明度の変化処理
    *
    * @param[in] なし
    *
    * @return 
    */
    public void AlphaChange()
    {
        m_timer += Time.deltaTime;
        float sin = (Mathf.Sin(m_timer) + 1.0f) / 2.0f; //0~1の範囲
        if(sin >= 0.99f)
        {
            sin = 1.0f;
            m_endAlpha = true;
        }
        m_color.a = sin;
        gameObject.GetComponent<Image>().color = m_color;
    }

    /**
    * @brief タイマーのリセット処理
    *
    * @param[in] なし
    *
    * @return 
    */
    public void ResetTimer()
    {
        m_timer = 0.0f;
    }

    /**
    * @brief 演出が終了しているかどうかを取得する
    *
    * @param[in] なし
    *
    * @return 演出の終了
    */
    public bool GetAlphaEnd()
    {
        return m_endAlpha;
    }
}
