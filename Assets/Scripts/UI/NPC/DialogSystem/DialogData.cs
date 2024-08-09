using System.Collections.Generic;

[System.Serializable]
public class DialogData
{
    public List<string> dialogName = new ();
    public List<int> dialogIndex = new (); 

    public DialogData (Dictionary<string, int> data)
    {
        foreach(KeyValuePair<string, int> kvp in data)
        {
            dialogName.Add(kvp.Key);
            dialogIndex.Add(kvp.Value);
        }
        
    }

}
