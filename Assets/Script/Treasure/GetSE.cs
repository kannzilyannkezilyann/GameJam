/**
 * @file   GetSE.cs
 *
 * @brief  取得時の効果音に関するヘッダファイル
 *
 * @author 制作者名　深谷翔太
 *
 * @date   日付　2026/02/05
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSE : MonoBehaviour
{
    //流す効果音
    [SerializeField] private AudioClip m_getSE;
    //効果音を流すためのコンポーネント
    private AudioSource m_source;

    /**
    * @brief 初期化処理
    *  
    * @param[in] なし
    *
    * @return なし
    */
    void Start()
    {
        if(m_source == null) m_source = GetComponent<AudioSource>();

        StartCoroutine(ChoiceCoroutine());
    }

    IEnumerator ChoiceCoroutine()
    {
        // 取得SE再生
        m_source.PlayOneShot(m_getSE);

        // SEが鳴り終わるまで待つ
        yield return new WaitForSeconds(m_getSE.length);

        Destroy(gameObject);
    }
}
