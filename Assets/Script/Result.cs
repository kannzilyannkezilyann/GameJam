using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    //遷移先のシーン
    [SerializeField] SceneAsset m_scene;

    [SerializeField] GameObject m_scoreText;
    [SerializeField] Canvas m_canvas;
    //生成したテキスト
    CountUpText m_countUpText;
    // Start is called before the first frame update
    void Start()
    {
        GameObject gameObject = Instantiate(m_scoreText);
        if(gameObject != null)
        {
            m_countUpText = gameObject.GetComponent<CountUpText>();
            m_countUpText.Initialize(0, GameManager.instance.GetScore(), 800);
            gameObject.transform.SetParent(m_canvas.transform, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && m_countUpText.IsFinish())
        {
            SceneManager.LoadScene(m_scene.name);
        }
    }
}
