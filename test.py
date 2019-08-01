import io
from PIL import Image
import requests

output = io.BytesIO()

data = Image.open("3.jpg")
data.save(output, "PNG")
data = output.getvalue()

response = requests.post("http://127.0.0.1:5000/", files={"post_data": data})

Image.open(io.BytesIO(response.content)).save("res.png")
