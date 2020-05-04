import cv2
import sys
import pytesseract
import os
import re
from shutil import move

def OCRSerial_folder(imgDir):

	for imgFilename in os.listdir(imgDir):
		OCRSerial_file(imgDir, imgFilename)
 
def OCRSerial_file(imgDir,folderTo,imgFilename):

	# scriptDir = os.path.dirname(__file__)
	#folderTo = os.path.join(os.path.dirname(os.path.abspath(__file__)),'OCR/');
	imgPath = imgDir+imgFilename

	if not os.path.exists(imgPath):
		print('ERROR: Image file path does not exist: ' + imgPath)
		sys.exit(1)
   
	try:
		# Get serial number
		serial = ocr_serial_number(imgPath)
		serial = serial.replace(" ", "")

		if serial == "":
			os.remove(imgPath)
			# Print error
			print("Error: could not read serial number")
			sys.exit(1)
		else:
			# Copy and rename file
			split = imgFilename.split('.')
			filenameNew = split[0]+'_'+serial+'.'+split[1]
	
			# Copy file to new folder
			if not os.path.exists(folderTo):
				os.makedirs(folderTo)
			# os.rename(imgPath,folderTo+filenameNew)
			move(imgPath, folderTo+filenameNew)
	
			# Print serial number
			print(filenameNew)
	except:
		os.remove(imgPath)
		sys.exit(1)

def ocr_serial_number(imgFilename):

	# Read image from disk
	img = cv2.imread(imgFilename, cv2.IMREAD_COLOR)

	# Crop image to contain serial number
	h, w = img.shape[:2]
	crop_img = img[int(h/4):int(h/4+h/2), int(9*w/10):w]

	rot_img = cv2.rotate(crop_img, cv2.ROTATE_90_CLOCKWISE);

	# show_image(rot_img)

	# Uncomment the line below to provide path to tesseract manually
	# pytesseract.pytesseract.tesseract_cmd = '/usr/bin/tesseract'

	# Define config parameters.
	# '-l eng'  for using the English language
	# '--oem 1' for using LSTM OCR Engine
	config = ('-l eng --oem 1 --psm 3 -c tessedit_char_whitelist=#-0123456789')

	# Run tesseract OCR on cropped image
	ocr = pytesseract.image_to_string(rot_img, config=config)
	serial = "#" + re.search("#(.*?)#", ocr).group(1) + "#"
	return serial

def show_image(img):
	cv2.imshow("cropped", img)
	cv2.waitKey(0)

if __name__ == '__main__':
	if len(sys.argv) == 2:
		scriptDir = os.path.dirname(os.path.abspath(__file__))
		fromDir = os.path.join(scriptDir,'Camera/');
		toDir = os.path.join(scriptDir,'OCR/');
		OCRSerial_file(fromDir,toDir,sys.argv[1])
		# OCRSerial_folder(imgDir)
	#if len(sys.argv) == 2:
	#	OCRSerial_folder(sys.argv[1])
	#if len(sys.argv) == 3:
	#	OCRSerial_file(sys.argv[1],sys.argv[2])
	if len(sys.argv) == 4:
		OCRSerial_file(sys.argv[1],sys.argv[2],sys.argv[3])
	sys.exit(0)
	