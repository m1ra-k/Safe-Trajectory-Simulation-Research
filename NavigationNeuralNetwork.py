import cv2 as cv
import numpy as np
import matplotlib.pyplot as plt
from tensorflow.keras import datasets, layers, models
import glob
from PIL import Image
import numpy as np
import os
import csv

# first do training dataset (how well model works on original data)
# later do testing dataset (how well model fits to new data)

# np array of ss

screenshot_images0 = []

for file in os.listdir('/home/ama9tk/Safe Trajectory Simulation Research/Assets/ML/Screenshots/'):
    if file.endswith('.png'):
        screenshot_image = np.array(Image.open(os.path.join('/home/ama9tk/Safe Trajectory Simulation Research/Assets/ML/Screenshots/', file)))
        screenshot_images0.append(screenshot_image)

screenshot_images = np.array(screenshot_images0)

# np array of commands (index only)

screenshot_labels0 = np.full((screenshot_images.size, 1), 0, dtype=int)
i = 0
index = np.array([0])

# change to training_data.csv
with open('/home/ama9tk/Safe Trajectory Simulation Research/Assets/ML/Input Data/test0.csv', 'r') as csv_file:
    csv_reader = csv.reader(csv_file)

    for row in csv_reader:  
        csv_arr = row[0].split(" : ")
        if csv_arr[3] == "Backward":
            index = np.array([0])
        elif csv_arr[3] == "Forward":
            index = np.array([1])
        elif csv_arr[3] == "Right":
            index = np.array([2])
        elif csv_arr[3] == "Left":
            index = np.array([3])
        elif csv_arr[3] == "N/A":
            index = np.array([4])
        else:
            break
        screenshot_labels0[i] = index
        i += 1

    screenshot_labels = np.full((i, 1), 0, dtype=int)

    for j in range(0, i):
        screenshot_labels[j] = screenshot_labels0[j]
    
# makes values as decimals

screenshot_images = screenshot_images / 255

class_names = ["Backward", "Forward", "Right", "Left", "N/A"]

i = 0
for i in range(4):
    plt.subplot(2, 2, i+1)
    plt.xticks([])
    plt.yticks([])
    plt.imshow(screenshot_images[i], cmap = plt.cm.binary)
    plt.xlabel(class_names[screenshot_labels[i][0]])

plt.show()

# trained and saved model 
model = models.Sequential()
model.add(layers.Conv2D(32, (3, 3), activation = "relu", input_shape = (501, 1028, 3)))
model.add(layers.MaxPooling2D(2, 2))
model.add(layers.Conv2D(64, (3, 3), activation = "relu"))
model.add(layers.MaxPooling2D(2, 2))
model.add(layers.Conv2D(64, (3, 3), activation = "relu"))
model.add(layers.Flatten())
model.add(layers.Dense(64, activation = "relu"))
model.add(layers.Dense(10, activation = "softmax"))

model.compile(optimizer = "adam", loss = "sparse_categorical_crossentropy", metrics = ["accuracy"])

print("1" + str(screenshot_images) + " size: " + str(screenshot_images.size))
print("2" + str(screenshot_images[0]))
print("3" + str(screenshot_labels) + " size: " + str(screenshot_labels.size))
print("4" + str(screenshot_labels[0]))

model.fit(screenshot_images, screenshot_labels, epochs = 10, validation_data = (screenshot_images, screenshot_labels))

model.save("test0.model")