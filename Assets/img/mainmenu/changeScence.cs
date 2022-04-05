using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeScence : MonoBehaviour
{
    // Start is called before the first frame update

    public void btn_change_scence(string scence_name){
        SceneManager.LoadScene(scence_name);
    }

    public void QuitGame(){
        Application.Quit();
        Debug.Log("exit");
    }
}
