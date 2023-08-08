#!/usr/bin/env python3
import time
import socket
import argparse
import numpy as np

import rospy
import rosbag
from geometry_msgs.msg import TransformStamped

class TCPClient():

  # mira: method automatically called to initialize the state of the object being created
  def __init__(self, ip="127.0.0.1", port=25000):

    self.position_vicon = [0, 0, 0]
    self.rotation_vicon = [0, 0, 0, 0]

    # Holds the drone position and rotation
    self.position = np.array([0, 0, 0], dtype=float)
    self.rotation = np.array([0, 0, 0, 0], dtype=float)

    # Create the connection
    self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    self.sock.connect((ip, port))

    # mira: delete ?
    # Each time we send a message we expect the word "Accepted" to be returned
    self.expected_msg_size = 8

    # mira: ASK ABOUT THIS ?
    # Set the rate the data is sent at
    # self.rate = 10
    # self.freq = 10.0/self.rate

    # Run the node
    self.Run()

  def callback(self, message):
    global position_vicon, rotation_vicon

    # We want to update at a set frequency
    start_time = time.time() 
      
    # Convert both position and rotation a string and append them
    msg1 = ','.join(np.round(self.position,8).astype(str))
    msg2 = ','.join(np.round(self.rotation,8).astype(str))
    msg = msg1 + "," + msg2

      

    # Send the message and wait for a new one
    if self.sock is not None:

      self.position_vicon = [message.transform.translation.x, message.transform.translation.y, message.transform.translation.z]
      self.rotation_vicon = [message.transform.rotation.x, message.transform.rotation.y, message.transform.rotation.z, message.transform.rotation.w]
      rospy.loginfo('I hear you! %s', message)
      self.position = self.position_vicon
      self.rotation = self.rotation_vicon

      self.sock.sendall(msg.encode("UTF-8"))

      print(f"Position: {self.position}")
      print(f"Rotation: {self.rotation}")

      print(f"=========================")

    # Sleep for the remaining duration of self.freq
    # time.sleep(max(self.freq - (time.time() - start_time), 0))
    # time.sleep(0.001)


    
    
    # rospy.loginfo("MIRA: %s", self.position[0])
    # rospy.loginfo("KHAN: %s", self.position_vicon[0])

  # This is the main loop of this class
  def Run(self):
    global position_vicon, rotation_vicon

    # mira: this is the place where i would have to update the position according to ROS
    # In ROS this would be done in a callback function
    # self.position[0] += 0.1
    rospy.init_node('listener', anonymous=True)
    
    bag_topic = '/vicon/FDE2D0/FDE2D0'

    rospy.Subscriber(bag_topic, TransformStamped, self.callback)

    rospy.spin()
    
      
# cd "Safe Trajectory Simulation Research"/Assets/ML/Scripts
# python3 

# to play bag:
# cd catkin_ws/src/beginner_tutorials
# rosbag play vicon.bag -l

# mira: ensures that the code block only runs when it is called directly
if __name__ == '__main__':
  
  # mira: argparse module allows me to create command-line interfaces (still confused on how these next few lines work)
  # Create the parser
  parser = argparse.ArgumentParser()

  # Add the arguments
  parser.add_argument("--ip", type=str, default="127.0.0.1", help="The IP address of the TCP server")
  parser.add_argument("--port", type=int, default=25000, help="The port of the TCP server")

  # Parse the arguments
  args = parser.parse_args()

  # mira: calls the class to be run with set arguments (is this only done once ?)
  # Run the code
  tcp_obj = TCPClient(ip=args.ip, port=args.port)






