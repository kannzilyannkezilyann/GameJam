/**
 * @file   HukidasiScript.cs
 *
 * @brief  吹き出しの動きに関するヘッダファイル
 *
 * @author 制作者名　深谷翔太
 *
 * @date   日付　2026/02/04
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HukidasiScript : MonoBehaviour
{
    //歩行時のSE
    [SerializeField] AudioClip m_hukidasiSE;
    //音楽を再生するためのコンポーネント
    private AudioSource m_audioSource;
    //プレイヤーのスクリプト
    [SerializeField] ItemSelectPlayerScript m_itemSelectPlayerScript;
    //アニメーション
    private Animator m_animator;
    //アニメーションが終了したかどうか
    private bool m_isAnimationEnd = false;
    //音声が再生されたかどうか
    private bool m_isPlay = false;

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
        m_animator = GetComponent<Animator>();
    }

    /**
    * @brief 初期化処理
    *
    * @param[in] なし
    *
    * @return なし
    */
    void Update()
    {
        if (m_itemSelectPlayerScript == null) return;

        if (m_itemSelectPlayerScript.GetIsStopPoint())
        {
            m_animator.SetTrigger("HukidasiAnimation");
            if(!m_isPlay)
            {
                m_isPlay = true;
                m_audioSource.PlayOneShot(m_hukidasiSE);
            }
        }
    }

    /**
    * @brief アニメーションが終了した時にフラグを更新する処理
    *
    * @param[in] なし
    *
    * @return なし
    */
    public void OnAnimationEnd()
    {
        Debug.Log("終わったよ");
        m_isAnimationEnd = true;
    }

    /**
   * @brief アニメーションが終了した時にフラグを更新する処理
   *
   * @param[in] なし
   *
   * @return なし
   */
    public bool GetAnimationEnd()
    {
        return m_isAnimationEnd;
    }
}
