using UnityEngine;
using UnityEngine.SceneManagement;

public class Acidente : MonoBehaviour
{
    public void IrParaMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
