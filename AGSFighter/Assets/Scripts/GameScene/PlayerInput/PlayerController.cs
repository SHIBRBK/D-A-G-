using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 使用するコントローラ番号
    [SerializeField] int m_controller_id = 0;

    private ControllerBehaviour m_controller = null;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitControllerIn(m_controller_id));
    }

    // Update is called once per frame
    void Update()
    {
        if (m_controller == null) { return; }

        var vect = m_controller.CrossKeyValue;
        vect *= 0.10f;
        this.transform.position += new Vector3(vect.x, vect.y, 0.0f);
    }

    private IEnumerator WaitControllerIn(int controller_id)
    {
        while (!ControllerManager.Instance.IsController(controller_id)) { yield return null; }
        m_controller = ControllerManager.Instance.GetController(controller_id);
    }
}