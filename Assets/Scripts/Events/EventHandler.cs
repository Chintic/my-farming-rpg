using System;
using System.Collections.Generic;

// forma do contrato do delegate
public delegate void MovementDelegate(float inputX, float inputY, bool isWalking, bool isRunning, bool isIdle, bool isCarrying,
    ToolEffect toolEffect, bool isUsingToolgRightr, bool isUsingToolLeft, bool IsUsingToolUp, bool isUsingToolDown,
    bool isLiftingToolRight, bool isLiftingToolLeft, bool isLiftingToolUp, bool isLiftingToolDown,
    bool isPickingUpRight, bool isPickingUpLeft, bool isPickingUpUp, bool isPickingUpDown,
    bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolUp, bool isSwingingToolDown,
    bool idleUp, bool idleDown, bool idleLeft, bool idleRight);

public static class EventHandler
{
    // Inventory Update Event
    public static event Action<InventoryLocation, List<InventoryItem>> InventoryUpdateEvent;

    public static void CallInventoryUpdateEvent(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList)
    {
        // essa interrogação é o mesmo que verificar so o inventoryupdateevent é null
        InventoryUpdateEvent?.Invoke(inventoryLocation, inventoryList);
    }

    // Movement Event

    public static event MovementDelegate MovementEvent;

    // Movement Event Call For Publishers

    public static void CallMovementEvent(float inputX, float inputY, bool isWalking, bool isRunning, bool isIdle, bool isCarrying,
    ToolEffect toolEffect, bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown,
    bool isLiftingToolRight, bool isLiftingToolLeft, bool isLiftingToolUp, bool isLiftingToolDown,
    bool isPickingUpRight, bool isPickingUpLeft, bool isPickingUpUp, bool isPickingUpDown,
    bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolUp, bool isSwingingToolDown,
    bool idleUp, bool idleDown, bool idleLeft, bool idleRight)
    {
        // essa interrogação é o mesmo que verificar so o MovementEvent é null
        MovementEvent?.Invoke(inputX, inputY,
            isWalking, isRunning, isIdle, isCarrying,
            toolEffect,
            isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
            isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
            isPickingUpRight, isPickingUpLeft, isPickingUpUp, isPickingUpDown,
            isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
            idleUp, idleDown, idleLeft, idleRight);
    }

    // Time Events

    // Advance game minute
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameMinuteEvent;

    public static void CallAdvanceGameMinuteEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        AdvanceGameMinuteEvent?.Invoke(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
    }

    // Advance game hour
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameHourEvent;

    public static void CallAdvanceGameHourEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        AdvanceGameMinuteEvent?.Invoke(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
    }

    // Advance game day
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameDayEvent;

    public static void CallAdvanceGameDayEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        AdvanceGameMinuteEvent?.Invoke(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
    }

    // Advance game season
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameSeasonEvent;

    public static void CallAdvanceGameSeasonEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        AdvanceGameMinuteEvent?.Invoke(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
    }

    // Advance game year
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameYearEvent;

    public static void CallAdvanceGameYearEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        AdvanceGameMinuteEvent?.Invoke(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
    }

    // Scene Load Events - in the order they happen

    // Before Scene Unload Fade Out Event
    public static event Action BeforeSceneUnloadFadeOutEvent;

    public static void CallBeforeSceneUnloadFadeOutEvent()
    {
        BeforeSceneUnloadFadeOutEvent?.Invoke();
    }

    // Before Scene Unload Event
    public static event Action BeforeSceneUnloadEvent;

    public static void CallBeforeSceneUnloadEvent()
    {
        BeforeSceneUnloadEvent?.Invoke();
    }

    // After Scene Loaded Event
    public static event Action AfterSceneLoadEvent;

    public static void CallAfterSceneLoadEvent()
    {
        AfterSceneLoadEvent?.Invoke();
    }

    // After Scene Load Fade In Event
    public static event Action AfterSceneLoadFadeInEvent;

    public static void CallAfterSceneLoadFadeInEvent()
    {
        AfterSceneLoadFadeInEvent?.Invoke();
    }
}