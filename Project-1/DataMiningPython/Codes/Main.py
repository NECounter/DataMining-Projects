import tensorflow as tf
import pandas as pd

import tensorflow as tf
device_name = tf.test.gpu_device_name()
if device_name != '/device:CPU:0':
  raise SystemError('GPU device not found')
print('Found GPU at: {}'.format(device_name))