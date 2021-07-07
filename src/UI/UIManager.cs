﻿using Microsoft.Xna.Framework;
using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;
using System;
using TSMapEditor.Models;
using TSMapEditor.Mutations;
using TSMapEditor.Rendering;
using TSMapEditor.UI.CursorActions;
using TSMapEditor.UI.Sidebar;

namespace TSMapEditor.UI
{
    class UIManager : XNAControl
    {
        public UIManager(WindowManager windowManager, Map map, TheaterGraphics theaterGraphics) : base(windowManager)
        {
            this.map = map;
            this.theaterGraphics = theaterGraphics;
        }

        private readonly Map map;
        private readonly TheaterGraphics theaterGraphics;

        private MapView mapView;
        private TileSelector tileSelector;
        private EditorSidebar editorSidebar;

        private EditorState editorState;
        private TileInfoDisplay tileInfoDisplay;

        private TerrainPlacementAction terrainPlacementAction;

        private MutationManager mutationManager;

        public override void Initialize()
        {
            Name = nameof(UIManager);

            UISettings.ActiveSettings.PanelBackgroundColor = new Color(0, 0, 0, 128);
            UISettings.ActiveSettings.PanelBorderColor = new Color(128, 128, 128, 255);

            Width = WindowManager.RenderResolutionX;
            Height = WindowManager.RenderResolutionY;

            editorState = new EditorState();
            terrainPlacementAction = new TerrainPlacementAction();

            mutationManager = new MutationManager();

            // Keyboard must be initialized before any other controls so it's properly usable
            KeyboardCommands.Instance = new KeyboardCommands();
            KeyboardCommands.Instance.Undo.Action = UndoAction;
            KeyboardCommands.Instance.Redo.Action = RedoAction;

            mapView = new MapView(WindowManager, map, theaterGraphics, editorState, mutationManager);
            mapView.Width = WindowManager.RenderResolutionX;
            mapView.Height = WindowManager.RenderResolutionY;
            AddChild(mapView);

            editorSidebar = new EditorSidebar(WindowManager, map, theaterGraphics);
            editorSidebar.Width = 250;
            editorSidebar.Y = 40;
            editorSidebar.Height = WindowManager.RenderResolutionY - editorSidebar.Y;
            AddChild(editorSidebar);

            tileSelector = new TileSelector(WindowManager, theaterGraphics);
            tileSelector.X = editorSidebar.Right;
            tileSelector.Width = WindowManager.RenderResolutionX - tileSelector.X;
            tileSelector.Height = 300;
            tileSelector.Y = WindowManager.RenderResolutionY - tileSelector.Height;
            
            AddChild(tileSelector);
            tileSelector.TileDisplay.SelectedTileChanged += TileDisplay_SelectedTileChanged;

            tileInfoDisplay = new TileInfoDisplay(WindowManager, theaterGraphics);
            AddChild(tileInfoDisplay);
            tileInfoDisplay.X = Width - tileInfoDisplay.Width;
            mapView.TileInfoDisplay = tileInfoDisplay;

            base.Initialize();

            Keyboard.OnKeyPressed += Keyboard_OnKeyPressed;
        }

        private void Keyboard_OnKeyPressed(object sender, Rampastring.XNAUI.Input.KeyPressEventArgs e)
        {
            if (!WindowManager.HasFocus)
                return;

            if (!IsActive)
                return;

            var selectedControl = WindowManager.SelectedControl;
            if (selectedControl != null)
            {
                if (selectedControl is XNATextBox || selectedControl is XNAListBox)
                    return;
            }

            foreach (var keyboardCommand in KeyboardCommands.Instance.Commands)
            {
                if (e.PressedKey == keyboardCommand.Key.Key)
                {
                    // Key matches, check modifiers

                    if ((keyboardCommand.Key.Modifiers & KeyboardModifiers.Alt) == KeyboardModifiers.Alt && !Keyboard.IsAltHeldDown())
                        continue;

                    if ((keyboardCommand.Key.Modifiers & KeyboardModifiers.Ctrl) == KeyboardModifiers.Ctrl && !Keyboard.IsCtrlHeldDown())
                        continue;

                    if ((keyboardCommand.Key.Modifiers & KeyboardModifiers.Shift) == KeyboardModifiers.Shift && !Keyboard.IsShiftHeldDown())
                        continue;

                    // All keys match, perform the command!
                    e.Handled = true;

                    keyboardCommand.Action?.Invoke();
                    break;
                }
            }
        }

        private void TileDisplay_SelectedTileChanged(object sender, EventArgs e)
        {
            mapView.CursorAction = terrainPlacementAction;
            terrainPlacementAction.Tile = tileSelector.TileDisplay.SelectedTile;
        }

        private void UndoAction()
        {
            mutationManager.Undo();
        }

        private void RedoAction()
        {
            mutationManager.Redo();
        }
    }
}
