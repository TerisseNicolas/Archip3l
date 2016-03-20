buildingsList = ['GoldMine', 'StoneMine', 'OilPlant', 'Sawmill', 'Factory', 'WindTurbine', 'Farm', 'Harbor', 'PowerPlant', 'Lab', 'Airport', 'Hotel', 'School','Church', 'Cinema', 'AmusementPark']
challengeTypesList = ['QCM', 'VraiFaux']
print('\n')
for challengeType in challengeTypesList:
	for building in buildingsList:
		fileName = challengeType + "_" + building + ".txt"
		challengeFile = open(challengeType + "/" + fileName, "r")
		if (fileName == "VraiFaux_StoneMine.txt"):
			for line in challengeFile.readlines():
				splitted = line.split(';')
				length = len(splitted)
				if (challengeType == "QCM" and length != 1 and length != 6):
					print("semicolon error in " + fileName)
				if (challengeType == "VraiFaux" and length != 1 and length != 5):
					print("semicolon error in " + fileName)
				if ('\"' in line or '«' in line or'‘' in line):
					print("quotation mark error in " + fileName)
print('\n')