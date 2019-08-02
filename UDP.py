import logging
from websocket_server import WebsocketServer
from PIL import Image
import io
import base64
from tran_goods import TransGoods


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
    logger.info('Message has been received from {}:{}'.format(client['address'][0], client['address'][1]))
    # reply_message = 'Re: ' + message
    print(len(message))
    print(message)
    file = base64.b64decode(message)

    image = Image.open(io.BytesIO(file))

    image = tg.segmentation(image)

    buf = io.BytesIO()
    image.save(buf, "PNG")

    send = base64.b64encode(buf.getvalue())
    print(send)

    server.send_message(client, send)
    # server.send_message(client, reply_message)
    logger.info('Message has been sent to {}:{}'.format(client['address'][0], client['address'][1]))


# main
if __name__ == "__main__":
    tg = TransGoods()

    server = WebsocketServer(port=PORT, host=HOST)
    server.set_fn_new_client(new_client)
    server.set_fn_client_left(client_left)
    server.set_fn_message_received(message_received)
    server.run_forever()
