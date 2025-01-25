using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyUI : MonoBehaviour
{
    private Camera cam;
    [SerializeField]
    private Image healthBarSprite;
    [SerializeField]
    private TextMeshProUGUI lvText;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + cam.transform.forward);
    }

    public void UpdateHealthBar(float HP, float maxHP)
    {
        HP = HP > 0 ? HP : 0;
        healthBarSprite.fillAmount = HP / maxHP;
    }

    public void UpdateLv(float LV)
    {
        lvText.text = "LV " + LV;
    }
}
