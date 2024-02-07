# Unity3DSpritePerspective
Simple perspective-based sprite animator system

[![YouTube video](https://img.youtube.com/vi/-OvK219uNsI/0.jpg)](https://www.youtube.com/watch?v=-OvK219uNsI)

Just a little system for Doom-style billboarded sprites that update based on the camera's perspective.

Sprite3DPerspective script swaps between sprites in a sprite array. Works for environmental objects.

PlayerAnimation script uses camera's perspective of the Player to set the active branch in the Player Animator (which is a series of Blendtrees). Script also implements deadzones to prevent cases where the camera's location would cause the Player sprite to flicker between perspectives.
