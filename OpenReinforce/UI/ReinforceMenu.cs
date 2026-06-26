// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Drawing;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Windows.Forms;
using OpenReinforce.Engine;
using OpenReinforce.Native.Interop;

namespace OpenReinforce.UI;

internal static class ReinforceMenu
{
    private static FrHandle? CurrentPursuit;

    private static int OpenCloseElapsed = 0;
    private const int OpenCloseDelay = 10;

    internal static readonly UIMenu Menu = new("Open Reinforce",
        "MAIN MENU", Point.Empty, "mpfclone_retro_loading",
        "fingerprint_hacking_minigame_loading_window_blank");

    internal static readonly MenuPool MenuPool = [Menu];

    private static readonly UIMenuListScrollerItem<ReinforceCategory> PriorityItem = new("Category",
        "Specifies the nature or the priority of the reinforcement response.");

    private static readonly UIMenuItem LocalPatrolItem = new("Local Patrol",
        "Send for a local patrol unit as reinforcement.");
    private static readonly UIMenuItem StatePatrolItem = new("State Patrol",
        "Send for a state patrol unit as reinforcement.");

    internal static void InitializeComponents()
    {
        LocalPatrolItem.Activated +=
            (_, _) => OnPatrolActivated(ReinforceType.LocalPatrol);
        StatePatrolItem.Activated +=
            (_, _) => OnPatrolActivated(ReinforceType.StatePatrol);

        Menu.AddItem(PriorityItem);
        Menu.AllowCameraMovement = true;
        Menu.MouseControlsEnabled = false;
        Menu.MouseEdgeEnabled = false;
        Menu.OnMenuOpen += Menu_Opening;

        PriorityItem.IndexChanged += PriorityItem_IndexChanged;
    }

    private static void OnPatrolActivated(ReinforceType reinforceType)
    {
        ResponseManager.CreateResponse(PriorityItem.SelectedItem,
            reinforceType);
        Menu.Visible = false;
    }

    private static void PriorityItem_IndexChanged(UIMenuScrollerItem sender, int oldIndex, int newIndex)
    {
        if (!Menu.Visible)
        {
            return;
        }

        var newCategory = (ReinforceCategory)newIndex;

        Menu.MenuItems.RemoveRange(1, Menu.MenuItems.Count - 1);
        if (newCategory != ReinforceCategory.Services)
        {
            Menu.MenuItems.Add(LocalPatrolItem);
            Menu.MenuItems.Add(StatePatrolItem);
        }

        Menu.RefreshIndex();
    }

    internal static void Process()
    {
        if (OpenCloseElapsed < OpenCloseDelay)
        {
            OpenCloseElapsed++;
        }

        if (Game.IsKeyDown(Keys.B) && OpenCloseElapsed >= OpenCloseDelay)
        {
            OpenCloseElapsed = 0;
            Menu.Visible = !Menu.Visible;
        }

        MenuPool.ProcessMenus();
    }

    private static void Menu_Opening(UIMenu sender)
    {
        UpdatePriorities();
    }

    internal static void UpdatePriorities()
    {
        PriorityItem.Items.Clear();

        bool isPursuitActive;
        if (!OpenReinforcePlugin.IsTestPlugin)
        {
            var activePursuit = FrFunctions.GetActivePursuit();
            isPursuitActive = !activePursuit.IsNull && FrFunctions.IsPursuitStillRunning(activePursuit);

            CurrentPursuit = isPursuitActive
                ? activePursuit
                : null;
        }
        else
        {
            isPursuitActive = EntryPoint.IsPursuitInProgress;
        }

        PriorityItem.Items.Add(ReinforceCategory.Emergency);
        PriorityItem.Items.Add(ReinforceCategory.Urgent);
        PriorityItem.Items.Add(ReinforceCategory.Services);

        if (isPursuitActive)
        {
            PriorityItem.Items.Add(ReinforceCategory.Pursuit);
            PriorityItem.SelectedItem = ReinforceCategory.Pursuit;
        }
        else
        {
            PriorityItem.SelectedItem = ReinforceCategory.Emergency;
        }
    }
}
