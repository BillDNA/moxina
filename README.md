# moxina

Please update this section with important notes about your implementation.

# Finding things
1. All Code is located in Assets/Scripts
2. Any Images are located in Assets/Resources
3. Prefabs are located in Assets/Prefabs 
# Game Controls
## Camera Controls
1. If you touch you mouse to the edge of the screen the camera will pan in that direction.
    * To Change the speed and range, on the Main Camera set the Edge Pan Speed and Edge Percent
2. Using the zoom wheal will zoom in and out 
    * To Change the zoom speed, on the Main Camera set the Zoom Speed
3. Right Click and hold will enter pan mode, releasing the right mouse button will exit pan mode.
4. Clicking On the Zoom Button will Dynamically zoom to fit all gears int he window
    * To adjust the speed set DynamicZoomTime, this is how long in seconds it will take to reach the final position and zoom.
## Play Controls
1. Pause and play buttons with start and stop the gears from spinning.
2. Clicking the Switch button with switch the direction the root gear traves
    * a toggle is also at the bottom of the screen
3. Adjusting the speed slider with increase or decrease the speed at witch the root gear rotates.
    * Changing this with automatically start the gears rotating.
4. The Clear button will remove all gears but the root gear from the scene.
5. The Add Gear Button will add a random gear to the gears.  
6. The Shuffle Button Will clear all existing Gears and created 10 new ones at random.
# Creating New Gear Sizes
### Generating the Gear Image
1. Use the [Gear Template Website](https://woodgears.ca/gear_cutting/template.html) to create a gear with the desired number of teeth.  Using the following settings.
    * Tooth Spacing = 5 mm
    * Contact Angle = 20 mm
    * Shat Hole Diameter = 5 mm (if you want the center hole to be the same size)
2. After Downloading the pdf convert it to a png.
    * It Can be helpful to leave the 'Show Center' and 'Pitch Diameter' checked for alignment purposes.
    * Make sure that teh gear is centered in png.
3. Import the png into Unity.
### Creating the Gear Perfab
1. Create an empty game object with a Sprite Render and the Gear components 
2. Set the up the information
    * Tooth Count - The number of teeth on the gear.
    * Tooth offset - The angle needed such that at the 3 O'clock position a tooth is centered.
    * Overlap Radius - The distance from which non attached gears can not overlay.
    * Mesh Radius - The distance this gear's center needs to be from the edge of another gears mesh radius. If you left 'Pitch Diameter' on this is the blue circle.
3. Save this Game object as a Prefab
### Adding the Gear to the game
1. Open the Main Scene.
2. Select the Game Manager
3. Add the newly created prefab to the list of prefabs.


# Out Side Resources

 1. [Gear Templates](https://woodgears.ca/gear_cutting/template.html).  Used to generate Images of gears, any future gears will need a contact angle of 20 degrees in order to mesh properly with existing gears.
 2. [Clean Settings UI](https://assetstore.unity.com/packages/tools/gui/clean-settings-ui-65588).  Located in 'Assets/Free UI build package', used to make some nicer looking UI controls.