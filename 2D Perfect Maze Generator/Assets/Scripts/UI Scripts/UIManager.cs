using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mazeMenu;
    [SerializeField] private InputField mazeWidthInput;
    [SerializeField] private InputField mazeHeightInput;
    [SerializeField] private Dropdown cellTypeDropdown;
    [SerializeField] private Dropdown algorithmTypeDropdown;

    private bool isMenuVisible = true;

    public event Action OnGenerateMazePressed;

    #region Singleton
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }
    #endregion

    private void Awake()
    {
        // Singleton pattern
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

        DropdownListPopulator.PopulateDropdown<Cell>(cellTypeDropdown);
        DropdownListPopulator.PopulateDropdown<Algorithm>(algorithmTypeDropdown);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            ToggleMenuVisibility();
        }
    }

    private void ToggleMenuVisibility()
    {
        if(isMenuVisible)
            mazeMenu.SetActive(false);
        else 
            mazeMenu.SetActive(true);
        isMenuVisible = !isMenuVisible;
    }

    public void SetMazeWidth(string value)
    {
        var width = Int32.Parse(value);
        if (width < 10)
            width = 10;
        else if (width > 250)
            width = 250;

        MazeManager.Instance.Width = width;
        mazeWidthInput.text = width.ToString();
    }

    public void SetMazeHeight(string value)
    {
        var height = Int32.Parse(value);
        if (height < 10)
            height = 10;
        else if (height > 250)
            height = 250;

        MazeManager.Instance.Height = height;
        mazeHeightInput.text = height.ToString();
    }

    public void SetMazeAnimation(bool value)
    {
        MazeManager.Instance.IsGenerationAnimated = value;
    }

    public void SetCellType(int index)
    {
        MazeManager.Instance.CellType = (Cell)index;
    }

    public void SetAlgorithmType(int index)
    {
        MazeManager.Instance.Algorithm = (Algorithm)index;
    }

    public void GenerateMaze()
    {
        ToggleMenuVisibility();
        OnGenerateMazePressed?.Invoke();
    }
}
