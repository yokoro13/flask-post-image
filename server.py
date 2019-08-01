from flask import Flask, request, make_response
from PIL import Image
import io
from tran_goods import TransGoods
import flask

app = Flask(__name__)


@app.route("/", methods=["GET"])
def do_get():
    return "Hey!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"


@app.route("/", methods=["POST"])
def do_post():
    if request.method == "POST":
        image_bytes = flask.request.data
        image = Image.open(io.BytesIO(image_bytes))

        # 処理
        image = tg.segmentation(image)

        buf = io.BytesIO()
        image.save(buf, "PNG")

        response = make_response(buf.getvalue())
        response.headers["Content-type"] = "Image"
        return response


if __name__ == '__main__':
    tg = TransGoods()
    app.debug = True
    app.run()