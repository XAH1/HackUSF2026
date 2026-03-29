import json, heapq

def find_path(start_id, end_id, map_data):
    graph = {}
    for edge in map_data["edges"]:
        a, b, d = edge["from"], edge["to"], edge["dist"]
        graph.setdefault(a, []).append((b, d))
        graph.setdefault(b, []).append((a, d))

    queue = [(0, start_id, [start_id])]
    visited = set()
    while queue:
        cost, node, path = heapq.heappop(queue)
        if node in visited:
            continue
        visited.add(node)
        if node == end_id:
            return path
        for neighbor, dist in graph.get(node, []):
            if neighbor not in visited:
                heapq.heappush(queue, (cost+dist, neighbor, path+[neighbor]))
    return []

with open("../data/building_map.json") as f:
    data = json.load(f)

# От входа до Hack Room
print(find_path("entrance", "hackroom", data))

# От лестницы до Hack Room  
print(find_path("stairs", "hackroom", data))

# От входа до Room 214
print(find_path("entrance", "room_214", data))