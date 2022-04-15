using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SlideShow : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private List<Sprite> spritelist;
    [SerializeField] private Button nextBtn;
    [SerializeField] private Button previousBtn;
    [SerializeField] private Button Finish;
    private int Index;
    private int index
    {
        get
        {
            return this.Index;
        }
        set
        {
            this.Index = value % spritelist.Count;
            image.sprite = spritelist[this.Index];
            previousBtn.gameObject.SetActive(this.Index == 0 ? false : true);
            Finish.gameObject.SetActive(this.Index == spritelist.Count - 1 ? true : false);
            nextBtn.gameObject.SetActive(this.Index == spritelist.Count - 1 ? false : true);
        }
    }
    void Start()
    {
        nextBtn.onClick.AddListener(() => { index += 1; });
        previousBtn.onClick.AddListener(() => { index -= 1; });
        Finish.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("main_menu_scene");
        });
        index = 0;
    }

}
