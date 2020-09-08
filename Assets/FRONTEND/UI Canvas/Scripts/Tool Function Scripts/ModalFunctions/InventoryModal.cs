using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public enum Category
{
    // Categories for the inventory
    SourcesBoards,
    Lenses,
    SlitsGratings,
    Detectors
}
[System.Serializable]
public class MeshCard
{
    // Define MeshCards object to be connected into the inspector
    public obj item;
    public GameObject meshCard;

    public MeshCard(obj item, GameObject meshCard)
    {
        this.item = item;
        this.meshCard = meshCard;
    }
}

public class InventoryModal : Modal
{
    // Add all relevant objects to the inspector - modal, buttons, locks etc.

    [SerializeField]
    private List<MeshCard> availableCards = new List<MeshCard>();

    public Dictionary<obj, GameObject> availableCardsDict = new Dictionary<obj, GameObject>();


    [SerializeField]
    private GameObject[] locks;

    [SerializeField]
    private Button sourcesBoards, lenses, slitsGratings, detectors;

    [SerializeField]
    private GameObject tabSource, tabLenses, tabSlit, tabDet;

    public Category categorySelected;

    void Awake ()
    {
        // Define all of the variables required, inherited from parent Modal class
        _modal = this.gameObject;
        _modalOverlay = this.transform.GetChild(0).GetComponentInParent<Button>();
        _closeModalBtn = this.transform.GetChild(1).GetChild(0).GetChild(1).GetComponentInParent<Button>();
        _activeBckg = TooltrayController.Instance.dynamicTray.transform.GetChild(0).GetChild(0).gameObject;
    }

    void Start()
    {
        // From parent class, remove input camera control
        DeactivateInputManager();

        // Lock all categories except sources and boards
        LockCategories();
        _activeBckg = TooltrayController.Instance.dynamicTray.transform.GetChild(0).GetChild(0).gameObject;
        CloseListeners(); // arguments were _inventoryModal, _activeBckg but Tim removed.
        ButtonFromKey(obj.board).onClick.AddListener(PlaceBoard);
    }

    void OnEnable()
    {
        if (CardFromKey(obj.laser).activeSelf == true)
        {
            // Select first category
            SelectSourcesBoards();
            RecolourSourcesBoards(true);
        }
    }

    private void DictionaryMeshCards()
    {
        // Assigns a key to each card to ease calling each object in program
        foreach(MeshCard card in availableCards)
        {
            availableCardsDict.Add(card.item, card.meshCard);
        }
    }

    private GameObject CardFromKey(obj item)
    {
        // Retrieves the required card from the selection of MeshID as defined
        // in the inspector
        if (availableCardsDict.ContainsKey(item) == false)
        {
            DictionaryMeshCards();
            return availableCardsDict[item];
        }

        return availableCardsDict[item];
    }

    private Button ButtonFromKey(obj item)
    {
        // Retrieves the required button from the card
        return CardFromKey(item).GetComponent<Button>();
    }

    private void LockCategories()
    {
        // Lock categories by making other buttons non interactive for now
        Button[] categories = new Button[] {lenses, slitsGratings, detectors};
        foreach(Button category in categories)
        {
            category.interactable = false;
        }
    }

    private void SelectAndPlaceItem(Button thisClick, obj item)
    {
        // Functionality for instantiating prefabs, closing the inventory as it is 
        // introduced onto the scene
        MeshesInventory.Instance.InstantiateItem(item);
        CloseModal();
        thisClick.interactable = false;
    }

    private void SelectAndPlaceTwo(Button thisClick, obj mesh, obj manager)
    {
        // Functionality for instantiating prefabs, closing the inventory as it is 
        // introduced onto the scene
        MeshesInventory.Instance.InstantiateTwo(mesh, manager);
        CloseModal();
        thisClick.interactable = false;
    }

    private void PlaceBoard()
    {
        // Placing the board onto the scene unlocks all categories
        SelectAndPlaceItem(ButtonFromKey(obj.board), obj.board);
        unlockCategories();
    }

