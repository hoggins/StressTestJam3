using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Letters
{
    public class LetterCore
    {
        public static List<string> LoadDb()
        {
            var dat =(TextAsset)Resources.Load("db/zdb-win");
            return dat.text.Split('\n').Select(w => w.Trim()).ToList();
        }
    }
}
