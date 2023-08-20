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
    /// <summary>
    /// Creates and manages the visualiser for the world loop layer.
    /// </summary>
    public class LoopVisualiserEditor : EditorWindow
    {
        /// <summary>
        /// True means show World Loop Layers.
        /// </summary>
        private bool worldLoopLayers;
        
        /// <summary>
        /// Add the window open to the window open location.
        /// </summary>
        [MenuItem("Custom/Loop Visualiser")]
        public static void OpenWindow()
        {
            string windowTitle = "Loop Visualiser";
            GetWindow(typeof(LoopVisualiserEditor), false, windowTitle, true);
        }
        
        /// <summary>
        /// Create the GUI for the editor.
        /// </summary>
        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Assets/Scripts/FQ.Editors/WorldVisualiser/VisualiseUI.uxml");
            root.Add(visualTree.Instantiate());

            SetupWorldLoopToggle(root);
            SetupRefreshWorldLoopButton(root);

            WorldLoopRefresh();
        }

        /// <summary>
        /// Sets up the button binds for Refresh World Loop.
        /// </summary>
        /// <param name="root">UI Root. </param>
        private void SetupRefreshWorldLoopButton(VisualElement root)
        {
            VisualElement element = FindElementByName(root, "RefreshWorldLoop"); 
            Button worldLoop = ExtractButton(element);
            if (worldLoop != null)
            {
                worldLoop.clicked += WorldLoopRefresh; 
            }
        }

        /// <summary>
        /// Setup world loop toggle binds.
        /// </summary>
        /// <param name="root">UI Root. </param>
        private void SetupWorldLoopToggle(VisualElement root)
        {
            VisualElement element = FindElementByName(root, "WorldLoop");
            Toggle worldLoop = ExtractToggle(element);
            if (worldLoop != null)
            {
                worldLoop.RegisterValueChangedCallback(OnToggleChange);
            }
        }

        /// <summary>
        /// Extract Toggle from Element.
        /// </summary>
        /// <param name="element">UI Element. </param>
        /// <returns>Toggle if found. </returns>
        private Toggle ExtractToggle(VisualElement element)
        {
            Toggle returnToggle = null;
            if (element is Toggle t)
            {
                returnToggle = t;
            }

            return returnToggle;
        }
        
        /// <summary>
        /// Extract Button from Element.
        /// </summary>
        /// <param name="element">UI Element. </param>
        /// <returns>Button if found. </returns>
        private Button ExtractButton(VisualElement element)
        {
            Button returnToggle = null;
            if (element is Button t)
            {
                returnToggle = t;
            }

            return returnToggle;
        }

        /// <summary>
        /// Finds an element by name.
        /// </summary>
        /// <param name="root">UI Root. </param>
        /// <param name="name">Name to search for. </param>
        /// <returns>Element or null if not found. </returns>
        private VisualElement FindElementByName(VisualElement root, string name)
        {
            foreach (var child in root.Children())
            {
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

        /// <summary>
        /// Called when the toggle is changed.
        /// </summary>
        /// <param name="value">Change event containing old and new value. </param>
        private void OnToggleChange(ChangeEvent<bool> value)
        {
            worldLoopLayers = value.newValue;
            WorldLoopRefresh();
        }

        /// <summary>
        /// Refreshes the World Loop Layer.
        /// </summary>
        private void WorldLoopRefresh()
        {
            DeleteWorldLoopLayer(); 
            if (worldLoopLayers)
            {
                CreateWorldLoopLayer(); 
            }
        }

        /// <summary>
        /// Delete World Loop Layer.
        /// </summary>
        private void DeleteWorldLoopLayer()
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

        /// <summary>
        /// Creates world loop layer.
        /// </summary>
        private void CreateWorldLoopLayer()
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
