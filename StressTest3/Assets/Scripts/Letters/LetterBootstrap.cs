using System;
using UnityEngine;

namespace Letters
{
    public class LetterBootstrap : MonoBehaviour
    {
        private void Awake()
        {
            var db = LetterCore.LoadDb();
            
            Debug.Log("some " + db[2]);
        }
    }
}