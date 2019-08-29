import logging
from websocket_server import WebsocketServer
from PIL import Image, ImageOps
import io
import numpy as np
import cv2

from trans_interior import TransInterior

PORT = 5000
HOST = '127.0.0.1'
# logger_setup
logger = logging.getLogger(__name__)
logger.setLevel(logging.INFO)
handler = logging.StreamHandler()
handler.setFormatter(logging.Formatter(' %(module)s -  %(asctime)s - %(levelname)s - %(message)s'))
logger.addHandler(handler)

# callback
def new_client(client, server):
    logger.info('New client {}:{} has joined.'.format(client['address'][0], client['address'][1]))


def client_left(client, server):
    logger.info('Client {}:{} has left.'.format(client['address'][0], client['address'][1]))


def message_received(client, server, message):
    global is_processing
    logger.info('Message has been received from {}:{}'.format(client['address'][0], client['address'][1]))
    if is_processing:
        print("NOW PROCESS")
        return

    is_processing = True
    # reply_message = 'Re: ' + message
    print(len(message))
    print(type(message))
    print(message)

    header = message[:6]
    contents = message[6:]

    image = Image.open(io.BytesIO(contents))
    # image.save("aaa.png")

    image = ImageOps.flip(image)

    # 処理
    image = translation.do_translation(image, [0])
    # image = translation.trans_interior(image, [0])

    image = ImageOps.flip(image)
    # image = translation.segmentation(image)

    buf = io.BytesIO()
    image.save(buf, "JPEG")
    message[6:] = buf.getvalue()
    print(message)

    server.send_message(client, message)
    is_processing = False
    # server.send_message(client, reply_message)
    logger.info('Message has been sent to {}:{}'.format(client['address'][0], client['address'][1]))


# main
if __name__ == "__main__":
    is_processing = False
    translation = TransInterior()

    server = WebsocketServer(port=PORT, host=HOST)
    server.set_fn_new_client(new_client)
    server.set_fn_client_left(client_left)
    server.set_fn_message_received(message_received)
    server.run_forever()
