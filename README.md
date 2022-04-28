# Module Controller
![Module Controller](https://repository-images.githubusercontent.com/428422036/b9c2a601-b3fb-4893-b3d5-c6f2ebb6548d)

## What is it?
*Module Controller* helps you piece your *Character Controllers*, *Vehicle Controllers*, and others into smaller modular pieces (aka *Modules*). This helps you keep your scripts more focused and makes it easier to modify and add new features.

## Getting started
1. Clone or download the repository.
2. Copy **ModuleController** folder from **Plugins** folder to your project.
3. Take a look at the included *Demos* to see examples of how you can introduce a bit more modularity into your *Character Controllers* and such.
4. Done!

Developed in **Unity 2020.3.23f1**, but I don't see why it wouldn't work in any Unity version.

## How To Use
1. Create new scripts for your *Module Controller* (named *FooModuleController*, for example) and your *Module base class* (named *AbstractFooModule*, for example).
2. If your classes are *MonoBehaviours*, you can inherit from **ModuleControllerBehaviour** and **ModuleBehaviour**, which are also *MonoBehaviours* and include all the needed boilerplate code. Alternatively, you can implement **IModuleController** and **IModule** interfaces to customize them further.
3. In your *Module Controller* script, add `SetupModules();` to your **Start** function and `UpdateModules(Time.deltaTime);` to your **Update** or **FixedUpdate** function.
4. Make new scripts for your *Modules* (named *FooBarModule, FooBazModule*, etc) and make them inherit from your *Module base class*. You can override your module's **SetupModule** function and add `Debug.Log($"Hello {GetType().Name}");` to confirm that they are setup.
5. Make a new *GameObject* and add your new *Module Controller* and *Module* scripts to it.
6. Press play!

## Demos
### Basic
A basic FPS Character Controller with modular abilities.
![Basic Demo](https://user-images.githubusercontent.com/54811990/165590604-8a1f0780-0ecc-4fb3-9448-b8de949f176f.png)
### Advanced
3rd Person Character Controller with simple IK.
![Advanced Demo](https://user-images.githubusercontent.com/54811990/165590645-6d7fcb1d-31dd-408c-8b01-883bc8ecbf4c.png)
### Vehicle
Vehicle Controller with multiple Module Controllers and submodules.
![Vehicle Demo](https://user-images.githubusercontent.com/54811990/165590693-a356b0db-aef9-48dd-9141-bf4886ffa992.png)

## Notes
- *Modules* can also be *Module Controllers* and have their own submodules. Just implement **IModuleController** in your *Module*, add all the necessary boilerplate code and make scripts for its *Modules*.
- Adding and removing *Modules* during *runtime* is currently not implemented.

## Assets
#### Models
[Car Kit](https://www.kenney.nl/assets/car-kit)
[Racing Kit](https://www.kenney.nl/assets/racing-kit)
[3rd Person Character](https://assetstore.unity.com/packages/essentials/starter-assets-third-person-character-controller-196526)
#### Sounds
[Engine](https://freesound.org/people/cr4sht3st/sounds/157144/)
[Skid](https://freesound.org/people/audible-edge/sounds/71739/)
[Impact1](https://freesound.org/people/Halleck/sounds/121622/)
[Impact2](https://freesound.org/people/Halleck/sounds/121657/)
[Impact3](https://freesound.org/people/Halleck/sounds/121656/)
