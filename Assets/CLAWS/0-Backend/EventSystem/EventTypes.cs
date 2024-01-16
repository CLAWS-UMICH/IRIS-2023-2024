using System;
using System.Collections.Generic;
using UnityEngine;

public class ScreenChangedEvent
{
    public Screens Screen;

    public ScreenChangedEvent(Screens screen)
    {
        Screen = screen;
    }
}

public class CloseEvent
{
    public Screens Screen;

    public CloseEvent(Screens screen)
    {
        Screen = screen;
    }
}

// Event for letting us know GPS data was received from the server
public class UpdatedGPSEvent
{
    public UpdatedGPSEvent()
    {
        Debug.Log("GPS update event created");
    }

    public override string ToString()
    {
        return "<UpdatedGPSEvent>: new GPS msg";
    }
}

public class UpdatedGPSOriginEvent
{
    public UpdatedGPSOriginEvent()
    {
        Debug.Log("GPS origin updated");
    }

    public override string ToString()
    {
        return "<UpdatedGPSOriginEvent>: new GPS origin";
    }
}

public enum Direction { up, down }

public class ScrollEvent
{
    public Screens screen;
    public Direction direction;

    public ScrollEvent(Screens _screen, Direction _dir)
    {
        screen = _screen;
        direction = _dir;
        Debug.Log("Scrolling " + _screen.ToString() + " " + _dir.ToString());
    }

    public override string ToString()
    {
        return "<ScrollEvent>: " + screen.ToString() + " " + direction.ToString();
    }
}

public class VitalsUpdatedEvent
{
    public Vitals vitals { get; private set; }

    public VitalsUpdatedEvent(Vitals v)
    {
        vitals = v;
    }
    public override string ToString()
    {
        return "<VitalsUpdatedEvent>: vitals were updated";
    }
}

// Websockets
// --------------------------------------------------------------------------------

// Messaging
public class MessagesDeletedEvent
{
    public List<Message> DeletedMessages { get; private set; }

    public MessagesDeletedEvent(List<Message> deletedMessages)
    {
        DeletedMessages = deletedMessages;
    }
}

public class MessagesEditedEvent
{
    public List<Message> EditedMessages { get; private set; }

    public MessagesEditedEvent(List<Message> editedMessages)
    {
        EditedMessages = editedMessages;
    }
}

public class MessagesAddedEvent
{
    public List<Message> NewAddedMessages { get; private set; }

    public MessagesAddedEvent(List<Message> newAddedMessages)
    {
        NewAddedMessages = newAddedMessages;
    }
}

// Geosamples
public class GeosamplesDeletedEvent
{
    public List<Geosample> DeletedGeosamples { get; private set; }

    public GeosamplesDeletedEvent(List<Geosample> deletedGeosamples)
    {
        DeletedGeosamples = deletedGeosamples;
    }
}

public class GeosamplesEditedEvent
{
    public List<Geosample> EditedGeosamples { get; private set; }

    public GeosamplesEditedEvent(List<Geosample> editedGeosamples)
    {
        EditedGeosamples = editedGeosamples;
    }
}

public class GeosamplesAddedEvent
{
    public List<Geosample> NewAddedGeosamples { get; private set; }

    public GeosamplesAddedEvent(List<Geosample> newAddedGeosamples)
    {
        NewAddedGeosamples = newAddedGeosamples;
    }
}

// Waypoints
public class WaypointsDeletedEvent
{
    public List<Waypoint> DeletedWaypoints { get; private set; }

    public WaypointsDeletedEvent(List<Waypoint> deletedWaypoints)
    {
        DeletedWaypoints = deletedWaypoints;
    }
}

public class WaypointsEditedEvent
{
    public List<Waypoint> EditedWaypoints { get; private set; }

    public WaypointsEditedEvent(List<Waypoint> editedWaypoints)
    {
        EditedWaypoints = editedWaypoints;
    }
}

public class WaypointsAddedEvent
{
    public List<Waypoint> NewAddedWaypoints { get; private set; }

    public WaypointsAddedEvent(List<Waypoint> newAddedWaypoints)
    {
        NewAddedWaypoints = newAddedWaypoints;
    }
}

public class WaypointToDelete
{
    public int waypointIndexToDelete { get; private set; }

    public WaypointToDelete(int index)
    {
        waypointIndexToDelete = index;
    }
}

public class WaypointToAdd
{
    public Location location { get; private set; }
    public int type { get; private set; }
    public string description { get; private set; }

