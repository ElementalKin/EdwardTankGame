Controls:
w - move forward
s - move backwards
a - rotate left
d - rotate right
q - rotate turret left
e - rotate turret right
f - change turret type
space - shoot
left mouse button - speed up bullets
middle mouse button - stops the bullets
right mouse butoon - slow down bullets
left control - speed up tank speed and rotation
left shift - affect the left and right mouse buttons changing speed from 100 to 10.

Game Concepts:
The rectangles with bullets on them give you ammo when you collide with them.
Changing of the barrels on the tank changes the reload speed of the tank and the size of the bullets.
The targets and bullets do nothing right now besides give and example of moving a visual object using matrix3s.
Wrapping around the screen is just there for fun.

MathClasses:
The two math classes in the project is because the unit test wants a diffrent type of matrix3 than I am using in my project and it would have taken me more time to change all -
code in my game so I just added the math class I used to pass the unit test.