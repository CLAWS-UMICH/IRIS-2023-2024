# Waypoint Python Server for webosckets. It send 3 waypoints at first and then deletes 1, then loops again and again.
# Steps:
# Run this file in VSCode and AFTER run your unity scene
# Update buttons UI to these waypoints using the scroll handler provided
# Use the event system to subscribe

import asyncio
import websockets # If this gives a warning/error, open command line and do 'pip install websockets'
import json

async def first_send(websocket):
    data = {
        "id" : "1",
        "type": "INITIAL",
    }

    # Convert the data to a JSON string
    message = json.dumps(data)

    # Send the JSON message to the connected client (Unity)
    await websocket.send(message)

async def send_data_new(websocket):
    data = {
        "id": 1,
        "type": "Waypoints",
        "use": "PUT",
        "data": {
            "currentIndex": 3,
            "AllWaypoints": [
                {
                    "waypoint_id": 0,
                    "waypoint_letter": "A",
                    "location": {
                        "latitude": 29.5647112,
                        "longitude": -95.081375
                    },
                    "type": 0,
                    "description": "Station 1",
                    "author": 123
                },
                {
                    "waypoint_id": 1,
                    "waypoint_letter": "B",
                    "location": {
                        "latitude": 29.5647118,
                        "longitude": -95.081275
                    },
                    "type": 1,
                    "description": "Navigation Point 1",
                    "author": 456
                },
                {
                    "waypoint_id": 2,
                    "waypoint_letter": "C",
                    "location": {
                        "latitude": 29.5647012,
                        "longitude": -95.081275
                    },
                    "type": 2,
                    "description": "Geological Point 1",
                    "author": 789
                },
                {
                    "waypoint_id": 3,
                    "waypoint_letter": "D",
                    "location": {
                        "latitude": 29.5647000,
                        "longitude": -95.081395
                    },
                    "type": 0,
                    "description": "Geological Point 1",
                    "author": 789
                },
            ]
        }
    }

    # Convert the data to a JSON string
    message = json.dumps(data)

    # Send the JSON message to the connected client (Unity)
    await websocket.send(message)





async def send_data_periodically(websocket):
    await send_data_new(websocket)
    await asyncio.sleep(5)

async def handle_client(websocket, path):
    try:
        await first_send(websocket)
        message = await websocket.recv()
        print(f"Received message from client: {message}")
        send_task = asyncio.create_task(send_data_periodically(websocket))

        while True:
            message = await websocket.recv()
            print(f"Received message from client: {message}")

    except websockets.exceptions.ConnectionClosed:
        send_task.cancel()

start_server = websockets.serve(handle_client, "localhost", 8080)

asyncio.get_event_loop().run_until_complete(start_server)
asyncio.get_event_loop().run_forever()
