using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstEnvyDoor : EnvyDoor
{
    protected override void test()
    {
        SceneLoader.LoadScene("SecondEnvy");
    }
}
