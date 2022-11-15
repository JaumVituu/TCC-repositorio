using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
    public Image[] characters;
    public TextMeshProUGUI[] characternames;
    public int selectedcharacter;
    // Start is called before the first frame update
    void Start()
    {
        characters[0].enabled = true;
        characters[1].enabled = false;
        characternames[0].enabled = true;
        characternames[1].enabled = false;
    }
    public void RightButton(){
        characters[selectedcharacter].enabled = false;
        characternames[selectedcharacter].enabled = false;
        selectedcharacter = (selectedcharacter + 1) % characters.Length;
        characters[selectedcharacter].enabled = true;
        characternames[selectedcharacter].enabled = true;
    }
    public void LeftButton(){
        characters[selectedcharacter].enabled = false;
        characternames[selectedcharacter].enabled = false;
        selectedcharacter--;
        if(selectedcharacter < 0){
            selectedcharacter += characters.Length;
        }
        characters[selectedcharacter].enabled = true;
        characternames[selectedcharacter].enabled = true;
    }
    public void Comecar(){
        PlayerPrefs.SetInt("selectedcharacter", selectedcharacter);
        SceneManager.LoadScene("Estande de Teste");
    }
}
