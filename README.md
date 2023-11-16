# Unity3DSpritePerspective
Simple perspective-based sprite animator system

[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/9g2B7B2KuoI/0.jpg)](https://www.youtube.com/watch?v=9g2B7B2KuoI)

Just a little system for creating Doom-style billboarded sprites that update based on the camera's perspective.

Sprite3DPerspective script swaps between sprites in a sprite array. Works for environmental objects.

PlayerAnimation script uses an Animator to apply animations based on the camera's perspective. Requires the Player component. Script also implements deadzones to prevent cases where the camera's location would cause the Player sprite to flicker between perspectives.
