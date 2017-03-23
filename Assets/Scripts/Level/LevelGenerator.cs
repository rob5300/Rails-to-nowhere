using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class LevelGenerator : MonoBehaviour {

    public Carriage startCarriage;
    List<GameObject> _storyCarriages = new List<GameObject>();
    List<GameObject> _fillerCarriages = new List<GameObject>();
    List<GameObject> _carriagesToPlace = new List<GameObject>();
    List<GameObject> _3DPuzzlesToPlace = new List<GameObject>();
	List<GameObject> _2DPuzzlesToPlace = new List<GameObject>();
    List<NPC> _carriageNPCs = new List<NPC>();

    Vector3 _placementPoint;
    GameObject _selectedCarriage;

    public int TotalCarriageAmount { get { return StoryCarriageAmount + FillerCarriageAmount; } }
    public int StoryCarriageAmount = 1;
    public int FillerCarriageAmount = 0;
    int storyCount = 0;
    int storyPlaced = 0;
    int fillerCount = 0;
    System.Random random = new System.Random();

    public void OnEnable() {
        SceneManager.sceneLoaded += LevelLoad;
    }

    public void OnDisable() {
        SceneManager.sceneLoaded -= LevelLoad;
    }

    public void LevelLoad(Scene scene, LoadSceneMode mode) {
        PopulateObjectLists();
        PopulateCarriageLists();
        GenerateLevel();
    }

    void PopulateObjectLists() {
        //Load Story NPCS.
        StoryNPC.LoadStoryNPCs();
        //Load Filler NPCS.
        FillerNPC.LoadFillerNPCSDUMMY();
        //Load in Filler carriages.
        _fillerCarriages.Add(Resources.Load<GameObject>("Carriages/FillerCarriage1"));
        CogPuzzle.Load3DPuzzlePrefabs();
    }

    void PopulateCarriageLists() {
        //We will pick story npcs first then add their carriage.

        if (StoryCarriageAmount < 1) {
            StoryCarriageAmount = 1;
            Debug.LogError("Story Carriage Amount was Zero (0). Generation will continue with one forced Story Carriage.");
        }
        storyCount = 0;
        storyPlaced = 0;
        fillerCount = 0;
        GameObject toAddCandidate;
        StoryNPC storyNPCCandidate;
        FillerNPC fillerNPCCandidate;
        //Select random carriages and place into list.
        while (_carriagesToPlace.Count < TotalCarriageAmount) {
            //The logic here is moved to a method to allow the filler carriage count to be 0. This avoids a DevideByZero.
            if (StoryToFillerCalc()) {
                //Add Story Carriage
                //Select NPC
                storyNPCCandidate = SelectStoryNPC();
                //Get the npcs carriage and add that to be placed.
                _carriagesToPlace.Add(storyNPCCandidate.Carriage);
                //Select a 3d Puzzle to use.
                Select3DPuzzle();
				_2DPuzzlesToPlace.Add(Select2DPuzzle());
                storyCount++;
                storyPlaced++;
            }
            else {
                //Add Filler Carriage
                fillerNPCCandidate = SelectFillerNPC();
                while (true) {
                    toAddCandidate = _fillerCarriages[random.Next(_fillerCarriages.Count - 1)];
                    if (!_carriagesToPlace.Contains(toAddCandidate)) {
                        _carriagesToPlace.Add(toAddCandidate);
                        fillerCount++;
                        storyPlaced = 0;
                        break;
                    }
                    else if (_fillerCarriages.Count <= fillerCount) {
                        //We need to check if there are not enough carriages to have no duplicates, if there are not then we must accept this selected carriage.
                        _carriagesToPlace.Add(toAddCandidate);
                        fillerCount++;
                        storyPlaced = 0;
                        break;
                    }
                }
            }
        }
    }

    private void Select3DPuzzle() {
        GameObject puzzleObject;
        while (true) {
            int rand;
            if (CogPuzzle.PuzzleList.Count == 1) {
                rand = 0;
            }
            else {
                rand = random.Next(0, CogPuzzle.PuzzleList.Count);
            }
            puzzleObject = CogPuzzle.PuzzleList[rand];
            if (!_3DPuzzlesToPlace.Contains(puzzleObject)) {
                _3DPuzzlesToPlace.Add(puzzleObject);
                break;
            }
            //Accept this puzzle as we only have 1 to use or we have no more unique puzzles.
            else if (CogPuzzle.PuzzleList.Count <= 1 || CogPuzzle.PuzzleList.Count <= _3DPuzzlesToPlace.Count) {
                _3DPuzzlesToPlace.Add(puzzleObject);
                break;
            }
        }
    }

	private GameObject Select2DPuzzle()
	{
		 return Resources.Load<GameObject>("2DPuzzles/Electrical Puzzle");
	}

    private StoryNPC SelectStoryNPC() {
        StoryNPC toreturn;
        if (StoryNPC.StoryNPCs.Count == 0) {
            Debug.LogError("Story NPC list is empty");
            return null;
        }

        while (true) {
            toreturn = StoryNPC.StoryNPCs[random.Next(StoryNPC.StoryNPCs.Count - 1)];
            if (!_carriageNPCs.Contains(toreturn)) {
                _carriageNPCs.Add(toreturn);
                break;
            }
            //Accept this npc as we only have 1 to use.
            else if (StoryNPC.StoryNPCs.Count <= 1) {
                _carriageNPCs.Add(toreturn);
                break;
            }
        }
        return toreturn;
    }

    private FillerNPC SelectFillerNPC() {
        FillerNPC toreturn;
        if (FillerNPC.FillerNPCs.Count == 0) {
            Debug.LogError("Filler NPC list is empty");
            return null;
        }

        while (true) {
            toreturn = FillerNPC.FillerNPCs[random.Next(StoryNPC.StoryNPCs.Count - 1)];
            if (!_carriageNPCs.Contains(toreturn)) {
                _carriageNPCs.Add(toreturn);
                break;
            }
            //Accept this npc as we only have 1 to use.
            else if (StoryNPC.StoryNPCs.Count <= 1) {
                _carriageNPCs.Add(toreturn);
                break;
            }
        }
        return toreturn;
    }

    bool StoryToFillerCalc() {
        //returns true to place story carriage. returns false to place a filler carriage.

        float storyf = StoryCarriageAmount;
        float fillerf = FillerCarriageAmount;
        //If the filler amount is 0 OR if the filler count is now the same as the amount placed, return true.
        if (FillerCarriageAmount < 1 || fillerCount >= FillerCarriageAmount) return true;
        //If the ratio is even, subtract one from the calculation.
        if ((storyf % fillerf) == 0) {
            return storyPlaced < Mathf.Floor(storyf / fillerf) - 1;
        }
        //Ratio is odd
        else {
            return storyPlaced < Mathf.Floor(storyf / fillerf);
        }

    }

    void GenerateLevel() {
        Carriage previousCarriage = startCarriage;
        GameObject placing;
        Carriage placingCarriage;
        foreach (GameObject carriage in _carriagesToPlace) {
            //We place the carriage and position it.
            placing = (GameObject)Instantiate(carriage, Vector3.zero, Quaternion.identity);
            placingCarriage = placing.GetComponent<Carriage>();
            Vector3 positionToPlaceAt = previousCarriage.FrontMountPoint.position - placingCarriage.RearMountPoint.position;
            placing.transform.position = positionToPlaceAt;
            previousCarriage = placingCarriage;

            //We give the carriage the npc to add and the puzzle to add.
            //We HAVE to remove it after we are finished to ensure that the element at index 0 is the next one to place.
            if (_carriageNPCs[0] is StoryNPC) {
                placingCarriage.PlaceCarriageNPC((StoryNPC)_carriageNPCs[0]);
            }
            else if (_carriageNPCs[0] is FillerNPC) {
                placingCarriage.PlaceCarriageNPC((FillerNPC)_carriageNPCs[0]);
            }
            _carriageNPCs.RemoveAt(0);

            if (placingCarriage.Type == Carriage.CarriageType.Story) {
                placingCarriage.Place3DPuzzle(_3DPuzzlesToPlace[0]);
                placingCarriage.Place2DPuzzle(_2DPuzzlesToPlace[0]);
                _2DPuzzlesToPlace.RemoveAt(0);
                _3DPuzzlesToPlace.RemoveAt(0); 
            }
        }
    }
}
