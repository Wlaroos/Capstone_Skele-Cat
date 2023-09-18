using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDMenu : MonoBehaviour
{
    [SerializeField] Text _levelName;
    [SerializeField] public int hp = 4;
    
    private void Start()
    {
        transform.Find("LevelName").GetComponent<Text>().text = ("- " + SceneManager.GetActiveScene().name + " -");
    }

    private void Update()
    {
        switch (hp)
        {
            case 4:
                transform.Find("HealthArea").Find("HP4").GetComponent<Image>().enabled = true;
                transform.Find("HealthArea").Find("HP3").GetComponent<Image>().enabled = true;
                transform.Find("HealthArea").Find("HP2").GetComponent<Image>().enabled = true;
                transform.Find("HealthArea").Find("HP1").GetComponent<Image>().enabled = true;
                break;
            case 3:
                transform.Find("HealthArea").Find("HP1").GetComponent<Image>().enabled = false;
                break;
            case 2:
                transform.Find("HealthArea").Find("HP2").GetComponent<Image>().enabled = false;
                break;
            case 1:
                transform.Find("HealthArea").Find("HP3").GetComponent<Image>().enabled = false;
                break;
            case <= 0:
                transform.Find("HealthArea").Find("HP4").GetComponent<Image>().enabled = false;
                break;
        }
    }

}