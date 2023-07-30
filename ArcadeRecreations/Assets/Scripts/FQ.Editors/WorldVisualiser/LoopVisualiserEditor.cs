using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace FQ.Editors
{
    
    public class LoopVisualiserEditor : EditorWindow
    {
        private static EditorWindow CurrentWindow;

        public const string WindowTitle = "LoopVisualiser";

        private Toggle toggle;

        private static bool layers;
        
        [MenuItem("Custom/Loop Visualiser")]
        public static void OpenWindow()
        {
            CurrentWindow  = GetWindow(typeof(LoopVisualiserEditor), false, WindowTitle, true);
        }
        
        public  void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // VisualElements objects can contain other VisualElement following a tree hierarchy
            Label label = new Label("Hello World!");
            root.Add(label);

            // Create toggle
            var toggle = new Toggle();
            toggle.name = "toggle";
            toggle.label = "Toggle";
            toggle.RegisterValueChangedCallback(OnToggleChange);
            toggle.value = layers;
            root.Add(toggle);

            // Create button
            Button button = new Button();
            button.name = "button";
            button.text = "Button";
            button.clicked += OnUpdateLoopableLayer;
            root.Add(button);
            
            FullLayerRefresh();
        }

        private void OnToggleChange(ChangeEvent<bool> value)
        {
            layers = value.newValue;
            FullLayerRefresh();
        }

        private void OnUpdateLoopableLayer()
        {
            layers = toggle.value;
            FullLayerRefresh();
        }

        private void FullLayerRefresh()
        {
            DeleteLayer(); 
            if (layers)
            {
                CreateLayer(); 
            }
        }

        private void DeleteLayer()
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("EditorOnly");
            if (gameObjects == null)
            {
                return;
            }
            
            foreach (var gameObject in gameObjects)
            {
                GameObject.DestroyImmediate(gameObject);
            }
        }

        private void CreateLayer()
        {
            var borderTile = Resources.Load<GameObject>("Editor/LoopVisualiser/LoopVisualiserTilemap");
            var arrowTile = Resources.Load<Tile>("Editor/LoopVisualiser/ArrowTiles/TileArrows_0");
            var tilemapGO = GameObject.Instantiate(borderTile);

            var tilemap = tilemapGO.GetComponentInChildren<Tilemap>();
            if (tilemap != null)
            {
                tilemap.SetTile(new Vector3Int(0, 0), arrowTile);
            }
        }
    }
}
