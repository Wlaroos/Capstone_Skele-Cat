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

        if (hp == 4 )
        {
            transform.Find("HealthArea").Find("HP4").GetComponent<Image>().enabled = true;
            transform.Find("HealthArea").Find("HP3").GetComponent<Image>().enabled = true;
            transform.Find("HealthArea").Find("HP2").GetComponent<Image>().enabled = true;
            transform.Find("HealthArea").Find("HP1").GetComponent<Image>().enabled = true;
        }
        else if (hp == 3)
        {
            transform.Find("HealthArea").Find("HP1").GetComponent<Image>().enabled = false;
        }

        else if (hp == 2)
        {
            transform.Find("HealthArea").Find("HP2").GetComponent<Image>().enabled = false;
        }

        else if (hp == 1)
        {
            transform.Find("HealthArea").Find("HP3").GetComponent<Image>().enabled = false;
        }

        else if (hp <= 0)
        {
            transform.Find("HealthArea").Find("HP4").GetComponent<Image>().enabled = false;
        }
    }

}