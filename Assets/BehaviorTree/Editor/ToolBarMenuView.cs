using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BT
{
    public class ToolBarMenuView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<ToolBarMenuView, UxmlTraits> { }

        public void Initialize(BehaviorTreeEditor behaviorTreeEditor)
        {

        }

        public ToolBarMenuView()
        {
            Toolbar toolbar = new Toolbar();

            // Create the dropdown menu button
            ToolbarMenu dropdownMenu = new ToolbarMenu();
            dropdownMenu.text = "File";

            // Add items to the dropdown
            dropdownMenu.menu.AppendAction("Action 1", (DropdownMenuAction action) => { Debug.Log("Action 1 selected"); });
            dropdownMenu.menu.AppendAction("Action 2", (DropdownMenuAction action) => { Debug.Log("Action 2 selected"); });
            dropdownMenu.menu.AppendAction("Action 3", (DropdownMenuAction action) => { Debug.Log("Action 3 selected"); });

            // Add the dropdown menu to the toolbar
            toolbar.Add(dropdownMenu);

            // Add the toolbar to the main container
            Add(toolbar);
        }
    }
}
