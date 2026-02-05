/**
 * @file   PlayerOperate.cs
 *
 * @brief  プレイヤー操作説明に関するヘッダファイル
 *
 * @author 制作者名　福地貴翔
 *
 * @date   日付　2026/02/04
 */
using NUnit;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOperate : MonoBehaviour
{
// クラス定数の宣言 -------------------------------------------------
    
// データメンバの宣言 -----------------------------------------------
    //矢印イメージ
    [SerializeField] private Image m_arrowImage;
    //開閉キー
    [SerializeField] private KeyCode m_key;
    //開いたときの位置
    [SerializeField] private Vector2 m_openPostion;
    //開じたときの位置
    [SerializeField] private Vector2 m_closePostion;
    //開閉中
    private bool m_isOpenClose;
    //移動開始位置
    private Vector2 m_startPosition;
    //移動終了位置
    private Vector2 m_endPositoon;
    //移動開始角度Y
    private float m_startRotation = 180.0f;
    //移動終了角度Y
    private float m_endRotation = 0.0f;
    // メンバ関数の定義 -------------------------------------------------

    /**
     * @brief 生成時処理
     *
     * @param[in] なし
     *
     * @return なし
     */
    void Start()
    {
        m_startPosition = m_openPostion;
        m_endPositoon = m_closePostion;
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
        //設定したキーが押されたら
        if(!m_isOpenClose && Input.GetKeyDown(m_key))
        {
            //開閉し始める
            m_isOpenClose = true;
        }
        //開閉中でなければ飛ばす
        if (!m_isOpenClose)
        {
            return;
        }

        Move(ref m_startPosition,ref m_endPositoon,ref m_startRotation,ref m_endRotation);
    }

    /**
     * @brief 開閉処理
     *
     * @param[in] startPosition  開始位置
     * @param[in] endPosition　　目標位置
     * @param[in] startRotationY  開始角度
     * @param[in] endRotationY　　目標角度
     *
     * @return なし
     */
    void Move(ref Vector2 startPosition, ref Vector2 endPosition,ref float startRotationY,ref float endRotationY)
    {
        float speed = 5.0f;
        //少しずつ目標位置に移動
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, endPosition, Time.deltaTime * speed);
        //矢印の向きも変える
        Quaternion currentRotation = m_arrowImage.rectTransform.rotation;
        Quaternion targetRotation = Quaternion.Euler(currentRotation.eulerAngles.x, endRotationY, currentRotation.eulerAngles.z);
        m_arrowImage.rectTransform.rotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime * speed);

        //ある程度目標位置に近づいたら
        if (Vector2.Distance(rectTransform.anchoredPosition, endPosition) <= 0.3f)
        {
            //到着させる
            rectTransform.anchoredPosition = endPosition;
            //矢印も
            m_arrowImage.rectTransform.rotation = Quaternion.Euler(
                m_arrowImage.rectTransform.rotation.eulerAngles.x,
                endRotationY,
                m_arrowImage.rectTransform.rotation.eulerAngles.z
            );
            //スタート位置と目標位置を入れ替える
            Vector2 save = startPosition;
            startPosition = endPosition;
            endPosition = save;
            float temp = startRotationY;
            startRotationY = endRotationY;
            endRotationY = temp;
            //開閉状態をオフに
            m_isOpenClose = false;
        }
    }

}
