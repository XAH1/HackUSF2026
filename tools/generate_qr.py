import qrcode

nodes = ["entrance", "elevator", "stairs", "hackroom", "big_window"]

for node_id in nodes:
    img = qrcode.make(node_id)
    img.save(f"../data/qr_codes/{node_id}.png")
    print(f"✓ {node_id}.png")