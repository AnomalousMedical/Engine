# TODO

## Don't forget mipmaps
Make / load mipmaps for things.

## Figure out why orientation has to be inverted from physics to rendering
This is all handled by the pbr renderer, but it is strange that rotations have to be inverted to render correctly.

## Figure out why camera position is backward
In order to move the camera through the world in the same coords as the rest the position must be inverted before setting the shader. This is
handled in pbr camera and lights.

## Fix physics timestep
use a fixed timestep with an accumulator, interpolate in between values
keep everything position seen on this frame, throw away results from last frame use this to interpolate unless its too old then just use the current value. assumption is nothing cared about its position until just now
otherwise can we make a copy of the whole simulation since it uses a buffer?

## Figure out if you can keep BodyReference or if BodyHandle should get a new one each time
You can use one BodyReference and its position will change, but some bepu demos show getting it each time. Figure this out.

## Get rid of custom file modes for virtual file system
Engine.Resources.FileMode

## Add wrapping support for material textures
Right now the material textures only work if the dest size is smaller than the source size
make it so the source can be wrapped to get pixels that would lay outside it, the groundwork is there