    public WaypointToAdd(Location loc, int t, string desc)
    {
        location = loc;
        type = t;
        description = desc;
    }
}

// Tasklist
public class TasksEditedEvent
{
    public List<TaskObj> EditedTasks { get; private set; }

    public TasksEditedEvent(List<TaskObj> editedTasks)
    {
        EditedTasks = editedTasks;
    }
}

public class TasksAddedEvent
{
    public List<TaskObj> NewAddedTasks { get; private set; }

    public TasksAddedEvent(List<TaskObj> newAddedTasks)
    {
        NewAddedTasks = newAddedTasks;
    }
}

public class TasksDeletedEvent
{
    public List<TaskObj> DeletedTasks { get; private set; }

    public TasksDeletedEvent(List<TaskObj> deletedTasks)
    {
        DeletedTasks = deletedTasks;
    }
}

public class TaskStartedEvent
{
    public TaskObj StartedTask { get; private set; }
    public TaskStartedEvent(TaskObj started_task)
    {
        Debug.Log("Task Started");
        StartedTask = started_task;
    }

    public override string ToString()
    {
        return "Task" + StartedTask.title + "Started";
    }
}

public class TaskFinishedEvent
{
    public TaskObj FinishedTask { get; private set; }
    public TaskFinishedEvent(TaskObj finished_task)
    {
        Debug.Log("Task Finished");
        FinishedTask = finished_task;
    }

    public override string ToString()
    {
        return "Task" + FinishedTask.title + "Finished";
    }
}

public class SubtaskStartedEvent
{
    public SubtaskObj StartedTask { get; private set; }
    public SubtaskStartedEvent(SubtaskObj started_task)
    {
        Debug.Log("Task Started");
        StartedTask = started_task;
    }

    public override string ToString()
    {
        return "Subtask" + StartedTask.title + "Started";
    }
}

public class SubtaskFinishedEvent
{
    public SubtaskObj FinishedTask { get; private set; }
    public SubtaskFinishedEvent(SubtaskObj finished_task)
    {
        Debug.Log("Task Finished");
        FinishedTask = finished_task;
    }

    public override string ToString()
    {
        return "Subtask" + FinishedTask.title + "Finished";
    }
}


// Mulitplayer
public class FellowAstronautLocationDataChangeEvent
{
    public FellowAstronaut AstronautToChange { get; private set; }

    public FellowAstronautLocationDataChangeEvent(FellowAstronaut astronautToChange)
    {
        AstronautToChange = astronautToChange;
    }
}

public class FellowAstronautVitalsDataChangeEvent
{
    public FellowAstronaut AstronautToChange { get; private set; }

    public FellowAstronautVitalsDataChangeEvent(FellowAstronaut astronautToChange)
    {
        AstronautToChange = astronautToChange;
    }
}

public class FellowAstronautBreadcrumbsDataChangeEvent
{
    public FellowAstronaut AstronautToChange { get; private set; }

    public FellowAstronautBreadcrumbsDataChangeEvent(FellowAstronaut astronautToChange)
    {
        AstronautToChange = astronautToChange;
    }
}

public class FellowAstronautNavigatingDataChangeEvent
{
    public FellowAstronaut AstronautToChange { get; private set; }

    public FellowAstronautNavigatingDataChangeEvent(FellowAstronaut astronautToChange)
    {
        AstronautToChange = astronautToChange;
    }
}

public class PopupEvent
{
    public string popupType;
    public string popupText;

    public PopupEvent(string in_popupType, string in_popupText)
    {
        popupType = in_popupType;
        popupText = in_popupText;
    }
}

// Pathfinding
public class StartPathfinding
{
    public Location location { get; private set; }

    public StartPathfinding(Location l)
    {
        location = l;
    }
}

public class SelectButton
{
    public string letter { get; private set; }

    public SelectButton(string l)
    {
        letter = l;
    }
}

public class BreadCrumbCollisionEvent
{
    public int index { get; private set; }

    public BreadCrumbCollisionEvent(int i)
    {
        index = i;
    }
}

// Pictures from Web
public class NewPicEvent
{
    public string image { get; private set; }

    public NewPicEvent(string pic)
    {
        image = pic;
    }
}

// Highlight Button
public class HighlightButton
{
    public GameObject button { get; private set; }

    public HighlightButton(GameObject _button)
    {
        button = _button;
    }
}

public class UnHighlight
{
    public List<string> levelnames { get; private set; }

    public UnHighlight(List<string> _levelnames)
    {
        levelnames = _levelnames;
    }
}