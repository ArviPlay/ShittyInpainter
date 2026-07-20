# ShittyInpainter
![Platform](https://img.shields.io/badge/platform-Windows-blue)
![.NET](https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff)
![License](https://img.shields.io/badge/license-MIT-red)
## description
a simple experimental image inpainting tool with a custom random algorithm built with **Windows Forms**

## how it works
- load an image
- select rect or lasso area on the image
- adjust the randomness strength using the slider
- click Inpaint to fill the selected area with a randomized pattern based on the image edges
- save the result as .png or .jpeg

## controls
- Load - choose an image file
- Inpaint - apply the algorithm to the selected area
- Save - save the modified image
- Slider - controls how much randomness is added to the filling
- Rectangle - rectangle selection mode
- Lasso (beta) - lasso selection mode

## hotkeys
- Ctrl+Z - undo last change
- Ctrl+O - load an image
- Ctrl+S - save the image

## algorithm
the program fills the selected area by taking colors from the edges of the selection and spreading them inward with random variations