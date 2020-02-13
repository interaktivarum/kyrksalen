import cv2
import sys
import pytesseract
import os
 
def OCRSerial(argv):

	scriptDir = os.path.dirname(__file__)
	folderFrom = os.path.join(scriptDir,'Camera/');
	folderTo = os.path.join(scriptDir,'OCR/');
	filename = argv[1]
	imgPath = folderFrom+filename

	if not os.path.exists(imgPath):
		print('ERROR: Image file path does not exist: ' + imgPath)
		sys.exit(1)
   
	# Get serial number
	serial = ocr_serial_number(imgPath)

	# Copy and rename file
	split = filename.split('.')
	filenameNew = split[0]+'_'+serial+'.'+split[1]

	# Copy file to new folder
	if not os.path.exists(folderTo):
		os.makedirs(folderTo)
	os.rename(imgPath,folderTo+filenameNew)
 
	# Print serial number
	return(filenameNew)

def ocr_serial_number(imgPath):

	# Read image from disk
	img = cv2.imread(imgPath, cv2.IMREAD_COLOR)

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
	if len(sys.argv) < 2:
		print('USAGE \n arg1, file path: input image file name')
		sys.exit(1)

	filenameNew = OCRSerial(sys.argv)
	print(filenameNew)
	