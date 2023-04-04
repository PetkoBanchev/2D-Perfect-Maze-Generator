using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class DropdownListPopulator
{
    public static void PopulateDropdown<T>(Dropdown dropdown)
    {
        string[] enumNames = Enum.GetNames(typeof(T));
        List<string> optionNames = new List<string>(enumNames);
        dropdown.AddOptions(optionNames);
    }
}
