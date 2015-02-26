---README FOR DYNAMIC LIGHTING AND TIME SYSTEM----------------------------------------


FATHERTIME:

	To set up the timesystem please drag the empty FatherTime prefab
	from Project/LightSystem anywhere into your scene editor.
	
	The FatherTime prefab is responsible for controling what time you
	want the scene to start at (12:00am - 11:59:59pm) and is also
	responsible for controlling how fast the gametime moves in relation
	to real time (multiplier of 1 means that the gametime is moving in
	real time).

SUN:
	
	To enable the sun system in your scene drag the sun into your scene
	editor such that it is positioned where you want midnight in your
	game to occur.

	For example, in the included ExampleScene, the sun is positioned
	directly beneath the terrain so that the sun is on the "opposite
	side" of the terrain at midnight in the game.

	Then, attach the terrain (or object you want the sun to rotate around)
	to the target in the inspector(with the sun selected).

	It is important to note that even though you can change FatherTime's
	GameTime variable dynamically but the sun's position is based on the
	fact that it's original starting position in the scene was at midnight
	(0 seconds) GameTime.

	The sun's initial position also determines the radius at which it will
	orbit the terrain(or other target).

ENVIRONMENTLIGHT:

	To use the EnvironmentLight simply drag the EnvironmentLight prefab
	into your scene editor. Then, in the inspector (with the environment
	light selected), change the size of the List Of Light Changes to whatever
	number of times you want the environment light to change itself during the
	day.

	For each element in the List Of Light Changes you can tell the
	EnvironmentLight:

		-At what GameTime you want the change to occur

		-The range of the EnvironmentLight

		-The Intensity of the EnvironmentLight

		-And the Color you want the EnvironmentLight to be

	There is an ExampleBuilding and an ExampleLamp that do a good job showing
	how an EnvironmentLight can be used with in-game objects.

CLOCKTEXT:

	To use the ClockText, simply drag the ClockText prefab into your scene editor.
	
	This is optional and will display the current GameTime to the Camera.


------------------------------------------------------------------------------------------
Please look at the ExampleScene included in the project for additional help
