using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScIAPTester : MonoBehaviour {

    private static ScIAPTester m_instance;

    public static ScIAPTester instance
    {
        get
        {
            return m_instance;
        }
    }

    public Text showItemNumber;
    void Awake()
    {
        m_instance = this;
    }

    void Update()
    {

    }
}
