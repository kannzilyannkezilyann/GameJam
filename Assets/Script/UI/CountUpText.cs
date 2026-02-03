using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountUpText : MonoBehaviour
{
// データメンバの宣言 -----------------------------------------------
    //現在数字
    int m_currentNumber;
    //目標数字
    int m_targetNumber;
    //増加数字
    int m_stepNumber;
    //テキスト
    TextMeshProUGUI m_text;
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
        m_text = GetComponent<TextMeshProUGUI>();
        m_text.text = "0000000";
    }

    /**
     * @brief 初期化処理
     *
     * @param[in] start  開始数字
     * @param[in] target 目標数字
     * @param[in] step　 増加数字 1秒で
     *
     * @return なし
     */

    public void Initialize(int start,int target,int step)
    {
        m_currentNumber = start;
        m_targetNumber = target;
        m_stepNumber = step;
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
        m_currentNumber += Mathf.RoundToInt(m_stepNumber * Time.deltaTime);


        if (IsFinish())
        {
            m_currentNumber = m_targetNumber;
        }

        m_text.text = m_currentNumber.ToString("000000");
    }
    /**
     * @brief 終了したか
     *
     * @param[in] なし
     *
     * @return true  終了
     * @return true  未了
     */

    public bool IsFinish()
    {
        return (m_currentNumber >= m_targetNumber);
    }
}
