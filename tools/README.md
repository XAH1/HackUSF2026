# HackUSF — Indoor Navigation

A lightweight indoor navigation system for hackathon venues. Scan a QR code at any checkpoint to instantly get directions to your destination.

## How It Works

1. QR codes are placed at key locations throughout the building
2. Scanning a code identifies your current position
3. The pathfinder computes the shortest route to your destination
4. Step-by-step directions are returned

## Project Structure

```
HackUSF/
├── data/
│   ├── building_map.json   # Graph of nodes and edges (locations + distances)
│   └── qr_codes/           # Generated QR code images
└── tools/
    ├── generate_qr.py       # Generates QR codes for each node
    └── pathfinder.py        # Dijkstra-based shortest path finder
```

## Map Nodes

| ID           | Label      |
| ------------ | ---------- |
| `entrance`   | Entrance   |
| `elevator`   | Elevator   |
| `stairs`     | Stairs     |
| `room_214`   | Room 214   |
| `hackroom`   | Hack Room  |
| `big_window` | Big Window |
| `trash`      | Trash      |

## Setup

```bash
pip install qrcode[pil]
```

### Generate QR Codes

```bash
cd tools
python generate_qr.py
```

QR images will be saved to `data/qr_codes/`.

### Find a Path

Edit `pathfinder.py` or import `find_path` directly:

```python
from pathfinder import find_path
import json

with open("../data/building_map.json") as f:
    data = json.load(f)

print(find_path("entrance", "hackroom", data))
# → ['entrance', 'big_window', 'hackroom']
```

## Adding Locations

1. Add a node to `building_map.json`:

```json
{ "id": "my_room", "label": "My Room", "x": 10, "y": 20 }
```

2. Add edges connecting it to existing nodes
3. Add `"my_room"` to the list in `generate_qr.py` and re-run it
