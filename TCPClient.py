#!/usr/bin/env python3
import time
import socket
import numpy as np
import rospy
from geometry_msgs.msg import TransformStamped

class TCPClient():

  # mira: method automatically called to initialize the state of the object being created
  def __init__(self, ip="127.0.0.1", port=25000):
    rospy.on_shutdown(self.shutdown_sequence)

    ip = rospy.get_param('~ip_address', "127.0.0.1")
    port = rospy.get_param('~port', 25000)
    topic = rospy.get_param('~topic', "")

    if len(topic) <= 0:
      print("Topic must be set")
      exit()

    # Holds the drone position and rotation
    self.position = np.array([0, 0, 0], dtype=float)
    self.rotation = np.array([0, 0, 0, 0], dtype=float)

    print("Topic: {topic}")
    self.sub = rospy.Subscriber(f"{topic}", TransformStamped, self.callback)

    # Create the connection
    self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    self.sock.connect((ip, port))

    # mira: delete ?
    # Each time we send a message we expect the word "Accepted" to be returned
    self.expected_msg_size = 8

    # Run the node
    self.Run()

  def callback(self, message):
    self.position = [message.transform.translation.x, message.transform.translation.z, message.transform.translation.y]
    self.rotation = [message.transform.rotation.x, message.transform.rotation.y, message.transform.rotation.z, message.transform.rotation.w]

  # This is the main loop of this class
  def Run(self):
    # We want to update at a set frequency
    start_time = time.time() 
      
    # Convert both position and rotation a string and append them
    msg1 = ','.join(np.round(self.position,8).astype(str))
    msg2 = ','.join(np.round(self.rotation,8).astype(str))
    msg = msg1 + "," + msg2

    # Send the message and wait for a new one
    if self.sock is not None:
      self.sock.sendall(msg.encode("UTF-8"))

      byte_array = bytes()

      print(f"Position: {self.position}")
      print(f"Rotation: {self.rotation}")

      while len(byte_array) < self.expected_msg_size:
        data = self.sock.recv(self.expected_msg_size * 2)
        byte_array += data

      print(f"=========================")

    time.sleep(0.1)

  def shutdown_sequence(self):
    rospy.loginfo(str(rospy.get_name()) + ": Shutting Down")
    self.sock.close()
    
      
# cd "Safe Trajectory Simulation Research"/Assets/ML/Scripts
# python3 

# to play bag:
# cd catkin_ws/src/beginner_tutorials
# rosbag play vicon.bag -l

def main():
  rospy.init_node("TCPClientNode")
  try:
    tcp_obj = TCPClient()
  except rospy.ROSInterruptException:
    pass

# mira: ensures that the code block only runs when it is called directly
if __name__ == '__main__':
  # mira: calls the class to be run with set arguments (is this only done once ?)
  # Run the code
  tcp_obj = TCPClient()






