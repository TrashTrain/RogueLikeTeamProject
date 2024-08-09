using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogData", menuName = "CreateDialogData/DialogData")]
public class DialogSet : ScriptableObject
{
    public int IdxNum;
    public int nextIdx;
    public DialogElement[] dialogElements;

}
[Serializable]
public struct DialogElement
{
    public string dialog;
    public int selectYes;
    public int selectNo;
}
