using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Jogar(){
        SceneManager.LoadScene("selecao");
    }
    public void Opcoes(){
        SceneManager.LoadScene("instrucoes");
    }
    public void Voltar(){
        SceneManager.LoadScene("Menu");
    }
    public void Sair(){
        Application.Quit();
    }
}
