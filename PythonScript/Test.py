import cv2
from cvzone.PoseModule import PoseDetector
import socket

# cap = cv2.VideoCapture('3.mp4')
cap = cv2.VideoCapture(0)

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
serverAddressPort = ("127.0.0.1", 5054)      # 定义localhost与端口，当然可以定义其他的host

detector = PoseDetector()
posList = []    # 保存到txt在unity中读取需要数组列表

while True:
    success, img = cap.read()
    img = detector.findPose(img)
    lmList, bboxInfo = detector.findPosition(img)

    if bboxInfo:
        lmString = ''
        for lm in lmList:
            lmString += f'{lm[1]},{img.shape[0] - lm[2]},{lm[3]},'
        posList.append(lmString)

    # print(len(posList))       
    print(lmString)
    date = lmString
    sock.sendto(str.encode(str(date)), serverAddressPort)


    cv2.namedWindow("Image", 0)
    cv2.resizeWindow("Image", 659, 441)  # 设置窗口大小
    cv2.imshow("Image", img)
    key = cv2.waitKey(1)
    # 记录数据到本地
    # if key == ord('r'):    
    with open("MotionData.txt", 'w') as f:
        f.writelines(["%s\n" % item for item in posList])
