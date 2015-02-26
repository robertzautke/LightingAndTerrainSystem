/*
 * This script creates the terrain generator window and calls all of the various algorithms scripts from here.
 * In order to add more algorithms or models in you need to add to the enum, add the the switch statements
 * in the appropriate places, and add the menus to change paramters.
 */

using UnityEditor;
using UnityEngine;
using System;

public class TerrainGenerator : EditorWindow
{
    // Set up alogorithms
    public enum AlgorithmType { Random, Perlin, DiamondSquare };
    AlgorithmType type = AlgorithmType.Perlin;

    public enum ErosionType { Thermal, ImprovedThermal}
    ErosionType erodeType = ErosionType.Thermal;

    GameObject terrain;

    // Init the generators
    RandomGenerator randomGen = new RandomGenerator();
    PerlinNoiseGenerator perlinGen = new PerlinNoiseGenerator();
    DiamondSquare diamondSquareGen = new DiamondSquare();

    ThermalErosion thermalGen = new ThermalErosion();
    ImprovedThermalErosion improvedThermalGen = new ImprovedThermalErosion();

    // Save the last heightmap in case the user wants to undo the last action
    float[,] undoArray;

    int smoothenIterations = 1;

    // Add menu item named "TerrainGenerator" to the Window menu
    [MenuItem("Window/TerrainGenerator")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(TerrainGenerator));
    }

    // Redraws the menu on each GUI draw
    void OnGUI()
    {
        // Field to attach the terrain to generate too.
        GUILayout.Label("Terrain Generator", EditorStyles.boldLabel);
        GUILayout.Space(16);
        terrain = (GameObject)EditorGUILayout.ObjectField("Terrain", terrain, typeof(UnityEngine.Object), true);

        //================= Terrain Generation =================\\
        GUILayout.Space(16);
        GUILayout.Label("Generation:", EditorStyles.boldLabel);

        // Select Algorithm and show options
        type = (AlgorithmType)EditorGUILayout.EnumPopup("Algorithm to Use:", type);
        switch (type)
        {
            case AlgorithmType.Random:
                ShowRandomGuiOptions();
                break;
            case AlgorithmType.Perlin:
                ShowPerlinGuiOptions();
                break;
            case AlgorithmType.DiamondSquare:
                ShowDiamondSquareGuiOptions();
                break;
        }

        // Run selected algorithm if possible
        if (GUILayout.Button("Generate!"))
        {
            if (terrain == null || terrain.GetComponent<Terrain>() == null)
                ShowNotification(new GUIContent("Terrain Field is empty or not a valid Terrain!"));
            else
            {
                // Set the undo array
                Terrain ter = terrain.GetComponent<Terrain>();
                TerrainData terrainData = ter.terrainData;
                int w = terrainData.heightmapWidth;
                int h = terrainData.heightmapWidth;

                undoArray = terrainData.GetHeights(0, 0, w, h);

                switch (type)
                {
                    case AlgorithmType.Random:
                        randomGen.Generate(terrain);
                        break;
                    case AlgorithmType.Perlin:
                        perlinGen.Generate(terrain);
                        break;
                    case AlgorithmType.DiamondSquare:
                        diamondSquareGen.Generate(terrain);
                        break;
                }
            }
        }

        //================= Erosion Generation =================\\
        GUILayout.Space(16);
        GUILayout.Label("Erosion:", EditorStyles.boldLabel);
        erodeType = (ErosionType)EditorGUILayout.EnumPopup("Algorithm to Use:", erodeType);

        // Select erosion type
        switch (erodeType)
        {
            case ErosionType.Thermal:
                ShowThermalGuiOptions();
                break;
            case ErosionType.ImprovedThermal:
                ShowImprovedThermalGuiOptions();
                break;
        }

        // Erode if possible
        if (GUILayout.Button("Erode!"))
        {
            if (terrain == null || terrain.GetComponent<Terrain>() == null)
                ShowNotification(new GUIContent("Terrain Field is empty or not a valid Terrain!"));
            else
            {
                // Set the undo array
                Terrain ter = terrain.GetComponent<Terrain>();
                TerrainData terrainData = ter.terrainData;
                int w = terrainData.heightmapWidth;
                int h = terrainData.heightmapWidth;

                undoArray = terrainData.GetHeights(0, 0, w, h);
                switch (erodeType)
                {
                    case ErosionType.Thermal:
                        thermalGen.Erode(terrain);
                        break;
                    case ErosionType.ImprovedThermal:
                        improvedThermalGen.Erode(terrain);
                        break;
                }
            }
        }

        //================= Smoothing =================\\
        GUILayout.Space(16);
        GUILayout.Label("Smooth:", EditorStyles.boldLabel);
        smoothenIterations = EditorGUILayout.IntField("Iterations: ", smoothenIterations);

        if (GUILayout.Button("Smooth!"))
        {
            // Set the undo array
            Terrain ter = terrain.GetComponent<Terrain>();
            TerrainData terrainData = ter.terrainData;
            int w = terrainData.heightmapWidth;
            int h = terrainData.heightmapWidth;

            undoArray = terrainData.GetHeights(0, 0, w, h);

            Smoother.Smoothen(terrain, smoothenIterations);
        }


        //================= Undo =================\\
        GUILayout.Space(16);
        if (GUILayout.Button("Undo Last Action!"))
        {
            if (terrain == null || terrain.GetComponent<Terrain>() == null || undoArray == null)
                ShowNotification(new GUIContent("Can't undo!"));
            else
            {
                Terrain ter = terrain.GetComponent<Terrain>();
                TerrainData terrainData = ter.terrainData;
                terrainData.SetHeights(0, 0, undoArray);
            }
        }
    }

    // Helper functions to show options for the various algorithms

    void ShowRandomGuiOptions()
    {
        randomGen.minY = EditorGUILayout.Slider("Minimum Y height: ", randomGen.minY, 0, 1);
        randomGen.maxY = EditorGUILayout.Slider("Maximum Y height: ", randomGen.maxY, 0, 1);
    }

    void ShowPerlinGuiOptions()
    {
        perlinGen.ocataveCount = EditorGUILayout.IntSlider("Octaves: ", perlinGen.ocataveCount, 1, 16);
        perlinGen.freq = EditorGUILayout.Slider("Frequency: ", perlinGen.freq, 1, 16);
        perlinGen.persistence = EditorGUILayout.Slider("Persistence: ", perlinGen.persistence, 0,1);
    }

    void ShowDiamondSquareGuiOptions()
    {
        diamondSquareGen.scaleMod = EditorGUILayout.Slider("Scale: ", diamondSquareGen.scaleMod, 0, 16);
        diamondSquareGen.featureSize = EditorGUILayout.IntPopup("Intial Square Size: ", diamondSquareGen.featureSize, new string[]{"2", "4", "8", "16", "32", "64", "128", "256", "512", "1024", "2048"}, new int[] {2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048});

    }

    void ShowThermalGuiOptions()
    {
        thermalGen.iterations = EditorGUILayout.IntField("Iterations: ", thermalGen.iterations);
        thermalGen.smoothness = EditorGUILayout.FloatField("Smoothness: ", thermalGen.smoothness);
        thermalGen.doSmooth = EditorGUILayout.Toggle("Smooth? ", thermalGen.doSmooth);
    }

    void ShowImprovedThermalGuiOptions()
    {
        improvedThermalGen.iterations = EditorGUILayout.IntField("Iterations: ", improvedThermalGen.iterations);
        improvedThermalGen.smoothness = EditorGUILayout.FloatField("Smoothess: ", improvedThermalGen.smoothness);
        improvedThermalGen.doSmooth = EditorGUILayout.Toggle("Smooth? ", improvedThermalGen.doSmooth);
    }
}