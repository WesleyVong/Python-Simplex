import numpy as np
from Simplex import simplex as sp
import sys

print(sys.argv)

if len(sys.argv) < 2:
    print("usage: python SimplexMain.py <data file> [max/min>]")
    sys.exit()

#Loads the Simplex array and calculate simplex
filename = sys.argv[1]
array = np.loadtxt(filename)
if len(sys.argv) < 3:
  sp.simplex(array)
else:
  maximize = sys.argv[2]
  sp.simplex(array, maximize)
    

