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
    public MeshID item;
    public GameObject meshCard;

    public MeshCard(MeshID item, GameObject meshCard)
    {
        this.item = item;
        this.meshCard = meshCard;
    }
}

public class InventoryModal : Modal
{
    // Add all relevant objects to the inspector - modal, buttons, locks etc.
    [SerializeField]
    private GameObject _inventoryModal;

    [SerializeField]
    private List<MeshCard> availableCards = new List<MeshCard>();

    public Dictionary<MeshID, GameObject> availableCardsDict = new Dictionary<MeshID, GameObject>();


    [SerializeField]
    private GameObject[] locks;

    [SerializeField]
    private Button sourcesBoards, lenses, slitsGratings, detectors;

    [SerializeField]
    private GameObject tabSource, tabLenses, tabSlit, tabDet;

    public Category categorySelected;

    private void DictionaryMeshCards()
    {
        // Assigns a key to each card to ease calling each object in program
        foreach(MeshCard card in availableCards)
        {
            availableCardsDict.Add(card.item, card.meshCard);
        }
    }

    private GameObject CardFromKey(MeshID item)
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

    private Button ButtonFromKey(MeshID item)
    {
        // Retrieves the required button from the card
        return CardFromKey(item).GetComponent<Button>();
    }


    void Start()
    {
        // Add close and optical board listeners, lock everything else
        TextMeshProUGUI sourcesText = sourcesBoards.GetComponent<TextMeshProUGUI>();
        sourcesText.color = new Color32(17, 17, 17, 255);
        LockCategories();
        CloseListeners(_inventoryModal);
        ButtonFromKey(MeshID.board).onClick.AddListener(PlaceBoard);
    }

    private void LockCategories()
    {
        // Lock categories by making buttons non interactive for now
        Button[] categories = new Button[] {sourcesBoards, lenses, slitsGratings, detectors};
        foreach(Button category in categories)
        {
            category.interactable = false;
        }
    }

    private void SelectAndPlaceItem(Button thisClick, MeshID item)
    {
        // Functionality for instantiating prefabs, closing the inventory as it is 
        // introduced onto the scene
        MeshesInventory.Instance.InstantiateItem(item);
        CloseModal(_inventoryModal);
        thisClick.interactable = false;
    }

    private void SelectAndPlaceTwo(Button thisClick, MeshID mesh, MeshID manager)
    {
        // Functionality for instantiating prefabs, closing the inventory as it is 
        // introduced onto the scene
        MeshesInventory.Instance.InstantiateTwo(mesh, manager);
        CloseModal(_inventoryModal);
        thisClick.interactable = false;
    }

    private void PlaceBoard()
    {
        // Placing the board onto the scene unlocks all categories
        SelectAndPlaceItem(ButtonFromKey(MeshID.board), MeshID.board);
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

                CardFromKey(MeshID.board).SetActive (true);
                CardFromKey(MeshID.laser).SetActive (true);
                ButtonFromKey(MeshID.laser).onClick.AddListener(delegate
                {
                    SelectAndPlaceTwo(ButtonFromKey(MeshID.laser), MeshID.laser, MeshID.propagationSystem);
                });

            break;

            case Category.Lenses:

                tabLenses.SetActive (true);

                CardFromKey(MeshID.lens).SetActive (true);
                ButtonFromKey(MeshID.lens).onClick.AddListener(delegate
                {
                    SelectAndPlaceItem(ButtonFromKey(MeshID.lens), MeshID.lens);
                });

            break;

            case Category.SlitsGratings:

                tabSlit.SetActive (true);

                CardFromKey(MeshID.grating).SetActive (true);
                ButtonFromKey(MeshID.grating).onClick.AddListener(delegate
                {
                    SelectAndPlaceItem(ButtonFromKey(MeshID.grating), MeshID.grating);
                });
            break;

            case Category.Detectors:

                tabDet.SetActive (true);

                CardFromKey(MeshID.screen).SetActive (true);
                ButtonFromKey(MeshID.screen).onClick.AddListener(delegate
                {
                    SelectAndPlaceTwo(ButtonFromKey(MeshID.screen), MeshID.screen, MeshID.emailManager);
                });

                CardFromKey(MeshID.cmosCamera).SetActive (true);
                ButtonFromKey(MeshID.cmosCamera).onClick.AddListener(delegate
                {
                    SelectAndPlaceItem(ButtonFromKey(MeshID.cmosCamera), MeshID.cmosCamera);
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
                    SelectAndPlaceItem(ButtonFromKey(MeshID.laser), MeshID.laser);
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

    private void SelectLenses()
    {
        categorySelected = Category.Lenses;
        SetActiveMeshes(categorySelected);
    }

    private void SelectSlitsGratings()
    {
        categorySelected = Category.SlitsGratings;
        SetActiveMeshes(categorySelected);
    }

    private void SelectDetectors()
    {
        categorySelected = Category.Detectors;
        SetActiveMeshes(categorySelected);
    }

    public override void CloseModal(GameObject _modal)
    {
        // Override abstract class method to add selection of first category as modal
        // is closed.
        base.CloseModal(_modal);
        SelectSourcesBoards();
    }

}