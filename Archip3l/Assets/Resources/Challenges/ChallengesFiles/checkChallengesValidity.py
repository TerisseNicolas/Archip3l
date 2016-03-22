buildingsList = ['GoldMine', 'StoneMine', 'OilPlant', 'Sawmill', 'Factory', 'WindTurbine', 'Farm', 'Harbor', 'PowerPlant', 'Lab', 'Airport', 'Hotel', 'School','Church', 'Cinema', 'AmusementPark']
challengeTypesList = ['QCM', 'VraiFaux']

#errors detection
print('\n')
for challengeType in challengeTypesList:
	for building in buildingsList:
		fileName = challengeType + "_" + building + ".txt"
		challengeFile = open(challengeType + "/" + fileName, "r")
		try:
			for line in challengeFile.readlines():
				splitted = line.split(';')
				length = len(splitted)
				if (challengeType == "QCM" and length != 1 and length != 6):
					print("semicolon error in " + fileName)
				if (challengeType == "VraiFaux" and length != 1 and length != 5):
					print("semicolon error in " + fileName)
				if ('\"' in line or '«' in line or'‘' in line):
					print("quotation mark error in " + fileName)
				if ('_' in line):
					print("underscore present in " + fileName)
			challengeFile.close
		except:
			print("error in " + fileName)

print('\n')

#number of lines printing

print('\n')
for challengeType in challengeTypesList:
	for building in buildingsList:
		fileName = challengeType + "_" + building + ".txt"
		challengeFile = open(challengeType + "/" + fileName, "r")
		try:
			print(fileName + ": " + str(len(challengeFile.readlines()) - 1) + " questions")	#-1: first line	
		except:
			print("error in " + fileName)
		challengeFile.close
print('\n')