using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace FQ.Editors
{
    
    public class LoopVisualiserEditor : EditorWindow
    {
        private bool layers;
        
        [MenuItem("Custom/Loop Visualiser")]
        public static void OpenWindow()
        {
            string windowTitle = "Loop Visualiser";
            GetWindow(typeof(LoopVisualiserEditor), false, windowTitle, true);
        }
        
        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Assets/Scripts/FQ.Editors/WorldVisualiser/VisualiseUI.uxml");
            root.Add(visualTree.Instantiate());

            SetupWorldLoopToggle(root);
            SetupRefreshWorldLoopButton(root);

            FullLayerRefresh();
        }

        private void SetupRefreshWorldLoopButton(VisualElement root)
        {
            VisualElement element = FindElementByName(root, "RefreshWorldLoop"); 
            Button worldLoop = ExtractButton(element);
            if (worldLoop != null)
            {
                worldLoop.clicked += FullLayerRefresh; 
            }
        }

        private void SetupWorldLoopToggle(VisualElement root)
        {
            VisualElement element = FindElementByName(root, "WorldLoop");
            Toggle worldLoop = ExtractToggle(element);
            if (worldLoop != null)
            {
                worldLoop.RegisterValueChangedCallback(OnToggleChange);
            }
        }

        private Toggle ExtractToggle(VisualElement element)
        {
            Toggle returnToggle = null;
            if (element is Toggle t)
            {
                returnToggle = t;
            }

            return returnToggle;
        }
        
        private Button ExtractButton(VisualElement element)
        {
            Button returnToggle = null;
            if (element is Button t)
            {
                returnToggle = t;
            }

            return returnToggle;
        }

        private VisualElement FindElementByName(VisualElement root, string name)
        {
            foreach (var child in root.Children())
            {
                Debug.Log($"Name: {child.name} == {name}");
                if (child.name == name) 
                {
                    return child;
                }
                
                VisualElement ve = FindElementByName(child, name);
                if (ve != null)
                {
                    return ve;
                }
            }

            return null;
        }

        private void OnToggleChange(ChangeEvent<bool> value)
        {
            layers = value.newValue;
            FullLayerRefresh();
        }

        private void ToggleWorldLoop()
        {
            ChangeEvent<bool> value = ChangeEvent<bool>.GetPooled(layers, !layers);
            OnToggleChange(value);
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
            
            foreach (var gameObject in gameObjects.Where(x => x.name.Contains("LoopVisualiserTilemap")))
            {
                DestroyImmediate(gameObject);
            }
        }

        private void CreateLayer()
        {
            var visualiserPrefab = Resources.Load<GameObject>("Editor/LoopVisualiser/LoopVisualiserTilemap");
            var borderTile = Resources.Load<Tile>("World/Tiles/BasicChecker-A/BasicChecker-A-Tile");
            GameObject map = GameObject.FindGameObjectWithTag("SnakeBorder");
            if (map != null)
            {
                var tilemap = map.GetComponentInChildren<Tilemap>();
                if (tilemap != null)
                {
                    var arrow = new SimpleArrowTileProvider("Editor/LoopVisualiser/ArrowTiles/TileArrows_");
                    new LoopVisualiser().AddVisualisationObject(visualiserPrefab, tilemap, borderTile, arrow);
                }
            }
        }
    }
}
