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
        "id" : 1,
        "type": "INITIAL",
    }

    # Convert the data to a JSON string
    message = json.dumps(data)

    # Send the JSON message to the connected client (Unity)
    await websocket.send(message)

async def send_data_1(websocket):
    data = {


    # Convert the data to a JSON string
    message = json.dumps(data)

    # Send the JSON message to the connected client (Unity)
    await websocket.send(message)


async def send_data_periodically(websocket):
    await send_data_1(websocket)

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
