import cv2
import sys
import pytesseract
import os
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
   
	# Get serial number
	serial = ocr_serial_number(imgPath)
	serial = serial.replace(" ", "")

	if serial == "":
		os.remove(imgPath)
		# Print error
		print("Error: could not read serial number")
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

def ocr_serial_number(imgFilename):

	# Read image from disk
	img = cv2.imread(imgFilename, cv2.IMREAD_COLOR)

	img = cv2.rotate(img, cv2.ROTATE_90_COUNTERCLOCKWISE);

	# Crop image to contain serial number
	h, w = img.shape[:2]
	crop_img = img[h-150:h-0, w-750:w-250]
	# show_image(crop_img)

	# Uncomment the line below to provide path to tesseract manually
	# pytesseract.pytesseract.tesseract_cmd = '/usr/bin/tesseract'

	# Define config parameters.
	# '-l eng'  for using the English language
	# '--oem 1' for using LSTM OCR Engine
	config = ('-l eng --oem 1 --psm 3')

	# Run tesseract OCR on cropped image
	return pytesseract.image_to_string(crop_img, config=config)

def show_image(img):
	cv2.imshow("cropped", img)
	cv2.waitKey(0)

if __name__ == '__main__':
	#if len(sys.argv) == 1:
	#	scriptDir = os.path.dirname(os.path.abspath(__file__))
	#	imgDir = os.path.join(scriptDir,'Camera/');
	#	OCRSerial_folder(imgDir)
	#if len(sys.argv) == 2:
	#	OCRSerial_folder(sys.argv[1])
	#if len(sys.argv) == 3:
	#	OCRSerial_file(sys.argv[1],sys.argv[2])
	#if len(sys.argv) == 4:
	OCRSerial_file(sys.argv[1],sys.argv[2],sys.argv[3])
	