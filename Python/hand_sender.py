import cv2
import mediapipe as mp
import socket
import json

# setup UDP
udp_ip = "127.0.0.1"
udp_port = 5052
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

mp_hands = mp.solutions.hands
mp_draw = mp.solutions.drawing_utils

cap = cv2.VideoCapture(0)

with mp_hands.Hands(max_num_hands=1, min_detection_confidence=0.7) as hands:
    while True:
        ret, frame = cap.read()
        if not ret:
            break
        frame = cv2.flip(frame, 1)
        rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        result = hands.process(rgb)

        if result.multi_hand_landmarks:
            for hand_landmarks in result.multi_hand_landmarks:
                mp_draw.draw_landmarks(frame, hand_landmarks, mp_hands.HAND_CONNECTIONS)

                # ambil posisi telapak tangan (landmark 0)
                palm = hand_landmarks.landmark[0]
                x, y = int(palm.x * 640), int(palm.y * 480)

                data = {"x": palm.x, "y": palm.y}
                sock.sendto(json.dumps(data).encode(), (udp_ip, udp_port))

        cv2.imshow("Hand Sender", frame)
        if cv2.waitKey(1) & 0xFF == 27:
            break

cap.release()
cv2.destroyAllWindows()
