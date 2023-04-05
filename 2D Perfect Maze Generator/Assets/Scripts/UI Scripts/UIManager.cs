using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Private Variables

    [SerializeField] private GameObject mazeMenu;
    [SerializeField] private InputField mazeWidthInput;
    [SerializeField] private InputField mazeHeightInput;
    [SerializeField] private Dropdown cellTypeDropdown;
    [SerializeField] private Dropdown algorithmTypeDropdown;

    private bool isMenuVisible = true;

    #endregion

    #region Events

    public event Action OnGenerateMazePressed;

    #endregion

    #region Singleton
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }
    #endregion

    #region Private Methods

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

    private void Update()
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

    #endregion

    #region Public Methods

    /// <summary>
    /// Gets the width value from the UI. Parses it to an int. Clamps it between 10 and 250.
    /// Passes it to the MazeManager and updates the UI text.
    /// </summary>
    /// <param name="value"></param>
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

    /// <summary>
    /// Gets the height value from the UI. Parses it to an int. Clamps it between 10 and 250.
    /// Passes it to the MazeManager and updates the UI text.</summary>
    /// <param name="value"></param>
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

    /// <summary>
    /// Gets the index from the dropdown list, parses it back to the Cell enum and sends it to the MazeManager.
    /// </summary>
    /// <param name="index"></param>
    public void SetCellType(int index)
    {
        MazeManager.Instance.CellType = (Cell)index;
    }

    /// <summary>
    /// Gets the index from the dropdown list, parses it back to the Algorithm enum and sends it to the MazeManager.
    /// </summary>
    /// <param name="index"></param>
    public void SetAlgorithmType(int index)
    {
        MazeManager.Instance.Algorithm = (Algorithm)index;
    }

    /// <summary>
    /// Connected to the GenerateMaze button in the UI.
    /// Hides the menu and invokes the OnGenerateMazePressed event.
    /// </summary>
    public void GenerateMaze()
    {
        SetMazeWidth(mazeWidthInput.text); // Resets the maze width due to the doubling in the MazeManager. Check lines 136-139 in the MazeManager for more info.
        ToggleMenuVisibility();
        OnGenerateMazePressed?.Invoke();
    }

    #endregion
}
