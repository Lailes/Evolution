from matplotlib import pyplot as plt
import os.path
import sys

BasePath =  'C:\\Users\\Auriel\\Desktop\\EvolutionData'
InitialFile = BasePath + '\\initial.tsv'

file = open(InitialFile, 'r')

startX = []
startY = []

while True:
    line = file.readline()
    if not line:
        break
    Data = line.replace(',', '.').split('\n')[0].split('\t')
    startX.append(float(Data[1]))
    startY.append(float(Data[2]))
file.close()
plt.plot(startX, startY, color='blue')



if len(sys.argv) > 1:
    populationFile = BasePath + '\\Population-' + sys.argv[1] + '.tsv'
    populationFile = open(populationFile, 'r')
    populationY = []
    populationX = []
    while True:
        line = populationFile.readline()
        if not line:
            break
        Data = line.replace(',', '.').split('\n')[0].split('\t')
        if len(Data) != 2:
            continue
        populationX.append(float(Data[0]))
        populationY.append(float(Data[1]))
    plt.plot(populationX, populationY, 'o')
plt.grid()
plt.show()
