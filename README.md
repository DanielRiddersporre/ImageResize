# ImageResize
Small desktop application for resizing images (.jpg, .jpeg, .png) without breaking the aspect ratio. UI made in WFP with a class library handling all the logic
behind the resizing. Some unit testing is set up to set the structure of the project. As of right now the default size of any resized images is 512x512. If the
original image is smaller than this, the image keeps it's original width and height.

# Using the application
- Open the solution in Visual Studio
- Run the program without Debugging and you should see the following application appear
  ![image](https://github.com/DanielRiddersporre/ImageResize/assets/108796937/87e45503-f201-4d6c-a039-ba287d762c73)
- Push "Select Image" and a new dialog pops up where you can choose an image.
- When you have chosen your image you can see the path to that image appear in the textbox below the buttons
- Press "Resize Image" and the process will start.
- If everything goes well your resized image can be found in ../ImageResize/ImageResize/bin/Debug/net7.0-windows/resizedImages/ with the part "_thumb" added to the filename
- If process ends with any errors you can find more information in the log file; ../ImageResize/ImageResize/bin/Debug/net7.0-windows/resizedlog.txt

# Dependencies
Application uses a few different frameworks to work. When cloning/forking the repository they should be automatically added to your local environment but
in case of, here's a list with the frameworks used:
- SixLabors.ImageSharp (Handling the image size manipulation)
- Serilog (For logging, mainly in the Library-project)
- Moq (For mocking in the test project)

# Error handling
Some error handling was implemented but mainly in the form of try/catch blocks. A dialog with the most important information is shown in the UI
when needed but everything else is logged in the resizedlog.txt file mentioned earlier in this information.

# Known Bugs and improvement points
- If an image.height is of a higher value than image.width the resizing is inconsistent and this is considered a bug.
- Only the three most popular image types are supported at the moment (.jpg, .jpeg, .png)