    private void unlockCategories()
    {
        // Unlocks all categories by removing lock icons, ativating listeners, 
        // changing the colour of the text and selecting the first category
        ActivateButtons();
        SelectSourcesBoards();

        foreach(GameObject lockIcon in locks)
        {
            lockIcon.SetActive (false);
        }

        Button[] categories = new Button[] {sourcesBoards, lenses, slitsGratings, detectors};

        foreach(Button category in categories)
        {
            category.interactable = true;
            TextMeshProUGUI categoryText = category.GetComponent<TextMeshProUGUI>();
            categoryText.color = new Color32(17, 17, 17, 255);
        }

    }

    private void ActivateButtons()
    {
        sourcesBoards.onClick.AddListener(SelectSourcesBoards);
        lenses.onClick.AddListener(SelectLenses);
        slitsGratings.onClick.AddListener(SelectSlitsGratings);
        detectors.onClick.AddListener(SelectDetectors);
    }
    private void SetActiveMeshes(Category categorySelected)
    {
        // Choose which cards to show depending on category chosen
        DeactivateMeshes();

        switch (categorySelected)
        {
            case Category.SourcesBoards:

                tabSource.SetActive (true);

                CardFromKey(obj.board).SetActive (true);
                if (ButtonFromKey(obj.board).interactable == false)
                CardFromKey(obj.laser).SetActive (true);
                ButtonFromKey(obj.laser).onClick.AddListener(delegate
                {
                    SelectAndPlaceTwo(ButtonFromKey(obj.laser), obj.laser, obj.propagation);
                });

            break;

            case Category.Lenses:

                tabLenses.SetActive (true);

                CardFromKey(obj.lens).SetActive (true);
                ButtonFromKey(obj.lens).onClick.AddListener(delegate
                {
                    SelectAndPlaceItem(ButtonFromKey(obj.lens), obj.lens);
                });

            break;

            case Category.SlitsGratings:

                tabSlit.SetActive (true);

                CardFromKey(obj.grating).SetActive (true);
                ButtonFromKey(obj.grating).onClick.AddListener(delegate
                {
                    SelectAndPlaceItem(ButtonFromKey(obj.grating), obj.grating);
                });
            break;

            case Category.Detectors:

                tabDet.SetActive (true);

                CardFromKey(obj.cmos).SetActive (true);
                ButtonFromKey(obj.cmos).onClick.AddListener(delegate
                {
                    SelectAndPlaceItem(ButtonFromKey(obj.cmos), obj.cmos);
                });
            break;
        }

    }

    private void DeactivateMeshes()
    {
        // Ensures that every time a category is selected, the cards are deactivated
        // and listeners removed (otherwise they will pile up and instantiate multiple meshes)
        foreach (MeshCard mesh in availableCards)
        {
            mesh.meshCard.SetActive (false);
        }

        foreach (MeshCard mesh in availableCards)
        {
            ButtonFromKey(mesh.item).onClick.RemoveListener(delegate
                {
                    SelectAndPlaceItem(ButtonFromKey(obj.laser), obj.laser);
                });
        }

        tabSource.SetActive(false);
        tabLenses.SetActive(false);
        tabSlit.SetActive(false);
        tabDet.SetActive(false);
    }

    private void SelectSourcesBoards()
    {
        categorySelected = Category.SourcesBoards;
        SetActiveMeshes(categorySelected);
    }

    private void RecolourSourcesBoards(bool sourcesTabActive)
    {
        // Bugs in Unity for selecting buttons dynamically through script, so have to force
        // colour change each time
        ColorBlock cb = sourcesBoards.colors;
        if (sourcesTabActive == true)
        {
            cb.normalColor = new Color32(17, 17, 17, 255);
            sourcesBoards.colors = cb;

        } else {

            cb.normalColor = new Color32(17, 17, 17, 64);
            sourcesBoards.colors = cb;
        }
    }

    private void SelectLenses()
    {
        RecolourSourcesBoards(false);

        categorySelected = Category.Lenses;
        SetActiveMeshes(categorySelected);
    }

    private void SelectSlitsGratings()
    {
        RecolourSourcesBoards(false);

        categorySelected = Category.SlitsGratings;
        SetActiveMeshes(categorySelected);
    }

    private void SelectDetectors()
    {
        RecolourSourcesBoards(false);

        categorySelected = Category.Detectors;
        SetActiveMeshes(categorySelected);
    }

    public override void CloseModal()
    {
        // Override abstract class method to add selection of first category as modal
        // is closed.
        SelectSourcesBoards();
        base.CloseModal();
    }

}