from matplotlib import pyplot as plt

FileTour = 'C:\\Users\\Auriel\\Desktop\\tour.txt'
FileCoordinates = 'C:\\Users\\Auriel\\Desktop\\coordinates.txt'
Coordinates = dict()
Path = []
f = open(FileCoordinates, 'r')
dataCoordinates = [line.strip().split(' ') for line in f]
for arr in dataCoordinates:
    Coordinates[float(arr[0])] = [float(arr[1]), float(arr[2])]
f.close()
p = open(FileTour, 'r')
Vertexes = p.readline().split('-')
for fr in range(0, len(Vertexes) - 1):
    Path.append([float(Vertexes[fr]), float(Vertexes[fr + 1])])
Path.append([float(Vertexes[-1]), 1.0])
p.close()
X = []
Y = []
for path in Path:
    X.append(Coordinates[path[0]][0])
    Y.append(Coordinates[path[0]][1])
X.append(Coordinates[Path[-1][1]][0])
Y.append(Coordinates[Path[-1][1]][1])
plt.plot(X, Y, 'b')
plt.plot([X[0]], [Y[0]], 'ro')
plt.plot([X[0], X[1]], [Y[0], Y[1]], 'r')
plt.show()
