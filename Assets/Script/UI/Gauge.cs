/**
 * @file   Gauge.cs
 *
 * @brief  水中ゲージに関するヘッダファイル
 *
 * @author 制作者名　深谷翔太
 *
 * @date   日付　2026/02/05
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Gauge : MonoBehaviour
{
    //画像
    [SerializeField] Image m_baseImage;
    [SerializeField] Image m_gaugeImage;
    [SerializeField] RectTransform m_gauge;
    //プレイヤーのスクリプト
    [SerializeField] PlayerScript m_playerScript;

    Vector3 m_defaultScale;

    // Start is called before the first frame update
    void Start()
    {
        Color c = m_baseImage.color;
        c.a = 0.0f;
        m_baseImage.color = c;
        c = m_gaugeImage.color;
        c.a = 0.0f;
        m_gaugeImage.color = c;

        m_defaultScale = m_gauge.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_playerScript.GetHeadHit())
        {
            Color c = m_baseImage.color;
            c.a = 1.0f;
            m_baseImage.color = c;
            c = m_gaugeImage.color;
            c.a = 1.0f;
            m_gaugeImage.color = c;

            float rate = m_playerScript.GetLate();

            m_gauge.localScale = new Vector3(
            m_defaultScale.x * rate,
            m_defaultScale.y,
            m_defaultScale.z
            );
        }
        else
        {
            Color c = m_baseImage.color;
            c.a = 0.0f;
            m_baseImage.color = c;
            c = m_gaugeImage.color;
            c.a = 0.0f;
            m_gaugeImage.color = c;
        }
    }
}
