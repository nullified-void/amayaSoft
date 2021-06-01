using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    public Image backGround;
    public Image content;
    public int value;
    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        this.gameController = GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<GameController>();
        DOTween.Init();
        if (gameController.level == 1)
        {
            this.transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0), 1);
        }
    }


    IEnumerator correctPick()
    {
        yield return new WaitForSeconds(1.5f);
       
        gameController.CorrectAnswerPicker();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.value == gameController.answers[gameController.answers.Count - 1])
        {
            transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0), 1); ;
            StartCoroutine(correctPick());
        }
        else
        {
            transform.DOShakePosition(1f,4f);
        }
    }
}
