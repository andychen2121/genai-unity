using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodeManager : MonoBehaviour
{
    [System.Serializable]
    public class StoryNode
    {
        public string nodeText;          
        public string[] choices;         
        public int[] nextNodeIndices;    
        public string backgroundImage;   
        public bool deathNode;           
    }

    public StoryNode[] storyNodes;       
    public TextMeshProUGUI plotText;     
    public Button[] choiceButtons;      
    public TextMeshProUGUI[] choiceTexts; 
    public Image backgroundImageUI;      

    private int currentNodeIndex = 0;
    private int checkpointNodeIndex = 0; 

    public void InitializeStoryNodes(StoryNode[] loadedNodes)
    {
        if (loadedNodes == null || loadedNodes.Length == 0)
        {
            Debug.LogError("Loaded story nodes array is empty or null. Ensure valid data is passed.");
            return;
        }

        storyNodes = loadedNodes;
        currentNodeIndex = 0;
        checkpointNodeIndex = 0;

        LoadNode(currentNodeIndex);
    }

    void LoadNode(int nodeIndex)
    {
        if (nodeIndex < 0 || nodeIndex >= storyNodes.Length)
        {
            Debug.LogError($"Invalid node index: {nodeIndex}. Check your storyNodes array.");
            return;
        }

        StoryNode currentNode = storyNodes[nodeIndex];
        if (currentNode == null)
        {
            Debug.LogError($"Node at index {nodeIndex} is null. Check your storyNodes setup.");
            return;
        }
        
        Sprite newBackground = Resources.Load<Sprite>(currentNode.backgroundImage);
        if (newBackground != null)
        {
            backgroundImageUI.sprite = newBackground;
        }
        else
        {
            Debug.LogWarning($"Background image {currentNode.backgroundImage} not found in Resources.");
        }
        
        plotText.text = currentNode.nodeText;
        
        if (currentNode.deathNode)
        {
            Debug.Log("Player has reached a death node. Returning to the last checkpoint...");
            currentNodeIndex = checkpointNodeIndex;
            LoadNode(currentNodeIndex);
            return;
        }
        
        for (int i = 0; i < choiceTexts.Length; i++)
        {
            if (i < currentNode.choices.Length)
            {
                choiceButtons[i].gameObject.SetActive(true); 
                choiceTexts[i].text = currentNode.choices[i]; 
                
                int choiceIndex = i; 
                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(choiceIndex));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
                choiceTexts[i].text = "";
            }
        }
        
        checkpointNodeIndex = nodeIndex;
    }

    void OnChoiceSelected(int choiceIndex)
    {
        if (choiceIndex < 0 || choiceIndex >= storyNodes[currentNodeIndex].nextNodeIndices.Length)
        {
            Debug.LogError($"Invalid choice index: {choiceIndex}. Check the nextNodeIndices array for node {currentNodeIndex}.");
            return;
        }
        
        int nextNodeIndex = storyNodes[currentNodeIndex].nextNodeIndices[choiceIndex];
        Debug.Log($"Transitioning from node {currentNodeIndex} to node {nextNodeIndex}.");
        currentNodeIndex = nextNodeIndex;
        LoadNode(currentNodeIndex);
    }
}
