using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    Sprite[] sprites;
    public SpriteAtlas[] texture;
    public int level = 1;
    public GameObject cell;
    List<GameObject> pool = new List<GameObject>();
    public List<int> answers = new List<int>();
    public GameObject panel;
    RectTransform rect;
    public Text text;
    [SerializeField]
    private GameObject screenBlocker;
    [SerializeField]
    private GameObject Button;
    [SerializeField]
    private ParticleSystem[] particleSystems;
    void Start()
    {
        screenBlocker.GetComponent<UnityEngine.UI.Image>().DOFade(0, 2f);
        StartCoroutine(ToggleScreenBlocker());
        //int randomIndex = Random.Range(0, texture.Length);
        //sprites = new Sprite[texture[randomIndex].spriteCount];
        //texture[randomIndex].GetSprites(sprites);
        Reload();
        
    }
    IEnumerator ToggleScreenBlocker()
    {
        yield return new WaitForSeconds(2f);
        screenBlocker.SetActive(false);
    }
    void Reload()
    {
        int randomIndex = Random.Range(0, texture.Length);
        sprites = new Sprite[texture[randomIndex].spriteCount];
        texture[randomIndex].GetSprites(sprites);
        List<int> usedVariants = new List<int>();
        if (sprites.Length <= 10)
        {
            usedVariants.Clear();
            answers.Clear(); 
        }
        int correctAsnwer = Random.Range(0, level * 3);
        foreach (GameObject gOBj in pool)
        {
            Object.Destroy(gOBj);
        }
        pool.Clear();
        for (int y = 0; y < level; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                pool.Add(Instantiate<GameObject>(cell, panel.transform));
                Cell captivity = pool[pool.Count - 1].GetComponent<Cell>();
                bool hue = true;
                while (hue)
                {
                    int rand = Random.Range(0, sprites.Length);
                    if (!answers.Contains(rand) && !usedVariants.Contains(rand))
                    {

                        hue = false;

                        if (y * 3 + x == correctAsnwer)
                        {
                            answers.Add(rand);
                            
                        }
                        usedVariants.Add(rand);
                    }
                }
                captivity.value = usedVariants[usedVariants.Count - 1];
                captivity.content.sprite = sprites[captivity.value];
                rect = captivity.GetComponent<RectTransform>();
                rect.localPosition = new Vector3(x * 200 - 200, y * 200 - 200 * (level-1) + 100 * Mathf.Clamp(level -2, 0, 2), 0);
                

            }
        }
        text.text = "FIND " + sprites[answers[answers.Count - 1]].name.Substring(7, sprites[answers[answers.Count - 1]].name.IndexOf("(Clone)") - 7);
        if (level == 1)
        {
            text.DOColor(Color.black, 3f);
        }
       
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name.ToString(), 0);
    }
    public void CorrectAnswerPicker()
    {
        if (level < 3)
        {
            foreach (ParticleSystem ps in particleSystems)
            {
                ps.Emit(10);
            }
            level++;
            Reload();
            RectTransform rectTrans = panel.GetComponent(typeof(RectTransform)) as RectTransform;
            rectTrans.sizeDelta = new Vector2(600, 200 * level);
        }
        else
        {
            Button.SetActive(true);
            screenBlocker.SetActive(true);
            screenBlocker.GetComponent<UnityEngine.UI.Image>().DOColor(Color.white, 2f);
            Button.GetComponent<UnityEngine.UI.Image>().DOColor(Color.white, 2f);
        }
    }
}
