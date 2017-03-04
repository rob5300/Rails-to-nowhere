using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {

    public Carriage startCarriage;
    List<GameObject> _storyCarriages = new List<GameObject>();
    List<GameObject> _fillerCarriages = new List<GameObject>();
    List<GameObject> _carriagesToPlace = new List<GameObject>();

    Vector3 _placementPoint;
    GameObject _selectedCarriage;

    public int TotalCarriageAmount { get { return StoryCarriageAmount + FillerCarriageAmount; }}
    public int StoryCarriageAmount = 1;
    public int FillerCarriageAmount = 0;

    public void OnEnable() {
        SceneManager.sceneLoaded += LevelLoad;
    }

    public void OnDisable() {
        SceneManager.sceneLoaded -= LevelLoad;
    }

    public void LevelLoad(Scene scene, LoadSceneMode mode) {
        PopulateCarriageLists();
        GenerateLevel();
    }

    void PopulateCarriageLists() {
        System.Random random = new System.Random();
        //Load in Carriages
        _storyCarriages.Add(Resources.Load<GameObject>("Carriages/StoryCarriage1"));
        _fillerCarriages.Add(Resources.Load<GameObject>("Carriages/FillerCarriage1"));

        int storyCount = 0;
        int storyPlaced = 0;
        int fillerCount = 0;
        GameObject toAddCandidate;
        //Select random carriages and place into list.
        while (_carriagesToPlace.Count < TotalCarriageAmount) {
            if(storyPlaced < Mathf.Floor(StoryCarriageAmount / FillerCarriageAmount)) {
                //Add Story Carriage
                while (true) {
                    toAddCandidate = _storyCarriages[random.Next(_storyCarriages.Count - 1)];
                    if (!_carriagesToPlace.Contains(toAddCandidate)) {
                        _carriagesToPlace.Add(toAddCandidate);
                        storyCount++;
                        storyPlaced++;
                        break;
                    }
                    else if(_storyCarriages.Count <= storyCount){
                        //We need to check if there are not enough carriages to have no duplicates, if there are not then we must accept this selected carriage.
                        _carriagesToPlace.Add(toAddCandidate);
                        storyPlaced++;
                        storyCount++;
                        break;
                    }
                }

            }
            else {
                //Add Filler Carriage
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

    void GenerateLevel() {
        Carriage previousCarriage = startCarriage;
        GameObject placing;
        Carriage placingCarriage;
        foreach(GameObject carriage in _carriagesToPlace) {
            placing = (GameObject)Instantiate(carriage, Vector3.zero, Quaternion.identity);
            placingCarriage = placing.GetComponent<Carriage>();
            Vector3 positionToPlaceAt = previousCarriage.FrontMountPoint.position - placingCarriage.RearMountPoint.position;
            placing.transform.position = positionToPlaceAt;
            previousCarriage = placingCarriage;
        }
    }
}
