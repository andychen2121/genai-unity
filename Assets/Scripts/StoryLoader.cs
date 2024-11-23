using System.IO;
using UnityEngine;

[System.Serializable]
public class StoryNode
{
    public int nodeIndex;
    public string nodeText;
    public string[] choices;
    public string backgroundImage;
    public int[] nextNodeIndices;
}

[System.Serializable]
public class StoryData
{
    public StoryNode[] storyNodes;
}

public class StoryLoader : MonoBehaviour
{
    public TextAsset jsonFile; 
    public StoryData storyData; 
    public NodeManager nodeManager; 

    void Start()
    {
        LoadStoryData();
    }

    void LoadStoryData()
    {
        if (jsonFile == null)
        {
            Debug.LogError("JSON file not assigned. Please assign a valid JSON file in the Inspector.");
            return;
        }
        
        storyData = JsonUtility.FromJson<StoryData>(jsonFile.text);

        if (storyData == null || storyData.storyNodes.Length == 0)
        {
            Debug.LogError("Story data is empty or invalid. Check the JSON format.");
            return;
        }

        Debug.Log("Story Loaded. Total Nodes: " + storyData.storyNodes.Length);
        
        if (nodeManager != null)
        {
            NodeManager.StoryNode[] convertedNodes = new NodeManager.StoryNode[storyData.storyNodes.Length];
            for (int i = 0; i < storyData.storyNodes.Length; i++)
            {
                convertedNodes[i] = new NodeManager.StoryNode
                {
                    nodeText = storyData.storyNodes[i].nodeText,
                    choices = storyData.storyNodes[i].choices,
                    nextNodeIndices = storyData.storyNodes[i].nextNodeIndices,
                    backgroundImage = storyData.storyNodes[i].backgroundImage,
                    deathNode = false // Default value, can be modified if necessary
                };
            }

            nodeManager.InitializeStoryNodes(convertedNodes);
            Debug.Log("NodeManager's storyNodes array successfully populated.");
        }
        else
        {
            Debug.LogError("NodeManager reference is missing. Ensure it is assigned in the Inspector.");
        }
    }
}
